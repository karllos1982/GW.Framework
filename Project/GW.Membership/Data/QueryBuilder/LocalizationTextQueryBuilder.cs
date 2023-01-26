
using GW.Helpers;
using System.Collections.Generic;

namespace GW.Membership.Data
{
    public class LocalizationTextQueryBuilder : QueryBuilder
    {
        public LocalizationTextQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("LocalizationTextID");
        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select * 
            from sysLocalizationText where LocalizationTextID=@pLocalizationTextID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select *           
             from sysLocalizationText 
             where 1=1 
             and (@pLanguage='' or [Language]=@pLanguage)
             and (@pName='' or [Name] like '%' + @pName + '%')
             and (@pCode='' or Code=@pCode)
             and (@pLocalizationTextID=0 or LocalizationTextID=@pLocalizationTextID)      
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {
            string ret = @"select *            
             from sysLocalizationText              
             where 1=1 
             and (@pLanguage='' or [Language]=@pLanguage)
             and (@pName='' or [Name] like '%' + @pName + '%')
             and (@pCode='' or Code=@pCode)
             and (@pLocalizationTextID=0 or LocalizationTextID=@pLocalizationTextID)              
             ";

            return ret;

        }
    }
}
