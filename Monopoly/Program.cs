using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            var fields = JsonConvert.DeserializeObject<Fields>(File.ReadAllText(@"d:\Fields.json"));

            var player1 = new Player("Djomla", 5000, fields.OtherFields[0]);
            var player2 = new Player("Shoux", 5000, fields.OtherFields[0]);
            var player3 = new Player("Ile", 5000, fields.OtherFields[0]);

            var players = new List<Player>() { player1, player2, player3 };
            
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

            var bankrupcy = new Bankrupcy(fields);
            var game = new Game(players, map, bankrupcy);
            var dice = new Dice();
            var trade = new Trade();
            var house = new House();
            var mortgage = new Mortgage();
            var choice = new Choice(game.Players, fields);
            
            foreach (var color in colors)
            {
                trade.FieldBought += color.OnFieldBought;
                trade.FieldSold += color.OnFieldSold;
                house.HouseBuilt += color.OnHouseBuilt;
                house.HouseSold += color.OnHouseSold;
                mortgage.FieldMortgaged += color.OnFieldMortgaged;
                mortgage.MortgagePayed += color.OnMortgagePayed;
            }

            dice.DiceRolled += game.OnDiceRolled;

            choice.ChoseRoll += dice.OnChoseRoll;
            choice.ChoseEndTurn += game.OnChoseEndTurn;
            choice.ChoseQuitGame += game.OnChoseQuitGame;
            choice.ChoseTrade += trade.OnChoseTrade;
            choice.ChoseMortgage += mortgage.OnChoseMortgage;
            choice.ChosePayMortgage += mortgage.OnChosePayMortgage;
            choice.ChoseBuildHouse += house.OnChoseBuildHouse;
            choice.ChoseSellHouse += house.OnChoseSellHouse;

            trade.FieldBought += utility.OnFieldBought;
            trade.FieldBought += station.OnFieldBought;
            trade.FieldSold += utility.OnFieldSold;
            trade.FieldSold += station.OnFieldSold;

            mortgage.FieldMortgaged += utility.OnFieldMortgaged;
            mortgage.FieldMortgaged += station.OnFieldMortgaged;
            mortgage.MortgagePayed += utility.OnMortgagePayed;
            mortgage.MortgagePayed += station.OnMortgagePayed;

            bankrupcy.PlayerLiquidated += house.OnPlayerLiquidated;
            bankrupcy.PlayerLiquidated += mortgage.OnPlayerLiquidated;
            bankrupcy.PlayerLiquidated += trade.OnPlayerLiquidated;
            bankrupcy.PlayerLiquidated += game.OnPlayerLiquidated;

            fields.GoToJailFields[0].WentToJail += game.OnWentToJail;


            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[2], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[3], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[4], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[5], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[6], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[7], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[8], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[9], 0);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[10], 0);
            //trade.BuyField(game.CurrentPlayer, fields.StationFields[0], 0);

            //foreach (var field in fields.PropertyFields.Where(f => f.Owner == player1))
            //{
            //    field.Houses = 1;
            //    field.CurrentRent = field.Rent[4];
            //}

            //trade.BuyField(game.Players[2], fields.PropertyFields[0], 0);
            //trade.BuyField(game.Players[2], fields.PropertyFields[1], 0);

            //while (!game.GameOver) // CHANGE ROLL BACK TO NORMAL IN DICE CLASS!!!
            //{
            //    game.CurrentPlayer.PrintStats();

            //    choice.AddAction("Use 'Get out of jail' card", choice.UseGetOutOfJailCard, game.CurrentPlayer.InJail && !game.AlreadyRolled && game.CurrentPlayer.GetOutOfJailCards > 0);
            //    choice.AddAction("Roll dice", choice.Roll, !game.AlreadyRolled);
            //    choice.AddAction("End turn", choice.EndTurn, game.AlreadyRolled);
            //    choice.AddAction("Trade", choice.Trade, fields.BuyableFields.Any(f => f.Owner == game.CurrentPlayer && f.CanTrade));
            //    choice.AddAction("Mortgage field", choice.Mortgage, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.CanMortgage));
            //    choice.AddAction("Pay Mortgage", choice.PayMortgage, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.UnderMortgage));
            //    choice.AddAction("Build house", choice.BuildHouse, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.CanBuild));
            //    choice.AddAction("Sell house", choice.SellHouse, fields.PropertyFields.Any(f => f.Owner == game.CurrentPlayer && f.CanRemoveHouse));
            //    choice.AddAction("Check field stats", choice.CheckFieldStats, true);
            //    choice.AddAction("Show player stats", choice.ShowPlayerStats, true);
            //    choice.AddAction("Quit game", choice.QuitGame, true);

            //    var command = Prompt.ChooseOption(choice.Actions, "Type first letter of command: ");

            //    choice.Actions[command]();
            //    choice.Actions.Clear();
            //}

        }
    }
}
