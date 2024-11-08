using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TextBasedAdventureGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private Dictionary<string, Location> locations;
        private Location currentLocation;
        private List<string> inventory;
        private const string configFilePath = "config.txt";
        private const string saveFilePath = "savegame.txt";
        private Regex movementRegex = new Regex(@"^(go|move) (north|south|east|west)$", RegexOptions.IgnoreCase);
        private Regex takeRegex = new Regex(@"^take (.+)$", RegexOptions.IgnoreCase);
        private Regex useRegex = new Regex(@"^use (.+)$", RegexOptions.IgnoreCase);
        private Regex lookRegex = new Regex(@"^(look around|examine (.+))$", RegexOptions.IgnoreCase);
        private Regex puzzleRegex = new Regex(@"^unscramble (.+)$", RegexOptions.IgnoreCase);

        public Game()
        {
            locations = new Dictionary<string, Location>();
            inventory = new List<string>();
            LoadConfiguration();
        }

        public void Start()
        {
            Console.WriteLine("Welcome to the Text-Based Adventure Game!");
            Console.WriteLine("Type 'load' to load a saved game or 'start' to begin a new game.");
            string command = Console.ReadLine().Trim().ToLower();
            Console.Clear();

            if (command == "load")
            {
                LoadGame();
            }
            else
            {
                currentLocation = locations["StartingRoom"]; // start from a default location
            }

            while (true)
            {
                Console.WriteLine(currentLocation.Description);
                Console.WriteLine("Enter a command:");
                command = Console.ReadLine().Trim();
                Console.Clear();

                if (command.ToLower() == "quit")
                {
                    Console.Clear();
                    SaveGame();
                    Console.WriteLine("Game saved. Goodbye!");
                    break;
                }

                ProcessCommand(command);
            }
        }

        private void ProcessCommand(string command)
        {
            Match match;

            if (movementRegex.IsMatch(command))
            {
                match = movementRegex.Match(command);
                string direction = match.Groups[2].Value.ToLower();
                if (currentLocation.Exits.ContainsKey(direction))
                {
                    currentLocation = locations[currentLocation.Exits[direction]];
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You can't go that way.");
                }
            }
            else if ((match = takeRegex.Match(command)).Success)
            {
                string item = match.Groups[1].Value.ToLower();
                if (currentLocation.Items.Contains(item))
                {
                    inventory.Add(item);
                    currentLocation.Items.Remove(item);
                    Console.Clear();
                    Console.WriteLine($"You have taken the {item}.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("There's no such item here.");
                }
            }
            else if ((match = useRegex.Match(command)).Success)
            {
                string item = match.Groups[1].Value.ToLower();
                if (inventory.Contains(item))
                {
                    UseItem(item);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You don't have that item.");
                }
            }
            else if ((match = lookRegex.Match(command)).Success)
            {
                if (match.Groups[1].Value.ToLower() == "look around")
                {
                    Console.WriteLine(currentLocation.Description);
                }
                else
                {
                    string objectName = match.Groups[2].Value.ToLower();
                    if (currentLocation.Items.Contains(objectName))
                    {
                        Console.Clear();
                        Console.WriteLine($"You see a {objectName}.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("There's nothing special about it.");
                    }
                }
            }
            else if ((match = puzzleRegex.Match(command)).Success)
            {
                string scrambledMessage = match.Groups[1].Value;
                Console.Clear();
                Console.WriteLine($"The unscrambled message is: {Unscramble(scrambledMessage)}");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("I don't understand that command.");
            }
        }

        private void UseItem(string item)
        {
            // item usage
            if (item == "key" && currentLocation.Name == "LockedRoom")
            {
                Console.Clear();
                Console.WriteLine("You used the key to unlock the door.");
                // unlocking logic
                currentLocation = locations["NextRoom"];
                Console.Clear();
                Console.WriteLine("You are now in the Next Room.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You can't use that item here.");
            }
        }

        private string Unscramble(string scrambled)
        {
            // reversing the string
            char[] array = scrambled.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        private void LoadConfiguration()
        {
            // configuration loading
            locations["StartingRoom"] = new Location
            {
                Name = "StartingRoom",
                Description = "You are in a small room with a door to the north. There is a key on the floor.\nCOMMANDS: move, take, use, quit",
                Exits = new Dictionary<string, string> { { "north", "LockedRoom" } },
                Items = new List<string> { "key" }
            };

            locations["LockedRoom"] = new Location
            {
                Name = "LockedRoom",
                Description = "You are in a locked room. There is a door to the south.\nCOMMANDS: move, take, use, quit",
                Exits = new Dictionary<string, string> { { "south", "StartingRoom" } },
                Items = new List<string> { }
            };

            locations["NextRoom"] = new Location
            {
                Name = "NextRoom",
                Description = "You are in the next room. You have won!",
                Exits = new Dictionary<string, string>(),
                Items = new List<string>()
            };
        }

        private void SaveGame()
        {
            using (StreamWriter writer = new StreamWriter(saveFilePath))
            {
                writer.WriteLine(currentLocation.Name);
                writer.WriteLine(string.Join(",", inventory));
            }
        }

        private void LoadGame()
        {
            if (File.Exists(saveFilePath))
            {
                using (StreamReader reader = new StreamReader(saveFilePath))
                {
                    string locationName = reader.ReadLine();
                    string inventoryItems = reader.ReadLine();
                    if (locations.ContainsKey(locationName))
                    {
                        currentLocation = locations[locationName];
                    }
                    inventory = new List<string>(inventoryItems.Split(','));
                }
            }
            else
            {
                Console.WriteLine("No save file found. Starting a new game.");
                currentLocation = locations["StartingRoom"];
            }
        }
    }

    class Location
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Exits { get; set; }
        public List<string> Items { get; set; }
    }
}
