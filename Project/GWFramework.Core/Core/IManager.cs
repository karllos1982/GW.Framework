using GW.Common;

namespace GW.Core
{
    public interface IManager
    {
        IContext Context { get; set; }

        void InitializeDomains(IContext context);

    }

}