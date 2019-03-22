using System.Collections.Generic;

namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface IActions : IBaseResponse
    {
        IReadOnlyList<IAction> Actions { get; }
    }
}
