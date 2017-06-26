using System.Collections.Generic;

namespace Monopoly
{
    public interface IField
    {
        string FieldName { get; set; }
        int FieldIndex { get; set; }

        void FieldEffect(Player currentPlayer, List<Player> otherPlayers);
        void PrintFieldStats();
    }
}