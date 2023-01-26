using GW.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Membership.Models
{
    public class LocalizationTextParam
    {
        public LocalizationTextParam()
        {
            pLocalizationTextID= 0;
            pLanguage = "";
            pCode = "";
            pName = "";
            pText = ""; 
        }

        public Int64 pLocalizationTextID { get; set; }

        public string pLanguage { get; set; }

        public string pName { get; set; }

        public string pCode { get; set; }

        public string pText { get; set; }

        
    }

    public class LocalizationTextEntry
    {
        [PrimaryValidationConfig("LocalizationTextID", "Text ID", FieldType.NUMERIC, false, 0)]
        public Int64 LocalizationTextID { get; set; }

        [PrimaryValidationConfig("Language", "Language", FieldType.TEXT, false, 5)]
        public string Language { get; set; }

        [PrimaryValidationConfig("Name", "Name", FieldType.TEXT, false, 50)]
        public string Name { get; set; }

        [PrimaryValidationConfig("Code", "Code", FieldType.TEXT, false, 10)]
        public string Code { get; set; }

        [PrimaryValidationConfig("Text", "Text", FieldType.TEXT, false, 255)]
        public string Text { get; set; }


    }

    public class LocalizationTextList
    {
        
        public Int64 LocalizationTextID { get; set; }
        
        public string Language { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
       

    }

    public class LocalizationTextResult
    {

        public Int64 LocalizationTextID { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Text { get; set; }

    }

}
