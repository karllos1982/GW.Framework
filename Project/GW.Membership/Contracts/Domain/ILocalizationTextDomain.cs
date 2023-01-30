using GW.Common;
using GW.Core;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;

namespace GW.Membership.Contracts.Domain
{
    public interface ILocalizationTextDomain :
        IDomain<LocalizationTextParam, LocalizationTextEntry, LocalizationTextResult, LocalizationTextList>
    {
        IMembershipRepositorySet RepositorySet { get; set; }

        Task<List<LocalizationTextList>> GetListOfLanguages();

    }
}
