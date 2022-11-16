using GW.Common;

namespace GW.Core
{
    public interface IRepository<TParam,TModel,TList,TSearchResult>
    {
        IContext Context { get; set; }

        object Create(TModel model);

        object Read(TModel model);

        object Update(TModel model);

        object Delete(TModel model);

        List<TList> List(TParam param);

        List<TSearchResult> Search(TParam param);


    }

}


