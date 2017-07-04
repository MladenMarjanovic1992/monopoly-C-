using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class PlayerLiquidatedEventArgs : EventArgs
    {
        public Player PlayerLiquidated { get; set; }
        public IEnumerable<IFieldBuildable> PropertyFieldsWithHouses { get; set; }
        public IEnumerable<IFieldRentable> AllPlayerFields { get; set; }
        public List<Player> StakeHolders { get; set; }
        public bool BankruptDuringOwnTurn { get; set; }
    }
}