using GW.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace GW.Helpers
{
    public static class Utilities 
    {

        public static bool CheckParam(string param, string defaultvalue)
        {
            bool ret = true;

            if (param == null)
            {
                ret = false;
            }
            else
            {
                if (param == "")
                {
                    ret = false;
                }
                else
                {
                    if (defaultvalue != null)
                    {
                        if (param == defaultvalue.ToString())
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;

        }

        public static string ClearHTMLTags(string htmltext)
        {
            string ret = "";

            ret = Regex.Replace(htmltext, "<.*?>", String.Empty);

            return ret;
        }

        public static long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        public static string ConvertoToHTML(string baseurl, string text)
        {
            string ret = text;
            if (text != null)
            {
                Regex regExp = default(Regex);

                regExp = new Regex("[\\n\\v]");
                ret = regExp.Replace(ret, "<br />");

                //Regex for URL tag without anchor
                regExp = new Regex("\\[url\\]([^\\]]+)\\[\\/url\\]");
                ret = regExp.Replace(ret, "<a href=\"$1\">$1</a>");

                //Regex for URL with anchor
                regExp = new Regex("\\[url=([^\\]]+)\\]([^\\]]+)\\[\\/url\\]");
                ret = regExp.Replace(ret, "<a href=\"$1\">$2</a>");

                //Image regex
                regExp = new Regex("\\[object\\]([^\\]]+)\\[\\/object:([^\\]]+)\\:([^\\]]+)\\]");
                ret = regExp.Replace(ret, "<img src=\"" + baseurl + "/$1\" width=\"$2\" height=\"$3\"  />");

                regExp = new Regex("\\[img\\]([^\\]]+)\\[\\/img\\]");
                ret = regExp.Replace(ret, "<img src=\"" + baseurl + "/$1\" alt=\"\"/>");

                regExp = new Regex("\\[img=([^\\]]+)]([^\\]]+)\\[\\/img\\]");
                ret = regExp.Replace(ret, "<img src=\"" + baseurl + "/$2\" alt=\"$1\"/>");

                //Bold text
                regExp = new Regex("\\[b\\](.+?)\\[\\/b\\]");
                ret = regExp.Replace(ret, "<b>$1</b>");

                //Italic text
                regExp = new Regex("\\[i\\](.+?)\\[\\/i\\]");
                ret = regExp.Replace(ret, "<i>$1</i>");

                //Underline text
                regExp = new Regex("\\[u\\](.+?)\\[\\/u\\]");
                ret = regExp.Replace(ret, "<u>$1</u>");

                //Font size
                regExp = new Regex("\\[size=([2-5])]([^\\]]+)\\[\\/size\\]");
                ret = regExp.Replace(ret, "<span style=\"font-size:$1em; font-weight:normal;\">$2</span>");

                regExp = new Regex("\\[size=([^\\]]+)\\]([^\\]]+)\\[\\/size\\]");
                ret = regExp.Replace(ret, "<span style=\"font-size:$1;\">$2</span>");

                regExp = new Regex("\\[small\\]([^\\]]+)\\[\\/small\\]");
                ret = regExp.Replace(ret, "<small>$1</small>");

                regExp = new Regex("\\[sub\\]([^\\]]+)\\[\\/sub\\]");
                ret = regExp.Replace(ret, "<sub>$1</sub>");

                regExp = new Regex("\\[sup\\]([^\\]]+)\\[\\/sup\\]");
                ret = regExp.Replace(ret, "<sup>$1</sup>");

                //Font color
                regExp = new Regex("\\[color=(#[0-9a-fA-F]{6}|[a-z-]+)]([^\\]]+)\\[\\/color\\]");
                ret = regExp.Replace(ret, "<span style=\"color:$1;\">$2</span>");

                regExp = new Regex("\\[del\\]([^\\]]+)\\[\\/del\\]");
                ret = regExp.Replace(ret, "<span style=\"text-decoration:line-through;\">$1</span>");

                //Text alignment
                regExp = new Regex("\\[left\\]([^\\]]+)\\[\\/left\\]");
                ret = regExp.Replace(ret, "<span style=\"text-align:left;\">$1</span>");

                regExp = new Regex("\\[right\\]([^\\]]+)\\[\\/right\\]");
                ret = regExp.Replace(ret, "<span style=\"text-align:right;\">$1</span>");

                regExp = new Regex("\\[center\\]([^\\]]+)\\[\\/center\\]");
                ret = regExp.Replace(ret, "<span style=\"text-align:center;\">$1</span>");

                regExp = new Regex("\\[justify\\]([^\\]]+)\\[\\/justify\\]");
                ret = regExp.Replace(ret, "<span style=\"text-align:justify;\">$1</span>");

                //Lists            
                regExp = new Regex("\\[li\\]([^\\]]+)\\[\\/li\\]");
                ret = regExp.Replace(ret, "<li>$1</li>");

                regExp = new Regex("\\[ulist\\]([^\\]]+)\\[\\/ulist\\]");
                ret = regExp.Replace(ret, "<ul>$1</ul><p>");

                regExp = new Regex("\\[olist\\]([^\\]]+)\\[\\/olist\\]");
                ret = regExp.Replace(ret, "</p><ol>$1</ol><p>");

                //Headers
                regExp = new Regex("\\[h1\\]([^\\]]+)\\[\\/h1\\]");
                ret = regExp.Replace(ret, "<h1>$1</h1>");

                regExp = new Regex("\\[h2\\]([^\\]]+)\\[\\/h2\\]");
                ret = regExp.Replace(ret, "<h2>$1</h2>");

                regExp = new Regex("\\[h3\\]([^\\]]+)\\[\\/h3\\]");
                ret = regExp.Replace(ret, "<h3>$1</h3>");

                regExp = new Regex("\\[h4\\]([^\\]]+)\\[\\/h4\\]");
                ret = regExp.Replace(ret, "<h4>$1</h4>");

                regExp = new Regex("\\[h5\\]([^\\]]+)\\[\\/h5\\]");
                ret = regExp.Replace(ret, "<h5>$1</h5>");

                regExp = new Regex("\\[h6\\]([^\\]]+)\\[\\/h6\\]");
                ret = regExp.Replace(ret, "<h6>$1</h6>");

                //Horizontal rule
                regExp = new Regex("\\[hr\\]");
                ret = regExp.Replace(ret, "<hr />");

                //YouTube
                // regExp = new Regex("\\<!--YouTube Error: bad URL entered-->http:\\/\\/([a-zA-Z]+.)youtube.com\\/watch\\?v=([a-zA-Z0-9_\\-]+)\\[\\/youtube\\]");
                // ret = regExp.Replace(ret, "<object width=\"425\" height=\"344\"><param name=\"movie\" value=\"http://www.youtube.com/v/$2\"></param><param name=\"allowFullScreen\" value=\"true\"></param><embed src=\"http://www.youtube.com/v/$2\" type=\"application/x-shockwave-flash\" allowfullscreen=\"true\" width=\"425\" height=\"344\"></embed></object>");

                //Google and Wikipedia page links
                // regExp = new Regex("\\[google\\]([^\\]]+)\\[\\/google\\]");
                // ret = regExp.Replace(ret, "<a href=\"http://www.google.com/search?q=$1\">$1");

                // regExp = new Regex("\\[wikipedia\\]([^\\]]+)\\[\\/wikipedia\\]");
                //  ret = regExp.Replace(ret, "<a href=\"http://www.wikipedia.org/wiki/$1\">$1</a>");

                //Set a maximum quote depth (In this case, hard coded to 3)
                //for (int i = 1; i <= 3; i++)
                //{
                regExp = new Regex("\\[quote=([^\\]]+)@([^\\]]+)|([^\\]]+)]([^\\]]+)\\[\\/quote\\]");
                ret = regExp.Replace(ret, "</p><div class=\"block\"><blockquote><cite>$1 <a href=\"$3\">wrote</a> on $2</cite><hr /><p>$4</p></blockquote></div></p><p>");
                regExp = new Regex("\\[quote=([^\\]]+)@([^\\]]+)]([^\\]]+)\\[\\/quote\\]");
                ret = regExp.Replace(ret, "</p><div class=\"block\"><blockquote><cite>$1 wrote on $2</cite><hr /><p>$3</p></blockquote></div><p>");
                regExp = new Regex("\\[quote=([^\\]]+)]([^\\]]+)\\[\\/quote\\]");
                ret = regExp.Replace(ret, "</p><div class=\"block\"><blockquote><cite>$1 wrote</cite><hr /><p>$2</p></blockquote></div><p>");
                regExp = new Regex("\\[quote\\]([^\\]]+)\\[\\/quote\\]");
                ret = regExp.Replace(ret, "</p><div class=\"block\"><blockquote><p>$1</p></blockquote></div><p>");
                // }
            }

            //ret = ret.Replace("../../","../Imagens/enem/");

            return ret;

        }

        private const string chars2 = "1A2B3C4D5E6F7G8H9K1N2U3V4X5Y6Z7";
        public static string GenerateCode(int Keys)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            int charLength = chars2.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Keys; i++)
            {
                int idx = random.Next(0, charLength);
                sb.Append(chars2.Substring(idx, 1));
            }
            return sb.ToString();
        }

        public static string ConvertFromBase64(string value)
        {
            string ret = null;
            byte[] b = null;

            try
            {
                b = Convert.FromBase64String(value);
                ret = Encoding.UTF8.GetString(b);

            }
            catch (Exception ex)
            {
                ret = value;
            }

            return ret;

        }

        public static string ConvertToBase64(string value)
        {
            string ret = null;

            try
            {
                byte[] byt = Encoding.UTF8.GetBytes(value);

                ret = Convert.ToBase64String(byt);

            }
            catch (Exception ex)
            {
                ret = value;
            }

            return ret;

        }

        public static PERMISSION_STATE_ENUM CheckPermission(List<UserPermissions> permissions,
          string objectcode, PERMISSION_CHECK_ENUM type)
        {
            PERMISSION_STATE_ENUM ret = PERMISSION_STATE_ENUM.NONE;

            if (permissions != null)
            {
                int i_state = 0;

                List<UserPermissions> list =
                        (from UserPermissions p in permissions
                         where p.ObjectCode == objectcode
                         orderby p.TypeGrant
                         select p).ToList();                

                foreach (UserPermissions p in list)
                {
                    switch (type)
                    {
                        case PERMISSION_CHECK_ENUM.READ:
                            i_state = p.ReadStatus;
                            break;

                        case PERMISSION_CHECK_ENUM.SAVE:
                            i_state = p.SaveStatus;
                            break;

                        case PERMISSION_CHECK_ENUM.DELETE:
                            i_state = p.DeleteStatus;
                            break;

                    }

                    if (i_state == 0)
                    {
                        ret = PERMISSION_STATE_ENUM.NONE;
                    }

                    if (i_state == 1)
                    {
                        ret = PERMISSION_STATE_ENUM.ALLOWED;
                    }

                    if (i_state == -1)
                    {
                        ret = PERMISSION_STATE_ENUM.DENIED;
                    }

                }
            }

            return ret;
        }

        public static bool GetState(int status, bool allownone)
        {
            bool ret = false;

            if (status == 1)
            {
                ret = true;
            }

            if (status == 0 && allownone)
            {
                ret = true;
            }

            return ret;
        }

        public static PermissionsState GetPermissionsState(List<UserPermissions> permissions,
            string objectcode, bool allownone)
        {
            PermissionsState ret = new PermissionsState(false, false, false);

            if (permissions != null)
            {                
                List<UserPermissions> list =
                        (from UserPermissions p in permissions
                         where p.ObjectCode == objectcode
                         orderby p.TypeGrant
                         select p).ToList();

                foreach (UserPermissions p in list)
                {
                                        
                    ret.AllowRead = GetState(p.ReadStatus, allownone);   
                    ret.AllowSave = GetState(p.SaveStatus, allownone);
                    ret.AllowDelete = GetState(p.DeleteStatus, allownone);                  

                }
            }

            return ret;
        }
    }
}
