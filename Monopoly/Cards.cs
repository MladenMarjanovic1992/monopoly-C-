using System.Collections.Generic;
using Medallion;

namespace Monopoly
{
    public class Cards
    {
        public List<CardMoveToField> MoveToFieldCards { get; set; }
        public List<CardPayMoneyToAllPlayers> PayMoneyToAllPlayersCards { get; set; }
        public List<CardPayOnlyPlayer> PayOnlyPlayerCards { get; set; }
        public List<CardGetOutOfJail> GetOutOfJailCards { get; set; }
        public List<CardHousesRepair> HousesRepairCards { get; set; }
        public List<CardAdvanceToNearest> AdvanceToNearestCards { get; set; }
        public List<CardAdvanceXSpaces> AdvanceXSpacesCards { get; set; }
        public List<ICard> Deck = new List<ICard>();
        public int CardsDrawn { get; set; }

        public ICard this[int index]
        {
            get => Deck[index];
            set => Deck[index] = value;
        }

        public void DrawNext(Player currentPlayer, List<Player> otherPlayers)
        {
            Deck[CardsDrawn].DrawCard(currentPlayer, otherPlayers);

            CardsDrawn++;
            if (CardsDrawn == Deck.Count)
                CardsDrawn = 0;
        }

        public void PrepareDeck(Dice dice, List<IFieldBuildable> fields, int mapSize)
        {
            // movement cards require a Dice object to move player
            foreach (var card in MoveToFieldCards)
            {
                card.AddDiceAndMapSize(dice, mapSize);
            }
            foreach (var card in AdvanceToNearestCards)
            {
                card.AddDiceAndMapSize(dice, mapSize);
            }
            foreach (var card in AdvanceXSpacesCards)
            {
                card.AddDice(dice);
            }

            // house repair cards require FieldProperty list to query houses
            foreach (var card in HousesRepairCards)
            {
                card.AddFields(fields);
            }

            Deck.AddRange(MoveToFieldCards);
            Deck.AddRange(PayMoneyToAllPlayersCards);
            Deck.AddRange(PayOnlyPlayerCards);
            Deck.AddRange(GetOutOfJailCards);
            Deck.AddRange(HousesRepairCards);
            Deck.AddRange(AdvanceToNearestCards);
            Deck.AddRange(AdvanceXSpacesCards);

            Deck.Shuffle();
        }
    }
}