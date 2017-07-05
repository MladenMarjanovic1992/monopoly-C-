using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Game // handles the current player's turn
    {
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public bool AlreadyRolled { get; set; }
        public bool GameOver { get; set; }

        private readonly List<IField> _map;
        private readonly Bankrupcy _bankrupcy;

        public Game(List<Player> players, List<IField> map, Bankrupcy bankrupcy)
        {
            Players = players;
            _map = map;
            _bankrupcy = bankrupcy;
            CurrentPlayer = Players[0];
            CurrentPlayer.CurrentPlayer = true;
        }

        public void EndTurn()
        {
            CurrentPlayer.CurrentPlayer = false;
            var currentPlayerIndex = Players.IndexOf(CurrentPlayer);

            if (currentPlayerIndex < Players.Count - 1)
            {
                CurrentPlayer = Players[currentPlayerIndex + 1];
                CurrentPlayer.CurrentPlayer = true;
            }
            else
            {
                CurrentPlayer = Players[0];
                CurrentPlayer.CurrentPlayer = true;
            }
            AlreadyRolled = false;
        }

        // dice event - when the dice are rolled
        public void OnDiceRolled(object sender, DiceEventArgs e)
        {
            // Rule: If the player is in jail, he can leave immediately if he rolls equal number dice
            while (CurrentPlayer.InJail && CurrentPlayer.RollsUntilOut > 0)
            {
                if (e.Rolled1 != e.Rolled2)
                {
                    e.Rolled1 = 0;
                    e.Rolled2 = 0;
                    CurrentPlayer.RollsUntilOut -= 1;
                }
                else
                {
                    CurrentPlayer.InJail = false;
                    CurrentPlayer.RollsUntilOut = 0;
                }
            }

            CurrentPlayer.Move(e.Rolled1 + e.Rolled2, _map);

            var otherPlayers = Players.Where(p => !p.CurrentPlayer).ToList();

            // Other players are needed for auctions, bankrupcies and cards which affect all players
            _map[CurrentPlayer.Position].FieldEffect(CurrentPlayer, otherPlayers);

            // Rule: Player rolls again if both dice are the same number
            if (e.Rolled1 != e.Rolled2)
                AlreadyRolled = true;

            // Checks if the player is bankrupt after FieldEffect
            if (_bankrupcy.IsBankrupt(CurrentPlayer))
            {
                Console.WriteLine($"{CurrentPlayer.PlayerName.ToUpper()} is bankrupt");
                _bankrupcy.Liquidate(CurrentPlayer, otherPlayers, true);
            }
        }

        // Event for GoToJail field
        public void OnWentToJail(object sender, EventArgs e)
        {
            CurrentPlayer.Move(-(CurrentPlayer.Position - _map.FindIndex(f => f.FieldName == "Jail")), _map);
            CurrentPlayer.InJail = true;
            CurrentPlayer.RollsUntilOut = 3; // Rule: Player stays in jail for 3 turns
        }

        // Event triggered by paying for a Card drawn by the current player (for instance "Doctor's fee - Pay 50$")
        public void OnPayedForCard(object sender, PayedForCardEventArgs e)
        {
            // Checks the player who payed if he is bankrupt (not necessarily the current player)
            if (_bankrupcy.IsBankrupt(e.PlayerLiable))
            {
                Console.WriteLine($"{e.PlayerLiable.PlayerName.ToUpper()} is bankrupt");
                _bankrupcy.Liquidate(e.PlayerLiable, new List<Player>(){ e.StakeHolder }, false);
            }
        }

        public void OnPlayerLiquidated(object sender, PlayerLiquidatedEventArgs e)
        {
            if(e.BankruptDuringOwnTurn)
                EndTurn();

            Players.Remove(e.PlayerLiquidated);

            if (Players.Count == 1)
            {
                Console.WriteLine($"{Players[0].PlayerName.ToUpper()} has won the game!");
                GameOver = true;
            }
        }

        // choice events
        public void OnChoseEndTurn(object sender, EventArgs e)
        {
            if(CurrentPlayer.Money >= 0)
                EndTurn();
            else
                Console.WriteLine($"You owe {-CurrentPlayer.Money}$! Sell houses or mortgage properties to settle your debt!");
        }

        public void OnChoseQuitGame(object sender, EventArgs e)
        {
            GameOver = true;
        }
    }
}