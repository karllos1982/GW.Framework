using GW.Common;

namespace GW.Core
{
    public interface IContext
    {
        ISettings Settings { get; set; }

        OperationStatus ConnStatus { get; set; }

        OperationStatus ExecutionStatus { get; set; }

        OperationStatus Begin();

        OperationStatus End();

        void RegisterDataLog(string userid, OPERATIONLOGENUM operation,
          string tableaname, string objID, object olddata, object currentdata);

        Task RegisterDataLogAsync(string userid, OPERATIONLOGENUM operation,
             string tableaname, string objID, object olddata, object currentdata);

        void Dispose(); 

    }

}