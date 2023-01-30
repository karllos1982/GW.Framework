using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface ILocalizationTextRepository :
        IRepository<LocalizationTextParam, LocalizationTextEntry, LocalizationTextResult, LocalizationTextList>
    {

        Task<List<LocalizationTextList>> GetListOfLanguages();
    }
}
