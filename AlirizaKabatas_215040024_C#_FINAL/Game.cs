using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommandBasedGame
{
    public class Game
    {
        private const string SaveFileName = "saved_game.txt";
        private const int MapWidth = 5;
        private const int MapHeight = 5;

        private bool _gameRunning;
        private Map _gameMap;
        private string _playerInput;
        private string _playerName;
        private Player _player;
        private bool _coinTaken;

        public bool CoinTaken
        {
            get { return _coinTaken; }
            set { _coinTaken = value; }
        }

        public Player Player
        {
            get { return _player; }
            private set { _player = value; }
        }

 
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }

        public void StartGame()
        {
            Console.WriteLine("Enter your name before starting the game:");
            _playerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(_playerName))
            {
                Console.WriteLine("Player name not entered, giving the name Traveller");
                _playerName = "Traveller";
            }
            else
            {
                Console.WriteLine($"Nice to meet you {_playerName}, let's start a great adventure together!");
            }

            CreateNewMap();
            CreatePlayer();

            InitializeGameConditions();

            _gameRunning = true;
            StartGameLoop();
        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, MapWidth, MapHeight);
        }

        private void CreatePlayer()
        {
            PlayerX = 0;
            PlayerY = 0;

            Player = new Player(_playerName, new List<Item>());
        }

        private void InitializeGameConditions()
        {
            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                GetInput();
                ProcessInput();
            }
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
        }

        private void ProcessInput()
        {
            if (string.IsNullOrWhiteSpace(_playerInput))
            {
                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput.ToLower())
            {
                case "n":
                    _gameMap.MovePlayer(0, 1);
                    Console.WriteLine("You moved north.");
                    break;
                case "s":
                    _gameMap.MovePlayer(0, -1);
                    Console.WriteLine("You moved south.");
                    break;
                case "e":
                    _gameMap.MovePlayer(1, 0);
                    Console.WriteLine("You moved east.");
                    break;
                case "w":
                    _gameMap.MovePlayer(-1, 0);
                    Console.WriteLine("You moved west.");
                    break;
                case "exit":
                    Console.WriteLine("We hope you enjoyed our game!");
                    _gameRunning = false;
                    break;
                case "save":
                    SaveGame();
                    Console.WriteLine("Game saved");
                    break;
                case "load":
                    LoadGame();
                    Console.WriteLine("Game loaded");
                    break;
                case "help":
                    DisplayHelp();
                    break;
                case "take":
                    _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    break;
            }
        }

        private void SaveGame()
        {
            using (StreamWriter writer = new StreamWriter(SaveFileName))
            {
                writer.WriteLine(_playerName);

                foreach (Item item in Player.Inventory)
                {
                    writer.WriteLine(item.Name);
                }

                writer.WriteLine(PlayerX);
                writer.WriteLine(PlayerY);
                writer.WriteLine(_coinTaken);
            }
        }

        private void LoadGame()
        {
            if (File.Exists(SaveFileName))
            {
                using (StreamReader reader = new StreamReader(SaveFileName))
                {
                    _playerName = reader.ReadLine();

                    Player.Inventory.Clear();
                    string itemName;
                    while ((itemName = reader.ReadLine()) != null)
                    {
                        Player.Inventory.Add(new Item(itemName));
                    }

                    PlayerX = int.Parse(reader.ReadLine());
                    PlayerY = int.Parse(reader.ReadLine());
                    _coinTaken = bool.Parse(reader.ReadLine());

                    Console.WriteLine("Game loaded successfully.");
                }
            }
            else
            {
                Console.WriteLine("No saved game found.");
            }
        }

        private void DisplayHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("N - Move north: Move your character one step to the north.");
            Console.WriteLine("S - Move south: Move your character one step to the south.");
            Console.WriteLine("E - Move east: Move your character one step to the east.");
            Console.WriteLine("W - Move west: Move your character one step to the west.");
            Console.WriteLine("exit - Exit the game: Quit the game.");
            Console.WriteLine("save - Save the game: Save your current progress in the game.");
            Console.WriteLine("load - Load the saved game: Resume the game from a previous save.");
            Console.WriteLine("Type 'help' for a list of available commands.");
        }
    }
}