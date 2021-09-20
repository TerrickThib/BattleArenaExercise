using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{   

    public enum ItemType
    {
        DEFENSE, 
        ATTACK,
        NONE
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
        private int _currentScene;
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
            Item bigStick = new Item { Name = "Big Stick", StatBoost = 5, ItemType = 1 };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, ItemType = 0 };

            //Hunter Items
            Item bow = new Item { Name = "Bow", StatBoost = 10, ItemType = 1 };
            Item boots = new Item { Name = "Boots", StatBoost = 90, ItemType = 0 };

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
            //Ask if you want to play again or end game.
            int Input = GetInput("Would you like to play again? ", "Yes", "No");
            if (Input ==0)
            {
                _gameOver = false;
            }
            else if (Input ==1)
            {
                _gameOver = true;
            }
            Console.ReadKey();
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
                case 0:
                    GetPlayerName();
                    break;
                case 1:
                    CharacterSelection();
                    break;
                case 2:
                    Battle();
                    CheckBattleResults();
                    break;
                case 3:
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
                _player = new Player(_playerName, 50, 15, 30, _tankItems);
                _currentScene++;
            }
            else if (Input == 1)
            {
                _player = new Player(_playerName, 30, 25, 20, _hunterItems);
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

            int input = GetInput("A " + _currentEnemy.Name + " stands in front of you! What will you do?", "Attack", "Equip Item", "Remove current item");
            
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
                _currentScene = 3;
            }
            else if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You Beat " + _currentEnemy.Name);
                Console.ReadKey();
                Console.Clear();
                _currentEnemyIndex++;

                if(_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene = 3;
                    Console.WriteLine("You killed everyone even Bill GOD WHY BILL WAS INCENT HE WAS GOOD. ");
                    return;
                }

                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }        
    }
}
