using System;

namespace Monopoly
{
    public class Dice
    {
        private readonly Random _rolled = new Random();

        public void Roll()
        {
            var roll1 = _rolled.Next(1, 7);
            var roll2 = _rolled.Next(1, 7);

            PrintRoll(roll1 + roll2);

            OnDiceRolled(roll1, roll2);
        }

        public event EventHandler<DiceEventArgs> DiceRolled;

        private void PrintRoll(int roll)
        {
            Console.WriteLine("Rolled: " + roll);
        }

        public void OnChoseRoll(object sender, EventArgs e)
        {
            Roll();
        }

        protected virtual void OnDiceRolled(int rolled1, int rolled2)
        {
            DiceRolled?.Invoke(this, new DiceEventArgs() { Rolled1 = rolled1, Rolled2 = rolled2 });
        }
    }
}