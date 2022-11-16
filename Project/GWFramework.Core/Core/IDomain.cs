using GW.Common;

namespace GW.Core
{
    public interface IDomain<TParam,TModel,TList,TSearchResult>
    {
        IContext Context { get; set; }

        object Get(TModel model);

        void FillChields(ref TModel obj);  

        List<TList> List(TParam param);

        List<TSearchResult> Search(TParam param);

        object Set(TModel model, object userid);

        object Delete(TModel model, object userid);

        OperationStatus EntryValidation(TModel obj);

        OperationStatus InsertValidation(TModel obj);

        OperationStatus UpdateValidation(TModel obj);

        OperationStatus DeleteValidation(TModel obj);

    }

}