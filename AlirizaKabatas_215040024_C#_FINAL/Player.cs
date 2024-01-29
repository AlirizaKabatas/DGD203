using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CommandBasedGame
{
    public class Player
    {
        public string Name { get; }
        public List<Item> Inventory { get; }

        public Player(string name, List<Item> inventory)
        {
            Name = name;
            Inventory = inventory;
        }
    }
}