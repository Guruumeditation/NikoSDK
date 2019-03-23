namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface IAction
    {
        int Id { get; }
        string Name { get; }

        int Type { get; }

        int Location { get; }

        int Value { get; }
    }
}
