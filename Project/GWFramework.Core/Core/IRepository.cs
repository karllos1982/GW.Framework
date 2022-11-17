using GW.Common;

namespace GW.Core
{
    public interface IRepository<TParam,TModel,TList,TSearchResult>
    {
        IContext Context { get; set; }

        OperationStatus Create(TModel model);

        TModel Read(TParam model);

        OperationStatus Update(TModel model);

        OperationStatus Delete(TModel model);

        List<TList> List(TParam param);

        List<TSearchResult> Search(TParam param);


    }

}


