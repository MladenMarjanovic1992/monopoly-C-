using System;

namespace Monopoly
{
    public class Mortgage
    {
        private void PutUnderMortgage(Player player, IFieldRentable field)
        {
            player.Money += field.MortgageValue;
            field.UnderMortgage = true;
            field.CanMortgage = false;

            OnFieldMortgaged(this, field);
        }

        private void PayOffMortgage(Player player, IFieldRentable field)
        {
            if (player.Money >= field.MortgageValue + field.MortgageValue / 10)
            {
                player.Money -= field.MortgageValue + field.MortgageValue / 10;
                field.UnderMortgage = false;
                field.CanMortgage = true;

                OnMortgagePayed(this, field);
            }
            else
            {
                Console.WriteLine("Not enough money");
            }
            
        }

        public void OnChoseMortgage(object sender, ChoseMortgageEventArgs e)
        {
            PutUnderMortgage(e.Player, e.Field);
        }

        public void OnChosePayMortgage(object sender, ChoseMortgageEventArgs e)
        {
            PayOffMortgage(e.Player, e.Field);
        }

        public void OnPlayerLiquidated(object sender, PlayerLiquidatedEventArgs e)
        {
            foreach (var field in e.AllPlayerFields)
            {
                PutUnderMortgage(e.PlayerLiquidated, field);
            }
        }

        public event EventHandler<FieldMortgagedEventArgs> FieldMortgaged;
        public event EventHandler<MortgagePayedEventArgs> MortgagePayed;

        protected virtual void OnFieldMortgaged(object source, IFieldRentable field)
        {
            FieldMortgaged?.Invoke(this, new FieldMortgagedEventArgs(){ Field = field });
        }

        protected virtual void OnMortgagePayed(object source, IFieldRentable field)
        {
            MortgagePayed?.Invoke(this, new MortgagePayedEventArgs(){ Field = field });
        }
    }
}