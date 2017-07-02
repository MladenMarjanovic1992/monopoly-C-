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
        public event EventHandler ChoseQuitGame;
        public event EventHandler<ChoseTradeEventArgs> ChoseTrade;
        public event EventHandler<ChoseMortgageEventArgs> ChoseMortgage;
        public event EventHandler<ChoseMortgageEventArgs> ChosePayMortgage;
        public event EventHandler<ChoseBuildEventArgs> ChoseBuildHouse;
        public event EventHandler<ChoseBuildEventArgs> ChoseSellHouse;

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
            var fieldToBuy = Prompt.ChooseField(opponentFields, "Which field do you want to buy?");
            var offerMoney = Prompt.EnterAmount(_currentPlayer, "How much money are you offering?");
            var fieldToSell = Prompt.ChooseField(playerFields, "Which field do you want to sell?");
            var askMoney = Prompt.EnterAmount(opponent, "How much money are you asking?");

            if(Prompt.YesOrNo("Confirm trade? (y/n)"))
                OnChoseTrade(opponent, _currentPlayer, fieldToBuy, fieldToSell, offerMoney, askMoney);
        }

        public void Mortgage()
        {
            var playerFields = new FieldRentableFilter().FilterFields(_fields.BuyableFields, new MortgageSpecification(_currentPlayer, true)).ToList();
            var fieldToMortgage = Prompt.ChooseField(playerFields, "Which field do you want to mortgage?");

            if(fieldToMortgage != null)
                OnChoseMortgage(fieldToMortgage, _currentPlayer);
        }

        public void PayMortgage()
        {
            var playerFields = new FieldRentableFilter().FilterFields(_fields.BuyableFields, new MortgageSpecification(_currentPlayer, false)).ToList();
            var fieldToPayMortgage = Prompt.ChooseField(playerFields, "Which field do you want to pay mortgage for?");

            if (fieldToPayMortgage != null)
                OnChosePayMortgage(fieldToPayMortgage, _currentPlayer);
        }

        public void BuildHouse()
        {
            var buildableFields = _fields.PropertyFields.Where(f => f.Owner == _currentPlayer && f.CanBuild).ToList();
            var fieldToBuild = Prompt.ChooseField(buildableFields, "Which field do you want to build on?");

            if(fieldToBuild != null)
                OnChoseBuildHouse(fieldToBuild, _currentPlayer);
        }

        public void SellHouse()
        {
            var unbuildableFields = _fields.PropertyFields.Where(f => f.Owner == _currentPlayer && f.CanRemoveHouse).ToList();
            var fieldToUnbuild = Prompt.ChooseField(unbuildableFields, "Where do you want to sell a house?");

            if(fieldToUnbuild != null)
                OnChoseSellHouse(fieldToUnbuild, _currentPlayer);
        }

        public void CheckFieldStats()
        {
            Prompt.ChooseField(_fields.BuyableFields, "Which field do you want to see?").PrintFieldStats();
        }

        public void ShowPlayerStats()
        {
            var player = Prompt.ChoosePlayer(_players, "Whose stats do you want to see?");
            player.PrintStats();

            var fieldsOwned = new FieldRentableFilter().FilterFields(_fields.BuyableFields, new OwnerSpecification(player));
            Console.Write("Owns: ");
            foreach (var field in fieldsOwned)
            {
                Console.Write($"{field.FieldName}; ");
            }
            Console.WriteLine();
        }

        public void UseGetOutOfJailCard()
        {
            _currentPlayer.InJail = false;
            _currentPlayer.RollsUntilOut = 0;
            _currentPlayer.GetOutOfJailCards--;
        }

        public void QuitGame()
        {
            if (Prompt.YesOrNo("Do you really want to quit?"))
                OnChoseQuitGame();
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
            ChosePayMortgage?.Invoke(this, new ChoseMortgageEventArgs(){Field = field, Player = player});
        }

        protected virtual void OnChoseBuildHouse(FieldProperty field, Player player)
        {
            ChoseBuildHouse?.Invoke(this, new ChoseBuildEventArgs(){Player = player, PropertyField = field});
        }

        protected virtual void OnChoseSellHouse(FieldProperty field, Player player)
        {
            ChoseSellHouse?.Invoke(this, new ChoseBuildEventArgs() { Player = player, PropertyField = field });
        }

        protected virtual void OnChoseQuitGame()
        {
            ChoseQuitGame?.Invoke(this, EventArgs.Empty);
        }
    }
}