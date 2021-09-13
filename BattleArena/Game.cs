using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        bool gameOver;
        int currentScene;
        Character player;
        Character[] enemies;
        private int currentEnemyIndex = 0;
        private Character currentEnemy;
        string name = "";

        //Monsters
        Character goblin;
        Character muscleman;
        Character toad;

        //Player choice
        Character tank;
        Character hunter;

        
        
               

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {                        
          Start();
            while (!gameOver)
            {
                DisplayMainMenu();
                GetPlayerName();
                CharacterSelection();
                Console.Clear();
                Battle(ref player, ref goblin);
                End();
            } 
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            //Character Choice 1
            tank.name = "Tank" + (name);
            tank.health = 50.0f;
            tank.attackPower = 15;
            tank.defensePower = 30;

            //Character Choice 2
            hunter.name = "Hunter" + (name);
            hunter.health = 30.0f;
            hunter.attackPower = 25.0f;
            hunter.defensePower = 20.0f;

            //Enemie1
            goblin.name = "Goblin";
            goblin.health = 10.0f;
            goblin.attackPower = 2.0f;
            goblin.defensePower = 5.0f;

            //Enemie2
            muscleman.name = "Muscleman";
            muscleman.health = 30.0f;
            muscleman.attackPower = 15.0f;
            muscleman.defensePower = 10.0f;

            //Enemie3
            toad.name = "Toad";
            toad.health = 15.0f;
            toad.attackPower = 8.0f;
            toad.defensePower = 5.0f;

            
        }
        
        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            //Ask if you want to play again or end game.
            int Input = GetInput("Would you like to play again? ", "Yes", "No");
            if (Input ==1)
            {
                gameOver = false;
            }
            else if (Input ==2)
            {
                gameOver = true;
            }
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
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
            currentScene = 0;
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            //Gets players Choice
            int Input = GetInput("Welcome to fight club ", "Start Game ", "Quit Game");
            if (Input == 1)
            {
                currentScene = 1;
            }
            else if (Input == 2)
            {
                gameOver = true;
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            currentScene = 1;
            Console.WriteLine("Welcome To FightCLub!! First Rule of fight club dont talk about fight club");
            Console.WriteLine("So whats your name? ");
            name = Console.ReadLine();
            Console.WriteLine(name);
            int Input = GetInput("Are You sure? ", "Yes ", "No");
            if (Input ==1)
            {
                currentScene = 2;
            }
            else if (Input ==2)
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
            currentScene = 2;
            int Input = GetInput("Pick a Character. ", "Tank ", "Hunter");
            if(Input == 1)
            {
                player = tank;
                DisplayStats(tank);
                currentScene = 3;
            }
            else if (Input == 2)
            {
                player = hunter;
                DisplayStats(hunter);
                currentScene = 3;
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine("Name: " + character.name);
            Console.WriteLine("Health: " + character.health);
            Console.WriteLine("AttackPower: " + character.attackPower);
            Console.WriteLine("DefensePower: " + character.defensePower);
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            float damage = attackPower - defensePower;

            if (damage <= 0)
            {
                damage = 0;
            }
            return damage;
        }
        float CalculateDamage(Character attacker, Character defender)
        {
            return attacker.attackPower - defender.defensePower;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public float Attack(ref Character attacker, ref Character defender)
        {
            float damageTaken = CalculateDamage(attacker, defender);
            defender.health -= damageTaken;
            return damageTaken;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle(ref Character player, ref Character goblin)
        {
            //Shows Players stats
            DisplayStats(player);
            
            //Shows enemys stats
            DisplayStats(goblin);
            
            //Character Attacks enemy
            float damageTaken = Attack(ref player, ref goblin);
            Console.WriteLine(goblin.name + " has taken " + damageTaken);

            //Enemy Attacks Player
            damageTaken = Attack(ref goblin, ref player);
            Console.WriteLine(player.name + " has taken " + damageTaken);
            
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            
        }

    }
}
