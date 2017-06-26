using System;

namespace Monopoly
{
    public class Player
    {
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public int Position { get; set; }
        public string FieldPosition { get; set; }
        public bool CurrentPlayer { get; set; }

        public Player(string name, int money)
        {
            PlayerName = name;
            Money = money;
            FieldPosition = "Start";
        }

        public void Move(int rolled)
        {
            if (Position + rolled < 40)
                Position += rolled;
            else
                Position += rolled - 40;
        }

        public void PrintStats()
        {
            Console.WriteLine();
            Console.WriteLine($"Name: {PlayerName.ToUpper()}");
            Console.WriteLine($"Money: {Money}");
            Console.WriteLine($"Position: {FieldPosition}");
            Console.WriteLine();
        }

    }
}