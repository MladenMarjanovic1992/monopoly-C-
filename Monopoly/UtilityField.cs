using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class UtilityField : IFieldRentable
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }

        public Player Owner { get; set; }
        public int Price { get; set; }
        public bool UnderMortgage { get; set; }
        public int[] Rent { get; set; }
        public int CurrentRent { get; set; }
        public string Color { get; set; }
        public bool CanMortgage { get; set; }

        public UtilityField()
        {
            Rent = new int[2];
            CanMortgage = true;
        }

        public int MortgageValue => Price / 2;

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            PrintFieldStats();

            if (Owner != null && Owner != currentPlayer && !UnderMortgage)
            {
                var diceRoll = new Random();
                var payedSum = CurrentRent * (diceRoll.Next(1, 7) + diceRoll.Next(1, 7))

                currentPlayer.Money -= payedSum;
                Owner.Money += payedSum;

                Console.WriteLine($"Payed: {payedSum}");
            }
            else if (Owner == null)
            {
                var buysField = Prompt.YesOrNo("Buy field? (y/n)") && currentPlayer.Money >= Price;

                if (buysField)
                {
                    var trade = new Trade();
                    trade.BuyField(currentPlayer, this, Price);
                }
                else
                {
                    Console.WriteLine("Auction!");
                    PrintFieldStats();
                    var trade = new Trade();
                    var highestBidder = Prompt.ChoosePlayer(otherPlayers, "Which player had the highest bid (enter number)?");
                    var highestBid = Prompt.EnterAmount(highestBidder, "What was the winning bid?");

                    trade.BuyField(highestBidder, this, highestBid);
                }
            }
        }

        public void PrintFieldStats()
        {
            Console.WriteLine();
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine($"Price: {Price}");
            Console.WriteLine($"Mortgage value: {MortgageValue}");
            Console.WriteLine("Rent: 4 x Dice roll if owned, 10 x Dice roll if monopoly");
            Console.WriteLine("Owner: {0}", Owner == null ? "Not owned" : Owner.PlayerName);
            Console.WriteLine("Under mortgage: {0}", UnderMortgage ? "Yes" : "No");
            Console.WriteLine();
        }

        public void OnPlayerMoved(object sender, PlayerMovedEventArgs e)
        {
            if (FieldIndex == e.Player.Position)
            {
                e.Player.FieldPosition = FieldName;
                FieldEffect(e.Player, e.OtherPlayers);
            }
        }
    }
}