using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Monopoly
{
    public class Choice
    {
        private Player _currentPlayer;
        private readonly List<Player> _players;
        private readonly Fields _fields;

        public Dictionary<string, Action> Actions { get; set; }

        public Choice(List<Player> players, Fields fields)
        {
            _players = players;
            _currentPlayer = players.First(p => p.CurrentPlayer);
            _fields = fields;
            Actions = new Dictionary<string, Action>();
        }

        public event EventHandler ChoseRoll;
        public event EventHandler ChoseEndTurn;
        public event EventHandler<ChoseTradeEventArgs> ChoseTrade;
        public event EventHandler<ChoseMortgageEventArgs> ChoseMortgage;
        public event EventHandler<ChosePayMortgageEventArgs> ChosePayMortgage;

        public void Roll()
        {
            OnChoseRoll();
        }

        public void EndTurn()
        {
            OnChoseEndTurn();
            _currentPlayer = _players.First(p => p.CurrentPlayer);
        }

        public void Trade()
        {
            var opponent = Prompt.ChoosePlayer(_players.Where(p => !p.CurrentPlayer).ToList(), "Trade with: ");
            var fieldFilter = new FieldRentableFilter();
            var opponentFields = fieldFilter.FilterFields(_fields.BuyableFields, new OwnerSpecification(opponent)).ToList();
            var playerFields = fieldFilter.FilterFields(_fields.BuyableFields, new OwnerSpecification(_currentPlayer)).ToList();
            var fieldToBuy = Prompt.ChooseFieldRentable(opponentFields, "Which field do you want to buy?");
            var offerMoney = Prompt.EnterAmount(_currentPlayer, "How much money are you offering?");
            var fieldToSell = Prompt.ChooseFieldRentable(playerFields, "Which field do you want to sell?");
            var askMoney = Prompt.EnterAmount(opponent, "How much money are you asking?");

            if(Prompt.YesOrNo("Confirm trade? (y/n)"))
                OnChoseTrade(opponent, _currentPlayer, fieldToBuy, fieldToSell, offerMoney, askMoney);
        }

        public void Mortgage()
        {
            var fieldFilter = new FieldRentableFilter();
            var playerFields = fieldFilter.FilterFields(_fields.BuyableFields, new MortgageSpecification(_currentPlayer, true)).ToList();
            var fieldToMortgage = Prompt.ChooseFieldRentable(playerFields, "Which field do you want to mortgage?");

            if(fieldToMortgage != null)
                OnChoseMortgage(fieldToMortgage, _currentPlayer);
        }

        public void PayMortgage()
        {
            var fieldFilter = new FieldRentableFilter();
            var playerFields = fieldFilter.FilterFields(_fields.BuyableFields, new MortgageSpecification(_currentPlayer, false)).ToList();
            var fieldToPayMortgage = Prompt.ChooseFieldRentable(playerFields, "Which field do you want to pay mortgage for?");

            if (fieldToPayMortgage != null)
                OnChosePayMortgage(fieldToPayMortgage, _currentPlayer);
        }

        public void AddAction(string actionName, Action action, bool condition)
        {
            if (condition)
                Actions.Add(actionName, action);
        }

        protected virtual void OnChoseRoll()
        {
            ChoseRoll?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnChoseEndTurn()
        {
            ChoseEndTurn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnChoseTrade(Player opponent, Player currentPlayer, IFieldRentable buyField, IFieldRentable sellField, int offerMoney, int askMoney)
        {
            ChoseTrade?.Invoke(this, new ChoseTradeEventArgs()
            {
                Opponent = opponent,
                CurrentPlayer = currentPlayer,
                BuyField = buyField,
                SellField = sellField,
                OfferMoney = offerMoney,
                AskMoney = askMoney
            });
        }

        protected virtual void OnChoseMortgage(IFieldRentable field, Player player)
        {
            ChoseMortgage?.Invoke(this, new ChoseMortgageEventArgs(){Field = field, Player = player});
        }

        protected virtual void OnChosePayMortgage(IFieldRentable field, Player player)
        {
            ChosePayMortgage?.Invoke(this, new ChosePayMortgageEventArgs(){Field = field, Player = player});
        }
    }
}