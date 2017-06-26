using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class Game
    {
        public List<Player> Players { get; set; } // will be set to private readonly
        public Player CurrentPlayer { get; set; }
        public bool AlreadyRolled { get; set; }

        public event EventHandler<PlayerMovedEventArgs> PlayerMoved;

        public Game(List<Player> players)
        {
            Players = players;
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

        public void OnDiceRolled(object sender, DiceEventArgs e)
        {
            CurrentPlayer.Move(e.Rolled1 + e.Rolled2);
            if (e.Rolled1 != e.Rolled2)
                AlreadyRolled = true;
            OnPlayerMoved();
        }

        public void OnChoseEndTurn(object sender, EventArgs e)
        {
            EndTurn();
        }

        protected virtual void OnPlayerMoved()
        {
            var otherPlayers = Players.FindAll(p => !p.CurrentPlayer);

            PlayerMoved?.Invoke(this, new PlayerMovedEventArgs() { Player = CurrentPlayer, OtherPlayers = otherPlayers });
        }
    }
}