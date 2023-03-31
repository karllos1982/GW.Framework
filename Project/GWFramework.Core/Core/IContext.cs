using GW.Common;

namespace GW.Core
{
    public interface IContext
    {
        ISettings Settings { get; set; }

        string LocalizationLanguage { get; set; }

        OperationStatus ConnStatus { get; set; }

        OperationStatus ExecutionStatus { get; set; }

        OperationStatus Begin(int connindex, object isolationlavel);

        OperationStatus End();

        void RegisterDataLog(string userid, OPERATIONLOGENUM operation,
          string tableaname, string objID, object olddata, object currentdata);

        Task RegisterDataLogAsync(string userid, OPERATIONLOGENUM operation,
             string tableaname, string objID, object olddata, object currentdata);

        Task<List<LocalizationTextItem>> GetLocalizationTextsAsync();

        Task<bool> CheckUniqueValueForInsert(string tablename, string fieldname, string fieldvalue);

        Task<bool> CheckUniqueValueForUpdate(string tablename, string fieldname,
                   string fieldvalue, string pkfieldname, string pkvalue); 

        void Dispose(); 

    }

}