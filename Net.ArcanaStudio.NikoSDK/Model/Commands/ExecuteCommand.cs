namespace Net.ArcanaStudio.NikoSDK.Model.Commands
{
    public class ExecuteCommand : NikoCommandBase
    {
        public ExecuteCommand(int id, int value) : base(Constants.CommandNames.ExecuteActions, id, value)
        {
        }
    }
}
