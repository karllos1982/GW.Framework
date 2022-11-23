using GW.Common;

namespace GW.Core
{
    public interface IDomain<TParam,TModel,TList,TSearchResult>
    {
        IContext Context { get; set; }

        Task<TModel> Get(TParam param);

        Task<TModel> FillChields(TModel obj);

        Task<List<TList>> List(TParam param);

        Task<List<TSearchResult>> Search(TParam param);

        Task<TModel> Set(TModel model, object userid);

        Task<TModel> Delete(TModel model, object userid);

        Task EntryValidation(TModel obj);

        Task InsertValidation(TModel obj);

        Task UpdateValidation(TModel obj);

        Task DeleteValidation(TModel obj);

    }

    public interface IDomainSync<TParam, TModel, TList, TSearchResult>
    {
        IContext Context { get; set; }

        TModel Get(TParam param);

        TModel FillChields(TModel obj);

        List<TList> List(TParam param);

        List<TSearchResult> Search(TParam param);

        TModel Set(TModel model, object userid);

        TModel Delete(TModel model, object userid);

        void EntryValidation(TModel obj);

        void InsertValidation(TModel obj);

        void UpdateValidation(TModel obj);

        void DeleteValidation(TModel obj);

    }

}