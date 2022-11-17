
namespace GW.Common
{
    // OBSOLITE INTERFACE
    public interface IDataWorker
    {        

        List<T> ExecuteQueryToList<T>(ref OperationStatus executionstatus,
            string sql, object filter = null);

        T ExecuteQueryFirst<T>(ref OperationStatus executionstatus,
            string sql, object filter = null);

        OperationStatus Execute(ref OperationStatus executionstatus,
            string sql, object data);


    }
}
