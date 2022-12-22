using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GW
{
    public class Item
    {
        public string Language { get; set; }

        public string Name { get; set; } 

        public string Text { get; set; }

        public string Code { get; set; }

    }

    public class Localization
    {
        
        private static List<Item> _items = null;

        private static void LoadLocalizationENG(ref List<Item> items)
        {
            items.Add(new Item() { Language="eng", Code = "1", Name = "Execution-Error", Text = "Execution error:" });
            items.Add(new Item() { Language = "eng",  Code = "2", Name = "Validation-Error", Text = "Data validation error." });
            items.Add(new Item() { Language = "eng", Code = "3", Name = "Record-NotFound", Text = "The requested record was not found." });
            items.Add(new Item() { Language = "eng", Code = "4", Name = "Login-Invalid-Password", Text = "Invalid password. You still have {0} attempts before the account is deactivated." });
            items.Add(new Item() { Language = "eng", Code = "5", Name = "Login-Attempts", Text = "You have already used access attempts and the account has been disabled. Request activation." });
            items.Add(new Item() { Language = "eng", Code = "6", Name = "Login-Inactive-Account", Text = "The account associated with the User is not active. Request account activation." });
            items.Add(new Item() { Language = "eng", Code = "7", Name = "Login-Locked-Account", Text = "The account associated with the User is locked out. Contact your system administrator." });
            items.Add(new Item() { Language = "eng", Code = "8", Name = "Login-User-NotFound", Text = "User not found." });
            items.Add(new Item() { Language = "eng", Code = "9", Name = "User-Exists", Text = "There is already a user with the email:" });
            items.Add(new Item() { Language = "eng", Code = "10", Name = "Email-Exists", Text = "The email you entered already exists." });
            items.Add(new Item() { Language = "eng", Code = "11", Name = "Profile-NotBe-Null", Text = "The Profile cannot be null." });
            items.Add(new Item() { Language = "eng", Code = "12", Name = "User-Error-Exclude-Childs", Text = "There was an error deleting child items (Roles)." });
            items.Add(new Item() { Language = "eng", Code = "13", Name = "User-Invalid-Password-Code", Text = "The password exchange authorization code is invalid." });
            items.Add(new Item() { Language = "eng", Code = "14", Name = "Account-Active", Text = "The account associated with the User is already active." });
            items.Add(new Item() { Language = "eng", Code = "15", Name = "User-Invalid-Activation-Code", Text = "The activation authorization code is invalid." });
            items.Add(new Item() { Language = "eng", Code = "16", Name = "User-No-Image", Text = "Send the image file." });
            items.Add(new Item() { Language = "eng", Code = "17", Name = "User-Role-Exists", Text = "This Role is already associated with the user." });
            items.Add(new Item() { Language = "eng", Code = "18", Name = "User-Role-No-Exists", Text = "This Role does not belong to the user." });
            items.Add(new Item() { Language = "eng", Code = "19", Name = "Http-Unauthorized", Text = "Unauthorized access" });
            items.Add(new Item() { Language = "eng", Code = "20", Name = "Http-NotFound", Text = "The resource could not be found" });
            items.Add(new Item() { Language = "eng", Code = "21", Name = "Http-Forbidden", Text = "User profile without access permission." });
            items.Add(new Item() { Language = "eng", Code = "22", Name = "Http-500Error", Text = "An error occurred in the processing of the request." });
            items.Add(new Item() { Language = "eng", Code = "23", Name = "Http-ServiceUnavailable", Text = "The requested service is unavailable." });
            items.Add(new Item() { Language = "eng", Code = "24", Name = "API-Unexpected-Exception", Text = "Unexpected error not identified:: GetInnerExceptions@f2]" });
            items.Add(new Item() { Language="eng", Code = "25", Name = "ShortDayName-1", Text = "Sun" });
            items.Add(new Item() { Language="eng", Code = "26", Name = "ShortDayName-2", Text = "Mon" });
            items.Add(new Item() { Language="eng", Code = "27", Name = "ShortDayName-3", Text = "Tue" });
            items.Add(new Item() { Language="eng", Code = "28", Name = "ShortDayName-4", Text = "Wed" });
            items.Add(new Item() { Language="eng", Code = "29", Name = "ShortDayName-5", Text = "Thu" });
            items.Add(new Item() { Language="eng", Code = "30", Name = "ShortDayName-6", Text = "Fri" });
            items.Add(new Item() { Language="eng", Code = "31", Name = "ShortDayName-7", Text = "Sat" });
            items.Add(new Item() { Language="eng", Code = "32", Name = "MonthName-1", Text = "JANUARY" });
            items.Add(new Item() { Language="eng", Code = "33", Name = "MonthName-2", Text = "FEBRUARY" });
            items.Add(new Item() { Language="eng", Code = "34", Name = "MonthName-3", Text = "MARCH" });
            items.Add(new Item() { Language="eng", Code = "35", Name = "MonthName-4", Text = "APRIL" });
            items.Add(new Item() { Language="eng", Code = "36", Name = "MonthName-5", Text = "MAY" });
            items.Add(new Item() { Language="eng", Code = "37", Name = "MonthName-6", Text = "JUNE" });
            items.Add(new Item() { Language="eng", Code = "38", Name = "MonthName-7", Text = "JULY" });
            items.Add(new Item() { Language="eng", Code = "39", Name = "MonthName-8", Text = "AUGUST" });
            items.Add(new Item() { Language="eng", Code = "40", Name = "MonthName-9", Text = "SEPTEMBER" });
            items.Add(new Item() { Language="eng", Code = "41", Name = "MonthName-10", Text = "OCTOBER" });
            items.Add(new Item() { Language="eng", Code = "42", Name = "MonthName-11", Text = "NOVEMBER" });
            items.Add(new Item() { Language="eng", Code = "43", Name = "MonthName-12", Text = "DECEMBER" });
            items.Add(new Item() { Language="eng", Code = "44", Name = "Validation-NotNull", Text = "cannot be null." });
            items.Add(new Item() { Language="eng", Code = "45", Name = "Validation-Max-Characters", Text = "The {0} field cannot have more than {1} characters." });
            items.Add(new Item() { Language="eng", Code = "46", Name = "Validation-Invalid-Field", Text = "The {0} field is invalid." });
            items.Add(new Item() { Language="eng", Code = "47", Name = "Validation-Invalid-UserName", Text = "The {0} field is invalid. Do not use special characters or spaces." });
            items.Add(new Item() { Language="eng", Code = "48", Name = "Validation-Unique-Value", Text = "The {0} field is invalid. Value must be unique." });
            items.Add(new Item() { Language="eng", Code = "49", Name = "User-Instance-Exists", Text = "This Instance is already associated with the user." });
            items.Add(new Item() { Language="eng", Code = "50", Name = "User-Instance-No-Exists", Text = "This Instance does not belong to the user." });
        }

        public static Item GetItem(string codeorname, string language)
        {
            Item ret = null;
            string content = null;

            if (_items == null)
            {
                _items = new List<Item>();                

                try
                {
                    string jsonfile = "localization.json";
                   
                    string filename = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    filename = filename.Replace("\\GW.Framework.dll", "");
                    filename = Path.Combine(filename, jsonfile );
                    if (File.Exists(filename))
                    {
                        content = File.ReadAllText(filename);

                        if (content != null)
                        {
                            _items = JsonConvert.DeserializeObject<List<Item>>(content);
                        }
                    }
                  
                }
                catch (Exception ex)
                {

                }                                           

            }
            
            if (_items != null)
            {
                if (_items.Count == 0)
                {
                    _items = new List<Item>();
                    LoadLocalizationENG(ref _items);
                }

                ret = _items.Where(l => l.Code == codeorname && l.Language==language).FirstOrDefault();

                if (ret == null)
                {
                    ret = _items.Where(l => l.Name == codeorname && l.Language == language).FirstOrDefault();
                }
            }

            if (ret == null)
            {
                ret = new Item() { Code = "0", Name = "RootError", Text = "Error" }; 
            }

            return ret;
        }


    }
    

}
