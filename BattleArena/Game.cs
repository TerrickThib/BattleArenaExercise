﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{   

    public enum ItemType
    {
        DEFENSE, 
        ATTACK,
        NONE
    }

    public enum Scene
    {
        STARTMENU,
        NAMECREATION,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU

    }
    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;        
    }
    class Game
    {
        private bool _gameOver;
        private Scene _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex = 0;
        private Entity _currentEnemy;
        private string _playerName;
        private Item[] _tankItems;
        private Item[] _hunterItems;

        int[] AppendToArray(int[]arr, int value)
        {
            //Creat a new array with one more slot than the old array
            int[] newArray = new int[arr.Length + 1];

            //Copy the values from the old array into the new array
            for (int i = 0; i < arr.Length; i++)
            {
                newArray[i] = arr[i];
            }

            //Set the last index to be the new item
            newArray[newArray.Length - 1] = value;

            //REturn the new array
            return newArray;
        }
                                      
        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            int[] numbers = new int[] { 1, 2, 3, 4 };

            numbers = AppendToArray(numbers, 5);
            Start();
            while (!_gameOver)
            {
                Update();
               
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;
            InitalizeEnemies();
            InitalizeItems();
        }

        public void InitalizeItems()
        {
            //Tank Items
            Item bigStick = new Item { Name = "Big Stick", StatBoost = 5, Type = ItemType.ATTACK };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, Type = ItemType.DEFENSE };

            //Hunter Items
            Item bow = new Item { Name = "Bow", StatBoost = 10, Type = ItemType.ATTACK };
            Item boots = new Item { Name = "Boots", StatBoost = 90, Type = ItemType.DEFENSE };

            //Initialize arrays
            _tankItems = new Item[] { bigStick, bigShield };
            _hunterItems = new Item[] { bow, boots };
        }
        

        public void InitalizeEnemies()
        {
            _currentEnemyIndex = 0;

            //Enemie1
            Entity goblin = new Entity("Goblin", 10.0f, 2.0f, 5.0f);

            //Enemie2
            Entity muscleman = new Entity("Muscleman", 20.0f, 15.0f, 10.0f);

            //Enemie3
            Entity toad = new Entity("Toad", 15.0f, 8.0f, 5.0f);

            _enemies = new Entity[] { goblin, muscleman, toad };

            _currentEnemy = _enemies[_currentEnemyIndex];
        }
        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
            
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Guh Byeeee");
            Console.ReadKey();
        }

        public void Save()
        {
            //Create a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            //Saves current enemy index
            writer.WriteLine(_currentEnemyIndex);

            //Saves player and enemys stats
            _player.Save(writer);
            _currentEnemy.Save(writer);

            //Closes writer when done saving
            writer.Close();
        }

        public bool Load()
        {
            //If File Doesnot exist...
            if (!File.Exists("SaveData.txt"))
                //return false
                return false;

            //Create a new reader to read from the text file
            StreamReader reader = new StreamReader("SaveData.txt");

            //If the first line can't be converted into an integer...
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
                //return false
                return false;

            //Load player job
            string job = reader.ReadLine();

            //Assign player his job
            if (job == "Tank")
                _player = new Player(_tankItems);
            else if (job == "Hunter")
                _player = new Player(_hunterItems);
            else
                return false;

            _player.Job = job;

            //Create a new instance and try to load the player
            if (!_player.Load(reader))
                return false;

            //Create a new instance and try to load the enemy
            _currentEnemy = new Entity();
            if (!_currentEnemy.Load(reader))
                return false;

            //Update the array to match the current enemy stats
            _enemies[_currentEnemyIndex] = _currentEnemy;

            _currentScene = Scene.BATTLE;
            //Close the reader once loading is finished
            reader.Close();

            return true;
        }
        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

           while (inputReceived == -1)
            {
                //Print all of are options
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);                    
                }
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    //...decrement the input and check if it's within the bounds of the array if they choose 1 they atwally choose 0 in the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        //Set input received to be the default value
                        inputReceived = -1;
                        //Display error message
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }  
                //If the player didn't type an int
                else
                {
                    //Set input received to be the default value
                    inputReceived = -1;
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey(true);
                }

                Console.Clear();
            }

            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            switch(_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;
                case Scene.NAMECREATION:
                    GetPlayerName();
                    break;
                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    break;
                case Scene.BATTLE:
                    Battle();
                    CheckBattleResults();
                    break;
                case Scene.RESTARTMENU:
                    DisplayMainMenu();
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            //Gets players Choice
            int Input = GetInput("Play Again", "Yes ", "No");

            if (Input == 0)
            {
                _currentScene = 0;
                InitalizeEnemies();
            }
            else if(Input == 1)
            {
                _gameOver = true;
            }
            
            
        }

        public void DisplayStartMenu()
        {
            int choice = GetInput("Welcome to Battle Arena!", "Start New Game", "Load Game");

            if (choice == 0)
            {
                _currentScene = Scene.NAMECREATION;
            }
            else if (choice == 1)
            {
                if(Load())
                {
                    Console.WriteLine("Load Successful!");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Welcome To FightCLub!! First Rule of fight club dont talk about fight club");
            Console.WriteLine("So whats your name? ");
            _playerName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine(_playerName);

            int Input = GetInput("Are You sure? ", "Yes ", "No");
            if (Input ==0)
            {
                _currentScene++;
            }
            else if (Input ==1)
            {
                GetPlayerName();
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {            
            int Input = GetInput("Pick a Character. ", "Tank ", "Hunter");
            if(Input == 0)
            {
                _player = new Player(_playerName, 50, 15, 30, _tankItems, "Tank");
                _currentScene++;
            }
            else if (Input == 1)
            {
                _player = new Player(_playerName, 30, 25, 20, _hunterItems, "Knight");
                _currentScene++;
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine("Name: " + character.Name);
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("AttackPower: " + character.AttackPower);
            Console.WriteLine("DefensePower: " + character.DefensePower);
            Console.WriteLine();
        }

        public void DisplayEquipItemMenu()
        {
            //Gets item index
            int choice = GetInput("Select an item to equip.", _player.GetItemNames());

            //Equips item at given index
            if (!_player.TryEquipItem(choice))
                Console.WriteLine("You couldn't find that item in your bag.");

            //Prints feedback
            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
        }
       
        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            DisplayStats(_player);
            DisplayStats(_currentEnemy);

            int input = GetInput("A " + _currentEnemy.Name + " stands in front of you! What will you do?", "Attack", "Equip Item", "Remove current item", "Save Game");
            
            if (input == 0)
            {
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");
            }
            else if (input == 1)
            {
                DisplayEquipItemMenu();
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else if(input == 2)
            {
                if (!_player.TryRemoveCurrentItem())
                    Console.WriteLine("You don't have anything equipped.");
                else
                    Console.WriteLine("You placed the item in your bag. ");

                Console.ReadKey(true);
                Console.Clear();
                return;                
            }
            else if (input == 3)
            {
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine("The " + _currentEnemy.Name + " dealt" + damageDealt, " damage!");

            Console.ReadKey(true);
            Console.Clear();
        }
        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if(_player.Health <=0)
            {
                Console.WriteLine("You Fainted, Fight CLub Pass Revoked");
                Console.ReadKey(true);
                Console.Clear();
                _currentScene++;
            }
            else if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You Beat " + _currentEnemy.Name);
                Console.ReadKey();
                Console.Clear();
                _currentEnemyIndex++;

                if(_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene++;
                    Console.WriteLine("You killed everyone even Bill GOD WHY BILL WAS INCENT HE WAS GOOD. ");
                    return;
                }

                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }        
    }
}
