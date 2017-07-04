using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Monopoly
{
    class Program
    {
        static void Main(string[] args)
        {
            // load fields
            var fields = JsonConvert.DeserializeObject<Fields>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Fields.json")));

            // load dice
            var dice = new Dice();

            // welcome message
            Console.WriteLine("Welcome to Monopoly!");
            Console.WriteLine();

            // load players
            var numberOfPlayers = Prompt.EnterNumberOfPlayers();
            var players = new List<Player>();

            for (var i = 1; i <= numberOfPlayers; i++)
            {
                players.Add(new Player(Prompt.EnterPlayerName(i), 5000, fields.OtherFields[0]));
            }

            // initialize classes for buyable and buildable fields
            var utility = new Utility(fields.UtilityFields);
            var station = new Station(fields.StationFields);
            var colors = new List<Color>()
            {
                new Color("Brown", new List<FieldProperty>{fields.PropertyFields[0], fields.PropertyFields[1]}),
                new Color("Light Blue", new List<FieldProperty>{fields.PropertyFields[2], fields.PropertyFields[3], fields.PropertyFields[4]}),
                new Color("Pink", new List<FieldProperty>{fields.PropertyFields[5], fields.PropertyFields[6], fields.PropertyFields[7]}),
                new Color("Orange", new List<FieldProperty>{fields.PropertyFields[8], fields.PropertyFields[9], fields.PropertyFields[10]}),
                new Color("Red", new List<FieldProperty>{fields.PropertyFields[11], fields.PropertyFields[12], fields.PropertyFields[13]}),
                new Color("Yellow", new List<FieldProperty>{fields.PropertyFields[14], fields.PropertyFields[15], fields.PropertyFields[16]}),
                new Color("Green", new List<FieldProperty>{fields.PropertyFields[17], fields.PropertyFields[18], fields.PropertyFields[19]}),
                new Color("Dark Blue", new List<FieldProperty>{fields.PropertyFields[20], fields.PropertyFields[21]})
            };

            // initialize map
            var map = new List<IField>();
            map.AddRange(fields.PropertyFields);
            map.AddRange(fields.StationFields);
            map.AddRange(fields.UtilityFields);
            map.AddRange(fields.ChanceFields);
            map.AddRange(fields.CommunityChestFields);
            map.AddRange(fields.OtherFields);
            map.AddRange(fields.GoToJailFields);
            map.AddRange(fields.TaxFields);
            map.Sort((x, y) => x.FieldIndex.CompareTo(y.FieldIndex));

            // load cards
            var chanceCards = JsonConvert.DeserializeObject<Cards>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "ChanceCards.json")));
            var communityChestCards = JsonConvert.DeserializeObject<Cards>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "CommunityChestCards.json")));

            chanceCards.PrepareDeck(dice, fields.BuildableFields, map.Count);
            communityChestCards.PrepareDeck(dice, fields.BuildableFields, map.Count);

            foreach (var field in fields.ChanceFields)
            {
                field.Cards = chanceCards;
            }
            foreach (var field in fields.CommunityChestFields)
            {
                field.Cards = communityChestCards;
            }

            // initialize game
            var bankrupcy = new Bankrupcy(fields);
            var game = new Game(players, map, bankrupcy);
            
            var trade = new Trade();
            var house = new House();
            var mortgage = new Mortgage();
            var choice = new Choice(game.Players, fields);


            // add events

            // color events
            foreach (var color in colors)
            {
                trade.FieldBought += color.OnFieldBought;
                trade.FieldSold += color.OnFieldSold;
                house.HouseBuilt += color.OnHouseBuilt;
                house.HouseSold += color.OnHouseSold;
                mortgage.FieldMortgaged += color.OnFieldMortgaged;
                mortgage.MortgagePayed += color.OnMortgagePayed;
            }

            // card events
            foreach (var card in chanceCards.PayMoneyToAllPlayersCards)
            {
                card.PayedForCard += game.OnPayedForCard;
            }
            foreach (var card in communityChestCards.PayMoneyToAllPlayersCards)
            {
                card.PayedForCard += game.OnPayedForCard;
            }

            // dice events
            dice.DiceRolled += game.OnDiceRolled;

            // choice events
            choice.ChoseRoll += dice.OnChoseRoll;
            choice.ChoseEndTurn += game.OnChoseEndTurn;
            choice.ChoseQuitGame += game.OnChoseQuitGame;
            choice.ChoseTrade += trade.OnChoseTrade;
            choice.ChoseMortgage += mortgage.OnChoseMortgage;
            choice.ChosePayMortgage += mortgage.OnChosePayMortgage;
            choice.ChoseBuildHouse += house.OnChoseBuildHouse;
            choice.ChoseSellHouse += house.OnChoseSellHouse;

            // trade events
            trade.FieldBought += utility.OnFieldBought;
            trade.FieldBought += station.OnFieldBought;
            trade.FieldSold += utility.OnFieldSold;
            trade.FieldSold += station.OnFieldSold;

            // mortgage events
            mortgage.FieldMortgaged += utility.OnFieldMortgaged;
            mortgage.FieldMortgaged += station.OnFieldMortgaged;
            mortgage.MortgagePayed += utility.OnMortgagePayed;
            mortgage.MortgagePayed += station.OnMortgagePayed;

            // bankrupcy events
            bankrupcy.PlayerLiquidated += house.OnPlayerLiquidated;
            bankrupcy.PlayerLiquidated += mortgage.OnPlayerLiquidated;
            bankrupcy.PlayerLiquidated += trade.OnPlayerLiquidated;
            bankrupcy.PlayerLiquidated += game.OnPlayerLiquidated;

            // go to jail event
            fields.GoToJailFields[0].WentToJail += game.OnWentToJail;

            while (!game.GameOver)
            {
                game.CurrentPlayer.PrintStats();

                choice.AddAction("Use 'Get out of jail' card", choice.UseGetOutOfJailCard, game.CurrentPlayer.InJail && !game.AlreadyRolled && game.CurrentPlayer.GetOutOfJailCards > 0);
                choice.AddAction("Roll dice", choice.Roll, !game.AlreadyRolled);
                choice.AddAction("End turn", choice.EndTurn, game.AlreadyRolled);
                choice.AddAction("Trade", choice.Trade, fields.BuyableFields.Any(f => f.Owner == game.CurrentPlayer && f.CanTrade));
                choice.AddAction("Mortgage field", choice.Mortgage, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.CanMortgage));
                choice.AddAction("Pay Mortgage", choice.PayMortgage, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.UnderMortgage));
                choice.AddAction("Build house", choice.BuildHouse, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.CanBuild));
                choice.AddAction("Sell house", choice.SellHouse, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.CanRemoveHouse));
                choice.AddAction("Check field stats", choice.CheckFieldStats, true);
                choice.AddAction("Show player stats", choice.ShowPlayerStats, true);
                choice.AddAction("Quit game", choice.QuitGame, true);

                var command = Prompt.ChooseOption(choice.Actions, "Type first letter of command: ");

                choice.Actions[command]();
                choice.Actions.Clear();
            }
        }
    }
}
