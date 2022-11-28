using GW.Common;

namespace GW.Core
{
    public interface IDomain<TParam, TEntry, TResult, TList>
    {
        IContext Context { get; set; }

        Task<TResult> Get(TParam param);

        Task<TResult> FillChields(TResult obj);

        Task<List<TList>> List(TParam param);

        Task<List<TResult>> Search(TParam param);

        Task<TEntry> Set(TEntry model, object userid);

        Task<TEntry> Delete(TEntry model, object userid);

        Task EntryValidation(TEntry obj);

        Task InsertValidation(TEntry obj);

        Task UpdateValidation(TEntry obj);

        Task DeleteValidation(TEntry obj);

    }

    public interface IDomainSync<TParam, TEntry, TResult, TList>
    {
        IContext Context { get; set; }

        TResult Get(TParam param);

        TResult FillChields(TResult obj);

        List<TList> List(TParam param);

        List<TResult> Search(TParam param);

        TEntry Set(TEntry model, object userid);

        TEntry Delete(TEntry model, object userid);

        void EntryValidation(TEntry obj);

        void InsertValidation(TEntry obj);

        void UpdateValidation(TEntry obj);

        void DeleteValidation(TEntry obj);

    }

}