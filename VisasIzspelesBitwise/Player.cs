using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisasIzspelesBitwise
{
    class Player
    {
        public enum Role { Mazais, Lielais }; // Galds, 
        public string Name { get; private set; }
        public Player Next;

        public Player(string name)
        {
            Name = name;
        }
    }
}
