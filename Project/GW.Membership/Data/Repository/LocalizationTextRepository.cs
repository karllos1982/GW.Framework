using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class LocalizationTextRepository : ILocalizationTextRepository
    {

        public LocalizationTextRepository(IContext context)
        {
            Context = context;
        }

        private LocalizationTextQueryBuilder query = new LocalizationTextQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(LocalizationTextEntry model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysLocalizationText", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
        }

        public async Task<LocalizationTextResult> Read(LocalizationTextParam param)
        {
            LocalizationTextResult ret = null;

            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<LocalizationTextResult>(sql, param);

            return ret;
        }

        public async Task Update(LocalizationTextEntry model)
        {

            string sql = query.QueryForUpdate("sysLocalizationText", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task Delete(LocalizationTextEntry model)
        {

            string sql = query.QueryForDelete("sysLocalizationText", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task<List<LocalizationTextList>> List(LocalizationTextParam param)
        {
            List<LocalizationTextList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<LocalizationTextList>(query.QueryForList(null), param);

            return ret;
        }

        public async Task<List<LocalizationTextResult>> Search(LocalizationTextParam param)
        {
            List<LocalizationTextResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<LocalizationTextResult>(query.QueryForSearch(null), param);


            return ret;
        }

        public async Task<List<LocalizationTextList>> GetListOfLanguages()
        {
            List<LocalizationTextList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<LocalizationTextList>(query.QueryForListOfLanguages(null), null);

            return ret;
        }

    }

}
