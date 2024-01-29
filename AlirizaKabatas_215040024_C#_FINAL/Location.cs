using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBasedGame
{
    public class Location
    {
        public int X { get; }
        public int Y { get; }
        public string Name { get; }
        public Item Item { get; set; }

        public Location(int x, int y, string name, Item item = null)
        {
            X = x;
            Y = y;
            Name = name;
            Item = item;
        }

        public override string ToString()
        {
            return $"({X},{Y})";  // Parantez içinde (X,Y) formatında döndürüldü.
        }

        public void RemoveItem()
        {
            Item = null;
        }
    }
}
