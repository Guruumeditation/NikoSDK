using System.Collections.Generic;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    public class ActionsImp : IActions
    {
        public int Error { get;  }
        public bool IsError => Error != 0;
        public IReadOnlyList<IAction> Actions { get; }

        public ActionsImp(IReadOnlyList<IAction> actions)
        {
            Actions = actions;
        }

        public ActionsImp(int error)
        {
            Error = error;
        }
    }
}
