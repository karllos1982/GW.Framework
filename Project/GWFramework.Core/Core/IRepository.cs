using GW.Common;

namespace GW.Core
{
    public interface IRepository<TParam,TEntry,TResult, TList>
    {
        string TableName { get; set; }

        string PKFieldName { get; set; }

        IContext Context { get; set; }

        Task Create(TEntry model);

        Task<TResult> Read(TParam model);

        Task Update(TEntry model);

        Task Delete(TEntry model);

        Task<List<TList>> List(TParam param);

        Task<List<TResult>> Search(TParam param);


    }

    public interface IRepositorySync<TParam, TEntry, TResult, TList>
    {

        IContext Context { get; set; }

        void Create(TEntry model);

        TResult Read(TParam model);

        void Update(TEntry model);

        void Delete(TEntry model);

        List<TList> List(TParam param);

        List<TResult> Search(TParam param);

        
    }

}


