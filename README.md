# Text-Based Adventure Game

This is a simple console-based text adventure game written in C#. The player navigates through different locations, interacts with items, solves puzzles, and progresses through the game world. The game also supports saving and loading progress.

## Features

- **Navigate through locations**: Use commands like `go north`, `go south`, etc., to move between rooms.
- **Interact with items**: You can `take` and `use` items within the game.
- **Look around**: View descriptions of the environment and examine objects.
- **Solve puzzles**: Solve simple puzzles, such as unscrambling words.
- **Save and Load**: Save your game progress and load it later to pick up where you left off.
  
## Game Commands

Here are the primary commands you can use in the game:

- **Movement**: 
  - `go <direction>` or `move <direction>` (e.g., `go north`, `move south`)
  
- **Inventory Management**:
  - `take <item>` (e.g., `take key`) – Pick up an item from the environment.
  - `use <item>` (e.g., `use key`) – Use an item in your inventory.

- **Look Around**:
  - `look around` – Get a description of the current location.
  - `examine <item>` (e.g., `examine key`) – Look closer at a specific item in the location.

- **Puzzles**:
  - `unscramble <scrambled word>` (e.g., `unscramble yrtrewq`) – Unscramble a word to reveal the correct message.

- **Quit Game**:
  - `quit` – Save the game and exit.

## How to Play

1. **Start a New Game or Load Saved Game**:
   When you first start the game, you can choose to start a new game or load a previously saved game:
   - Type `load` to load a saved game.
   - Type `start` to start a new game.
   
2. **Game Objective**:
   Explore different locations, collect items, and use them to solve puzzles. The game progresses as you solve challenges, and the goal is to reach the final room ("NextRoom").

3. **Saving and Loading**:
   - The game automatically saves your progress when you type `quit`.
   - You can load the saved game by typing `load` at the start.

## How to Run the Game

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/text-based-adventure-game.git
   ```

2. Open the project in your preferred C# IDE (e.g., Visual Studio, Visual Studio Code).

3. Build and run the application:
   - In Visual Studio, press `Ctrl+F5` to run the game without debugging.
   - Or use the terminal with the following command:
     ```bash
     dotnet run
     ```

4. Follow the on-screen instructions to play the game.

## Code Overview

### Classes

- **`Program`**: The entry point of the game. It initializes and starts the `Game` class.
- **`Game`**: The main class responsible for managing game state, handling user commands, processing movement, item interaction, and game saving/loading.
- **`Location`**: Represents a location in the game world. It contains the name, description, exits (directions), and items found in the location.
- **`Regex` Patterns**: Used to handle and validate user input, such as movement commands, taking items, and solving puzzles.

### Key Methods

- **`Start()`**: Starts the game, loads saved progress if requested, or initializes a new game.
- **`ProcessCommand()`**: Handles user input and performs actions based on the recognized commands (movement, item interaction, etc.).
- **`UseItem()`**: Handles using an item (e.g., unlocking a door with a key).
- **`Unscramble()`**: A helper method that unscrambles a given word to solve puzzles.
- **`SaveGame()`**: Saves the player's current progress, including their location and inventory, to a file (`savegame.txt`).
- **`LoadGame()`**: Loads the saved progress from the file if it exists, otherwise starts a new game.

### Configuration

The game loads initial configuration from a hardcoded setup in the `LoadConfiguration()` method:
- **Rooms**: Initial locations like `StartingRoom`, `LockedRoom`, and `NextRoom`.
- **Items**: Items such as a `key` placed in different locations.
- **Exits**: Directions the player can move (e.g., north, south, east, west).

## Example Gameplay

```
Welcome to the Text-Based Adventure Game!
Type 'load' to load a saved game or 'start' to begin a new game.
> start

You are in a small room with a door to the north. There is a key on the floor.
COMMANDS: move, take, use, quit
> look around
You are in a small room with a door to the north. There is a key on the floor.

> take key
You have taken the key.

> move north
You are now in the LockedRoom.

> use key
You used the key to unlock the door.

You are now in the NextRoom. You have won!
```

## License

This project is open source and available under the MIT License. See the [LICENSE](LICENSE) file for more details.

---

### Notes:
- Replace `yourusername` with your actual GitHub username in the repository URL.
- If you plan to share or extend this game, you may want to add more locations, items, and puzzles in the `LoadConfiguration()` method to enhance gameplay.
