using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Diagnostics.SymbolStore;

namespace GW.Helpers
{
    public abstract class QueryBuilder
    {

        public QuerySelectBuilder SelectBuilder = new QuerySelectBuilder();
            
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

    public enum JOINTYPE
    {
        NONE = 0,
        INNER = 1,
        LEFT = 2,
        RIGHT = 3
    }

    public enum ORDERBYTYPE
    {
        NONE = 0,
        ASC = 1,
        DESC = 2
    }

    public record TableDef
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public bool AllColmuns { get; set; }
        public string PKFieldName { get; set; }

        public string JoinFieldName { get; set; }

        public JOINTYPE JoinType { get; set; }

        public TableDef JoinTable { get; set; }
    }

    public record FieldDef
    {
        public string TableAlias { get; set; }
        public string FieldName { get; set; }        
        public string ParamKey { get; set; }
        public bool DisplayInSelect { get; set; }
        public string AutoWhere { get; set; }       
        public string StaticWhere { get; set; }
        public ORDERBYTYPE OrderType { get; set; }
    }

    public record WhereClausuleDef
    {
       
        public FieldDef Field { get; set; }

        public bool AutoClausule { get; set; }

        public string DefaultCriteriaValue { get; set; }

        public string StaticCriteriaValue { get; set; }
    }

    public class QuerySelectBuilder
    {
        private List<TableDef> _Tables;
        public List<TableDef> Tables { get { return _Tables; } }

        private List<FieldDef> _Fields;
        public List<FieldDef> Fields { get { return _Fields; } }

       

        public QuerySelectBuilder()
        {
            _Tables = new List<TableDef>();
            _Fields = new List<FieldDef>();          

        }

        public void AddTable(string name, string alias, bool allcolumns, 
            string pk, string joinfield , JOINTYPE join, TableDef jointable )
        {
            var obj = new TableDef
            {
                Name = name,
                Alias = alias,
                AllColmuns= allcolumns,
                PKFieldName = pk,
                JoinFieldName = joinfield,
                JoinType = join,
                JoinTable = jointable
            };

            _Tables.Add(obj); 
        }

        public void AddField( string tablealias, string name,
          string paramkey, bool display, string autowhere, string staticwhere, ORDERBYTYPE order)
        {
            var obj = new FieldDef
            {                
                TableAlias = tablealias,
                FieldName=name,              
                ParamKey=paramkey,
                DisplayInSelect = display,
                AutoWhere = autowhere,
                StaticWhere =staticwhere,   
                OrderType =order
            };

            _Fields.Add(obj);
        }
      
        
        public string BuildQuery(string extrawhere = null)
        {
            StringBuilder strq = new StringBuilder();
            string text = "";

            text = "SELECT ";
          
            foreach (TableDef t in _Tables)
            {
                if (t.AllColmuns) { strq.Append($"{t.Alias}.*,");}                
            }

            if (strq.Length > 0)
            {
                text = text + strq.ToString();

                if (_Fields.Where(f => f.DisplayInSelect == true).ToList().Count == 0)
                { 
                    text = text.Remove(text.Length - 1, 1);
                }
            }

            strq.Clear();

            foreach (FieldDef f in _Fields)
            {
                if (f.DisplayInSelect)
                {
                    var f2 = _Tables.Where(t => t.Alias == f.TableAlias && t.AllColmuns).FirstOrDefault();
                    if (f2 == null)
                    {
                        strq.Append($"{f.TableAlias}.{f.FieldName},");
                    }
                }
            }

            if (strq.Length > 0) 
            {
                text = text + strq.ToString();
                text = text.Remove(text.Length - 1, 1);
            }
                        
            strq.Clear();

            text = text + " FROM ";
            string join_text = "";

            foreach (TableDef t in _Tables)
            {
                
                if (t.JoinType== JOINTYPE.NONE ) 
                {
                    strq.Append($"{t.Name} {t.Alias} "); 
                }
                else
                {
                    switch (t.JoinType)
                    {
                        case JOINTYPE.INNER:
                            join_text = "INNER JOIN";
                            break;

                        case JOINTYPE.LEFT: 
                            join_text = "LEFT JOIN";
                            break;

                        case JOINTYPE.RIGHT:
                            join_text = "RIGHT JOIN";
                            break;  

                    }

                    strq.Append($"{join_text} {t.Name} {t.Alias}{" on "} "+
                         $"{t.JoinTable.Alias}.{t.JoinTable.PKFieldName} = {t.Alias}.{t.JoinFieldName} ");
                }
            }

            text = text + strq.ToString();       
            strq.Clear();

            text = text + " where 1=1 ";

            foreach (FieldDef w in _Fields)
            {            
                if (w.AutoWhere != null)
                {              
                    strq.Append($" and ({w.ParamKey}={w.AutoWhere} or  {w.TableAlias}.{w.FieldName}={w.ParamKey})");
                           
                }

                if (w.StaticWhere != null && w.StaticWhere != "" )
                {
                    strq.Append($" and {w.TableAlias}.{w.FieldName}={w.StaticWhere} ");
                }

            }

            text = text + strq.ToString();       
            strq.Clear();

            if (extrawhere != null)
            {
                text = text + " " +  extrawhere;
            }

            text = text + " order by ";

            foreach (FieldDef f in _Fields)
            {
                if (f.OrderType != ORDERBYTYPE.NONE)
                {
                    if (f.OrderType != ORDERBYTYPE.ASC)
                    {
                        strq.Append($"{f.TableAlias}.{f.FieldName} ASC,");
                    }

                    if (f.OrderType != ORDERBYTYPE.DESC)
                    {
                        strq.Append($"{f.TableAlias}.{f.FieldName} DESC,");
                    }

                }                                
            }

            if (strq.Length > 0)
            {
                text = text + strq.ToString();
                text = text.Remove(text.Length - 1, 1);
            }          
            
            return text;

        }

        public void Clear()
        {
            _Tables = new List<TableDef>();
            _Fields = new List<FieldDef>();            
        }
    }

}
