using System.Collections.Generic;

namespace NikoSDK.Interfaces.Data
{
    public interface IActions : IBaseResponse
    {
        IReadOnlyList<IAction> Actions { get; }
    }
}
