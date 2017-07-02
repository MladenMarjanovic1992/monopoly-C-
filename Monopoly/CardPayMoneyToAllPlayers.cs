using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class CardPayMoneyToAllPlayers : ICard
    {
        public int Ammount { get; set; }
        public string CardMessage { get; set; }

        public void DrawCard(Player player, List<Player> otherPlayers)
        {
            Console.WriteLine(CardMessage);

            foreach (var p in otherPlayers)
            {
                player.Money += Ammount;
                p.Money -= Ammount;

                if (Ammount > 0)
                {
                    OnPayedForCard(p, player);
                }
            }
        }

        public event EventHandler<PayedForCardEventArgs> PayedForCard;

        protected virtual void OnPayedForCard(Player playerLiable, Player stakeHolder)
        {
            PayedForCard?.Invoke(this, new PayedForCardEventArgs(){PlayerLiable = playerLiable, StakeHolder = stakeHolder});
        }
    }
}