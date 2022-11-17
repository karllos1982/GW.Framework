using System.Text;
using System.Reflection;

namespace GW.Helpers
{
    public abstract class QueryBuilder
    {

        public List<string> Keys { get; set; }

        public List<string> ExcludeFields { get; set; }

        public abstract void Initialize();

        public abstract string QueryForGet(object param);

        public abstract string QueryForList(object param);

        public abstract string QueryForSearch(object param);

        public string QueryForCreate(string tablename, object baseobject, object param)
        {
            string ret = "";

            ret = BuildInsertCommand(tablename,
                baseobject, ExcludeFields);

            return ret;
        }

        public string QueryForUpdate(string tablename, object baseobject, object param)
        {
            string ret = "";

            ret = BuildUpdateCommand(tablename,
                baseobject, Keys, ExcludeFields);

            return ret;
        }

        public string QueryForDelete(string tablename, object baseobject, object param)
        {
            string ret = "";

            ret = BuildDeleteCommand(tablename, Keys);

            return ret;
        }

        public string BuildInsertCommand(string tablename,
            object baseobject, List<string> excludefields)
        {
            string ret = "";
            string pname = "";
            StringBuilder strq = new StringBuilder();
            StringBuilder strq_v = new StringBuilder();
            bool flag1 = true;
            bool flag2 = true;
            bool go = false;
            Type meta = baseobject.GetType();
            PropertyInfo[] prop = meta.GetProperties();

            strq.Append("insert into " + tablename + "(");
            strq_v.Append("values (");

            foreach (PropertyInfo p in prop)
            {
                pname = p.Name;
                go = true;

                if (pname != "ICollection`1")
                {

                    if (excludefields != null)
                    {
                        go = !excludefields.Exists(s => s == pname);
                    }

                    if (go)
                    {
                        if (flag1)
                        {
                            strq.Append(pname);
                            flag1 = false;
                        }
                        else
                        {
                            strq.Append("," + pname);
                        }

                        if (flag2)
                        {
                            strq_v.Append("@" + pname);
                            flag2 = false;
                        }
                        else
                        {
                            strq_v.Append(",@" + pname);
                        }
                    }

                }
            }

            strq.Append(")");
            strq_v.Append(")");

            ret = strq.ToString() + strq_v.ToString();

            return ret;
        }

        public string BuildUpdateCommand(string tablename,
         object baseobject, List<string> keyfields, List<string> excludefields)
        {
            string ret = "";
            string pname = "";
            StringBuilder strq = new StringBuilder();
            bool flag1 = true;
            bool go = false;
            bool go_k = false;
            Type meta = baseobject.GetType();
            PropertyInfo[] prop = meta.GetProperties();

            strq.Append("update " + tablename);
            strq.Append(" set ");

            foreach (PropertyInfo p in prop)
            {
                pname = p.Name;
                go = true;
                go_k = true;

                if (pname != "ICollection`1")
                {

                    if (excludefields != null)
                    {
                        go = !excludefields.Exists(s => s == pname);
                    }

                    if (keyfields != null)
                    {
                        go_k = !keyfields.Exists(s => s == pname);
                    }

                    if (go && go_k)
                    {
                        if (flag1)
                        {
                            strq.Append(pname + "=" + "@" + pname);
                            flag1 = false;
                        }
                        else
                        {
                            strq.Append("," + pname + "=" + "@" + pname);
                        }

                    }

                }
            }

            strq.Append(" where  ");

            flag1 = true;

            foreach (string s in keyfields)
            {

                if (flag1)
                {
                    strq.Append(s + "=" + "@" + s);
                    flag1 = false;
                }
                else
                {
                    strq.Append(" and " + s + "=" + "@" + s);
                }

            }

            ret = strq.ToString();

            return ret;
        }

        public string BuildDeleteCommand(string tablename,
              List<string> keyfields)
        {
            string ret = "";
            StringBuilder strq = new StringBuilder();
            bool flag1 = true;
            strq.Append("delete from " + tablename);
            strq.Append(" where ");

            foreach (string s in keyfields)
            {

                if (flag1)
                {
                    strq.Append(s + "=" + "@" + s);
                    flag1 = false;
                }
                else
                {
                    strq.Append(" and " + s + "=" + "@" + s);
                }

            }

            ret = strq.ToString();

            return ret;
        }
    }
}
