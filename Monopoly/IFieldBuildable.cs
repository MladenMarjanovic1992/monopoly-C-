namespace Monopoly
{
    public interface IFieldBuildable : IFieldRentable
    {
        int Houses { get; set; }
        int HousePrice { get; set; }
        bool CanBuild { get; set; }
        bool CanRemoveHouse { get; set; }
    }
}