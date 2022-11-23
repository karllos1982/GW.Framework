using GW.Common;

namespace GW.Core
{
    public interface IRepository<TParam,TModel,TList,TSearchResult>
    {

        IContext Context { get; set; }

        Task Create(TModel model);

        Task<TModel> Read(TParam model);

        Task Update(TModel model);

        Task Delete(TModel model);

        Task<List<TList>> List(TParam param);

        Task<List<TSearchResult>> Search(TParam param);


    }

    public interface IRepositorySync<TParam, TModel, TList, TSearchResult>
    {

        IContext Context { get; set; }

        void Create(TModel model);

        TModel Read(TParam model);

        void Update(TModel model);

        void Delete(TModel model);

        List<TList> List(TParam param);

        List<TSearchResult> Search(TParam param);


    }

}


