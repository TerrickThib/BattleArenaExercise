using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Entity
    {
        private string _name;
        private float _health;
        private float _attackPower;
        private float _defensePower;
        
        public string Name { get { return _name; } }
                        
        public float Health { get { return _health; } }

        public float AttackPower { get { return _attackPower; } }

        public float DefensePower { get { return _defensePower; } }
        

       public Entity(string name, float health, float attackPower, float defensePower)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
            _defensePower = defensePower;
        }
    }
}
