using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommandBasedGame;


namespace CommandBasedGame
{
    public class Map
    {
        private readonly Location[,] _map;
        private readonly Game _game;
        private bool _triedRiddle;

        public Map(Game game, int width, int height)
        {
            _game = game;
            _map = new Location[width, height];
            InitializeMap();
        }

        private void InitializeMap()
        {
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    Item item = null;
                    string locationName = $"Location_{x}_{y}";

                    if ((x == -1 && y == 1))
                    {
                        locationName = "Wizard";
                        item = new Item("Wizard");
                    }
                    else if (x == 1 && y == 2)
                    {
                        locationName = "Book";
                        item = new Item("Book");
                    }

                    _map[x + 2, y + 2] = new Location(x, y, locationName, item);
                }
            }
        }

        public void TakeItem(Player player, Tuple<int, int> coordinates)
        {
            Location currentLocation = GetLocation(coordinates);

            if (currentLocation != null)
            {
                if (currentLocation.Item != null)
                {
                    if (currentLocation.Item.Name == "Book")
                    {
                        Console.WriteLine("You took a book. It seems to be about rice.");
                    }
                    else
                    {
                        Console.WriteLine($"You took the {currentLocation.Item.Name}!");
                    }

                    player.Inventory.Add(currentLocation.Item);
                    currentLocation.RemoveItem();

                    if (currentLocation.Item != null && currentLocation.Item.Name == "Coin")
                    {
                        _game.CoinTaken = true;
                        Console.WriteLine("You now have a coin in your possession.");
                    }
                }
                else
                {
                    Console.WriteLine("There is nothing to take here.");
                }
            }
            else
            {
                Console.WriteLine("You are at the edge of the map.");
            }
        }

        private Location GetLocation(Tuple<int, int> coordinates)
        {
            int x = coordinates.Item1;
            int y = coordinates.Item2;

            if (IsInsideBounds(x, y))
            {
                return _map[x + 2, y + 2];
            }

            return null;
        }

        private bool IsInsideBounds(int x, int y)
        {
            return x >= -2 && x <= 2 && y >= -2 && y <= 2;
        }

        public void CheckForLocation(Tuple<int, int> coordinates)
        {
            Location currentLocation = GetLocation(coordinates);

            if (currentLocation != null)
            {
                Console.WriteLine($"You are at {currentLocation}.");

                if (currentLocation.Item != null)
                {
                    Console.WriteLine($"There is a {currentLocation.Item.Name} here.");

                    if (currentLocation.Item.Name == "Wizard")
                    {
                        Console.WriteLine("There is a wizard. Do you want to talk with him? (Yes/No)");

                        string talkChoice = Console.ReadLine()?.ToLower();

                        if (talkChoice == "yes")
                        {
                            Console.WriteLine("Hey Traveller. I am the Great Wizard, protector of the Mysterious Land. " +
                                              "If you want to journey to the Mysterious Land, you have to answer my riddle correctly. " +
                                              "If you answer correctly, you can travel to the Mysterious Land, but if you answer incorrectly, " +
                                              "you will suffer the consequences. The choice is yours.");

                            Console.WriteLine("Do you want to try and answer my riddle? (Yes/No)");

                            string answerChoice = Console.ReadLine()?.ToLower();

                            if (answerChoice == "yes")
                            {
                                if (_game.Player.Inventory.Any(item => item.Name == "Book"))
                                {
                                    Console.WriteLine("I can be long or can be short, I can be black, white, brown, or purple. " +
                                                      "You can find me the world over and I am often the main feature. What am I?");
                                    Console.WriteLine("1) Pumpkin");
                                    Console.WriteLine("2) Chocolate");
                                    Console.WriteLine("3) Flower");
                                    Console.WriteLine("5) Rice");

                                    Console.WriteLine("Which one is the correct answer, traveller?");
                                    string riddleAnswer = Console.ReadLine();

                                    if (riddleAnswer == "5")
                                    {
                                        Console.WriteLine("Correct answer! Now you can travel to the land of mysteries. You Won!");
                                        _game.CoinTaken = false;
                                        _game.Player.Inventory.Clear();
                                        _game.PlayerX = 0;
                                        _game.PlayerY = 0;
                                        _game.StartGame();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong answer. Great Wizard killed you. GAME OVER");
                                        _game.CoinTaken = false;
                                        _game.Player.Inventory.Clear();
                                        _game.PlayerX = 0;
                                        _game.PlayerY = 0;
                                        _game.StartGame();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("I can be long or can be short, I can be black, white, brown, or purple. " +
                                                      "You can find me the world over and I am often the main feature. What am I?");
                                    Console.WriteLine("1) Pumpkin");
                                    Console.WriteLine("2) Chocolate");
                                    Console.WriteLine("3) Flower");
                                    Console.WriteLine("4) Monkey");

                                    if (_game.CoinTaken)
                                    {
                                        Console.WriteLine("5) Offer coin to the Great Wizard");
                                    }

                                    Console.WriteLine("Which one is the correct answer, traveller?");
                                    string riddleAnswer = Console.ReadLine();

                                    if (riddleAnswer == "5")
                                    {
                                        Console.WriteLine("Correct answer! You can now travel to the Mysterious Realm.");
                                    }
                                    else if (riddleAnswer == "4" && _game.CoinTaken)
                                    {
                                        Console.WriteLine("You offered the coin to the Great Wizard. You can not buy me. Great Wizard killed you. GAME OVER");
                                        _game.CoinTaken = false;
                                        _game.Player.Inventory.Clear();
                                        _game.PlayerX = 0;
                                        _game.PlayerY = 0;
                                        _game.StartGame();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong answer. I will give you another chance to answer correctly. " +
                                                          "Think and come back.");
                                        Console.WriteLine("Do you want to try again? (Yes/No)");

                                        string retryChoice = Console.ReadLine()?.ToLower();
                                        if (retryChoice == "yes")
                                        {
                                            _triedRiddle = true;
                                            Console.WriteLine("Alright, here's your second chance.");
                                            Console.WriteLine("1) Pumpkin");
                                            Console.WriteLine("2) Chocolate");
                                            Console.WriteLine("3) Flower");
                                            Console.WriteLine("4) Monkey");

                                            if (_game.CoinTaken)
                                            {
                                                Console.WriteLine("4) Offer coin to the Great Wizard");
                                            }

                                            Console.WriteLine("Which one is the correct answer, traveller?");
                                            string secondAttempt = Console.ReadLine();

                                            if (secondAttempt == "5")
                                            {
                                                Console.WriteLine("Correct answer! You can now travel to the Mysterious Land.");
                                            }
                                            else if (secondAttempt == "8" && _game.CoinTaken)
                                            {
                                                Console.WriteLine("You offered the coin to the Great Wizard. You can not buy me. Great Wizard killed you. GAME OVER");
                                                _game.CoinTaken = false;
                                                _game.Player.Inventory.Clear();
                                                _game.PlayerX = 0;
                                                _game.PlayerY = 0;
                                                _game.StartGame();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Wrong answer. Great Wizard killed you. GAME OVER");
                                                _game.CoinTaken = false;
                                                _game.Player.Inventory.Clear();
                                                _game.PlayerX = 0;
                                                _game.PlayerY = 0;
                                                _game.StartGame();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("You decided not to try again. You can continue your journey.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("You decided not to try the riddle. You can continue your journey.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You chose not to talk with the wizard. You can continue your journey.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You are at the edge of the map.");
                }
            }
        }

        public void MovePlayer(int xOffset, int yOffset)
        {
            int newPlayerX = _game.PlayerX + xOffset;
            int newPlayerY = _game.PlayerY + yOffset;

            if (IsInsideBounds(newPlayerX, newPlayerY))
            {
                _game.PlayerX = newPlayerX;
                _game.PlayerY = newPlayerY;
            }
            else
            {
                Console.WriteLine("You are at the edge of the map.");
            }

            CheckForLocation(GetCoordinates());
        }

        public Tuple<int, int> GetCoordinates()
        {
            return new Tuple<int, int>(_game.PlayerX, _game.PlayerY);
        }

        public bool CanContinueJourney()
        {
            return !_triedRiddle;
        }
    }
}

