using GW.Common;

namespace GW.Core
{
    public interface IContext
    {
        ISettings Settings { get; set; }

        OperationStatus ConnStatus { get; set; }

        OperationStatus ExecutionStatus { get; set; }

        OperationStatus Begin(int sourceindex);

        OperationStatus Commit();

        OperationStatus End();

        void Dispose(); 

    }

}