using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class ChanceField : IField
    {
        public string FieldName { get; set; }
        public int FieldIndex { get; set; }
        public Cards Cards { get; set; }

        public void FieldEffect(Player currentPlayer, List<Player> otherPlayers)
        {
            Console.WriteLine("Chance!");

            Cards[Cards.CardsDrawn].DrawCard(currentPlayer, otherPlayers);
        }

        public void PrintFieldStats()
        {
            Console.WriteLine($"Field: {FieldName}");
            Console.WriteLine($"Field position: {FieldIndex}");
            Console.WriteLine();
        }
    }

    public interface ICard
    {
        void DrawCard(Player player, IEnumerable<Player> otherPlayers);

        event EventHandler DrewCard;
    }

    public class PayMoneyToAllPlayersCard : ICard
    {
        private readonly int _payAmountOtherPlayers;

        public PayMoneyToAllPlayersCard(int payAmountOtherPlayers)
        {
            _payAmountOtherPlayers = payAmountOtherPlayers;
        }

        public void DrawCard(Player player, IEnumerable<Player> otherPlayers)
        {
            foreach (var p in otherPlayers)
            {
                player.Money -= _payAmountOtherPlayers;
                p.Money += _payAmountOtherPlayers;
            }
            OnDrewCard();
        }

        public event EventHandler DrewCard;

        protected virtual void OnDrewCard()
        {
            DrewCard?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Cards
    {
        public List<ICard> Deck = new List<ICard>();
        private int _cardsDrawn;

        public Cards()
        {
            foreach (var card in Deck)
            {
                card.DrewCard += OnDrewCard;
            }
        }

        public ICard this[int index]
        {
            get => Deck[index];
            set => Deck[index] = value;
        }

        public void OnDrewCard(object sender, EventArgs e)
        {
            _cardsDrawn++;
            if (_cardsDrawn == 25) // set appropriate number when number of cards is known!
                _cardsDrawn = 0;
        }
    }
}