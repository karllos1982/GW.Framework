
namespace GW.Common
{
    public interface IDataWorker
    {        

        List<T> ExecuteQueryToList<T>(string sql, object filter = null);

        T ExecuteQueryFirst<T>(string sql, object filter = null);

        OperationStatus Execute(string sql, object obj);


    }
}
