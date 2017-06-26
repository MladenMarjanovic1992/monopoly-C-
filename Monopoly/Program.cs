using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            var player1 = new Player("Djomla", 5000);
            var player2 = new Player("Shoux", 5000);
            var player3 = new Player("Ile", 5000);

            var players = new List<Player>() { player1, player2, player3 };
            var fields = JsonConvert.DeserializeObject<Fields>(File.ReadAllText(@"d:\Fields.json"));
            var utility = new Utility(fields.UtilityFields);
            var station = new Station(fields.StationFields);
            var colors = new List<Color>()
            {
                new Color("Brown", new List<PropertyField>{fields.PropertyFields[0], fields.PropertyFields[1]}),
                new Color("Light Blue", new List<PropertyField>{fields.PropertyFields[2], fields.PropertyFields[3], fields.PropertyFields[4]}),
                new Color("Pink", new List<PropertyField>{fields.PropertyFields[5], fields.PropertyFields[6], fields.PropertyFields[7]}),
                new Color("Orange", new List<PropertyField>{fields.PropertyFields[8], fields.PropertyFields[9], fields.PropertyFields[10]}),
                new Color("Red", new List<PropertyField>{fields.PropertyFields[11], fields.PropertyFields[12], fields.PropertyFields[13]}),
                new Color("Yellow", new List<PropertyField>{fields.PropertyFields[14], fields.PropertyFields[15], fields.PropertyFields[16]}),
                new Color("Green", new List<PropertyField>{fields.PropertyFields[17], fields.PropertyFields[18], fields.PropertyFields[19]}),
                new Color("Dark Blue", new List<PropertyField>{fields.PropertyFields[20], fields.PropertyFields[21]})
            };
            var game = new Game(players);
            var dice = new Dice();
            var trade = new Trade();
            var house = new House();
            var mortgage = new Mortgage();
            var choice = new Choice(game.Players, fields);

            dice.DiceRolled += game.OnDiceRolled;
            choice.ChoseRoll += dice.OnChoseRoll;
            choice.ChoseEndTurn += game.OnChoseEndTurn;
            choice.ChoseTrade += trade.OnChoseTrade;
            choice.ChoseMortgage += mortgage.OnChoseMortgage;
            choice.ChosePayMortgage += mortgage.OnChosePayMortgage;

            foreach (var field in fields.BuyableFields)
            {
                game.PlayerMoved += field.OnPlayerMoved;
            }

            foreach (var chance in fields.ChanceFields)
            {
                game.PlayerMoved += chance.OnPlayerMoved;
            }

            foreach (var communityChest in fields.CommunityChestFields)
            {
                game.PlayerMoved += communityChest.OnPlayerMoved;
            }

            foreach (var taxField in fields.TaxFields)
            {
                game.PlayerMoved += taxField.OnPlayerMoved;
            }

            foreach (var goToJailField in fields.GoToJailFields)
            {
                game.PlayerMoved += goToJailField.OnPlayerMoved;
            }

            foreach (var otherField in fields.OtherFields)
            {
                game.PlayerMoved += otherField.OnPlayerMoved;
            }

            foreach (var color in colors)
            {
                trade.FieldBought += color.OnFieldBought;
                trade.FieldSold += color.OnFieldSold;
                house.HouseBuilt += color.OnHouseBuilt;
                house.HouseSold += color.OnHouseSold;
                mortgage.FieldMortgaged += color.OnFieldMortgaged;
                mortgage.MortgagePayed += color.OnMortgagePayed;
            }

            trade.FieldBought += utility.OnFieldBought;
            trade.FieldBought += station.OnFieldBought;
            trade.FieldSold += utility.OnFieldSold;
            trade.FieldSold += station.OnFieldSold;
            mortgage.FieldMortgaged += utility.OnFieldMortgaged;
            mortgage.FieldMortgaged += station.OnFieldMortgaged;
            mortgage.MortgagePayed += utility.OnMortgagePayed;
            mortgage.MortgagePayed += station.OnMortgagePayed;


            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[2], fields.PropertyFields[2].Price);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[3], fields.PropertyFields[3].Price);
            //trade.BuyField(game.CurrentPlayer, fields.PropertyFields[4], fields.PropertyFields[4].Price);

            //trade.BuyField(game.Players[1], fields.PropertyFields[0], fields.PropertyFields[0].Price);
            //trade.BuyField(game.Players[1], fields.PropertyFields[1], fields.PropertyFields[1].Price);

            //trade.BuyField(game.CurrentPlayer, fields.StationFields[0], fields.StationFields[0].Price);
            //trade.BuyField(game.CurrentPlayer, fields.StationFields[1], fields.StationFields[1].Price);
            //trade.BuyField(game.CurrentPlayer, fields.StationFields[2], fields.StationFields[2].Price);
            //trade.BuyField(game.CurrentPlayer, fields.StationFields[3], fields.StationFields[3].Price);

            //house.BuildHouse(player1, fields.PropertyFields[2]);
            //house.BuildHouse(player1, fields.PropertyFields[3]);
            //house.BuildHouse(player1, fields.PropertyFields[4]);
            //house.BuildHouse(player1, fields.PropertyFields[2]);
            //house.BuildHouse(player1, fields.PropertyFields[3]);
            //house.BuildHouse(player1, fields.PropertyFields[4]);
            //Console.WriteLine();
            //house.SellHouse(player1, fields.PropertyFields[4]);

            //Console.WriteLine();

            //mortgage.PutUnderMortgage(player1, fields.PropertyFields[0]);
            //Console.WriteLine();
            //mortgage.PayOffMortgage(player1, fields.PropertyFields[0]);
            //Console.WriteLine();
            //house.BuildHouse(player1, fields.PropertyFields[0]);
            //Console.WriteLine();
            //house.BuildHouse(player1, fields.PropertyFields[1]);
            //house.BuildHouse(player1, fields.PropertyFields[1]);
            //Console.WriteLine();

            while (true) // change field effect functionality, add field private field to player!
            {
                game.CurrentPlayer.PrintStats();

                choice.AddAction("Roll dice", choice.Roll, !game.AlreadyRolled);
                choice.AddAction("End turn", choice.EndTurn, game.AlreadyRolled);
                choice.AddAction("Trade", choice.Trade, true);
                choice.AddAction("Mortgage field", choice.Mortgage, true);
                choice.AddAction("Pay Mortgage", choice.PayMortgage, true);

                var command = Prompt.ChooseOption(choice.Actions, "Type first letter of command: ");

                choice.Actions[command]();
                choice.Actions.Clear();
            }
        }
    }

    public class Bankrupcy
    {
        private readonly List<IFieldRentable> _fieldsRentable;
        private readonly List<PropertyField> _propertyFields;

        public Bankrupcy(Fields fields)
        {
            _fieldsRentable = fields.BuyableFields;
            _propertyFields = fields.PropertyFields;
        }

        public bool IsBankrupt(Player player)
        {
            return HousesLiquidationValue(player) + MortgageValue(player) < 0;
        }

        private int HousesLiquidationValue(Player player)
        {
            var propertiesWithHouses = _propertyFields.Where(p => p.Owner == player && p.Houses > 0);

            return propertiesWithHouses.Sum(field => field.HousePrice / 2);
        }

        private int MortgageValue(Player player)
        {
            var fieldsForMortgage = _fieldsRentable.Where(f => f.Owner == player && !f.UnderMortgage);

            return fieldsForMortgage.Sum(field => field.MortgageValue);
        }
    }
}
