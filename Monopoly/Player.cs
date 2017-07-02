using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class Player
    {
        public string PlayerName { get; }
        public int Money { get; set; }
        public int Position { get; set; }
        public bool CurrentPlayer { get; set; }
        public bool InJail { get; set; }
        public int RollsUntilOut { get; set; }
        public int GetOutOfJailCards { get; set; }
        private IField _field;

        public Player(string name, int money, IField startField)
        {
            PlayerName = name;
            Money = money;
            _field = startField;
        }

        public void Move(int rolled, List<IField> map)
        {
            if (Position + rolled < 40)
                Position += rolled;
            else
            {
                Position += rolled - 40;
                Money += 200;
            }

            _field = map[Position];
        }

        public void PrintStats()
        {
            Console.WriteLine();
            Console.WriteLine($"Name: {PlayerName.ToUpper()}");
            Console.WriteLine($"Money: {Money}");
            Console.WriteLine($"Position: {_field.FieldName}");
            Console.WriteLine();
        }

    }
}