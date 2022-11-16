using GW.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GW.Helpers
{

        public enum FieldType
        {
            TEXT = 0, //texto geral
            NUMERIC = 1, // númerico
            DATATIME = 2, //data
            CPF = 3, //tipo CPF
            CNPJ = 4, //tipo CNPJ
            PHONENUMBER = 5, //tipo telefone fixo com DDD            
            CEP = 6, //tipo CEP
            EMAIL = 7, //tipo email
            TIME = 8, //tipo hora (HH:MM)
            URL = 9, // tipo url (site)
            USERNAME = 10, //faz a validação verificando se existe caracteres especiais e espaço
            CELLPHONENUMBER = 11 //tipo celular com DDD e nono dígito 
        }

        [AttributeUsage
          (AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)
        ]
        public class PrimaryValidationConfig : Attribute
        {
            public string FieldName { get; set; }
            public string FieldLabel { get; set; }
            public FieldType  DataType { get; set; }
            public bool AllowNull { get; set; }
            public int MaxLength { get; set; }


            public PrimaryValidationConfig(string name, string label, FieldType type,
                bool allownull = true, int maxlength=0)
            {
                FieldName = name;
                FieldLabel = label;
                DataType = type;
                AllowNull = allownull;
                MaxLength = maxlength;
            }
                       
        }

        /// <summary>
        ///Classe para autovalidação primária dos models, com base nos attributes da propriedades
        /// </summary>
        
        public static class PrimaryValidation
        {
           
            public static OperationStatus Execute( object modelobject, List<string> ignorefields)
            {
                OperationStatus ret = new OperationStatus(true);
                Validations val = new Validations();

                if (ignorefields == null)
                {
                    ignorefields = new List<string>();
                }

                System.Type t = modelobject.GetType();
                //System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);
               
                PropertyInfo[] prop = t.GetProperties();
                PropertyInfo pin;
                System.Attribute attr;
                PrimaryValidationConfig configs;

                string value = "";

                foreach (PropertyInfo p in prop)
                {
                    var ignorefield = ignorefields.Where(x => x == p.Name).FirstOrDefault();

                    if (ignorefield == null)
                    {
                        pin = t.GetProperty(p.Name);

                        if (pin != null)
                        {
                            if (pin.PropertyType.Name != "ICollection`1")
                            {
                                if (pin.GetValue(modelobject) != null)
                                {
                                    value = pin.GetValue(modelobject).ToString();
                                }
                                else
                                {
                                    value = "";
                                }

                                if (p.GetCustomAttributes().ToList().Count > 0)
                                {
                                    attr = p.GetCustomAttributes().ToList()[0];
                                    if (attr != null)
                                    {
                                        if (attr is PrimaryValidationConfig)
                                        {
                                            configs = (PrimaryValidationConfig)attr;
                                            val.Validator(ref ret, value, configs.DataType,
                                            configs.FieldName, configs.FieldLabel, configs.AllowNull, configs.MaxLength);

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
             
                return ret;
            }
        }  


        /// <summary>
        ///Classe de validação principal
        /// </summary>
        public class Validations
        {

            /// <summary>
            ///Verifica se tem um caracter numa string
            /// </summary>
            public bool HasCharInString(string word, string character)
            {
                bool ret = false;

                if (word.IndexOf(character) != -1)
                {
                    ret = true;
                }

                return ret;
            }

            /// <summary>
            ///Verifica se tem todas as strings de um array numa string
            /// </summary>
            public bool HasCharsInString(string word, string[] chars)
            {
                bool ret = false;

                foreach (string st in chars)
                {
                    if (word.IndexOf(st) != -1)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                        break;
                    }
                }

                return ret;
            }

            /// <summary>
            ///verifica se o valor passado como parâmetro é data
            /// </summary>
            public bool IsDate(string value)
            {
                bool ret = false;
                DateTime aux;

                try
                {
                    aux = DateTime.Parse(value);
                    ret = true;
                }
                catch (Exception ex)
                {

                }
                return ret;
            }

            /// <summary>
            ///verifica se o valor passado como parâmetro é hora no formato HH:MM
            /// </summary>
            public bool IsTime(string value)
            {
                bool ret = false;
                string s_hour = "";
                string s_minute = "";
                int i_hour = 0;
                int i_minute = 0;

                if (value.Length == 5)
                {
                    if (value.Substring(2, 1) == ":")
                    {
                        s_hour = value.Substring(0, 2);
                        s_minute = value.Substring(3, 2);
                        if (this.IsNumeric(s_hour) && this.IsNumeric(s_minute))
                        {
                            i_hour = int.Parse(s_hour);
                            i_minute = int.Parse(s_minute);
                            if ((i_hour >= 0 && i_hour <= 23) && (i_minute >= 0 && i_minute <= 59))
                            {
                                ret = true;
                            }
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            ///Verifica e o valor passado como parâmetro é numerico
            /// </summary>
            public bool IsNumeric(string value)
            {
                bool ret = false;
                Double aux;

                try
                {
                    value = value.Trim();
                    if (value.Length != 0)
                    {
                        aux = Double.Parse(value);
                        ret = true;
                    }

                }
                catch (Exception ex)
                {

                }
                return ret;
            }



            /// <summary>
            ///Efetua uma validação primária num valor passado como parâmetro de acordo com as especificações 
            /// </summary>
            public void Validator(ref OperationStatus ret, string value, FieldType type, 
                string fieldname, string fieldlabel, bool allownull, int maxlength)
            {
                bool exit = false;

                if (value != null)
                {
                    value = value.Trim();
                }

                if (!allownull)
                {
                    
                    if (value == null)
                    {                            
                        ret.AddInnerException(fieldname, fieldlabel + " " + 
                                GW.Localization.GetItem("Validation-NotNull").Text);
                        exit = true;
                            
                    }
                    else
                    {
                        if (value.Length == 0)
                        {
                        ret.AddInnerException(fieldname, fieldlabel + " " +
                            GW.Localization.GetItem("Validation-NotNull").Text);
                        exit = true;
                        }
                    }
                 
                }
                else
                {
                    if (value == null)
                    {
                        exit = true;
                    }
                }

                if (!exit)
                {
                     string msg = "";
               
                    switch (type)
                        {
                        
                        case FieldType.TEXT:
                            if (value.Length > maxlength)
                            {
                                msg = GW.Localization.GetItem("Validation-Max-Characters").Text;
                                msg.Replace("{fieldlabel}", fieldlabel).Replace("{maxlength}",maxlength.ToString() ); 
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.CPF:
                            if (value.Length > 0 && !this.ValidateCPF(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel ); 
                                ret.AddInnerException(fieldname,msg);
                            }
                            break;

                        case FieldType.CNPJ:
                            if (value.Length > 0 && !this.ValidateCNPJ(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                             }
                            break;

                        case FieldType.CEP:
                            if (value.Length > 0 && !this.ValidateCEP(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.PHONENUMBER:
                            if (value.Length > 0 && !this.ValidatePhone(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.CELLPHONENUMBER:
                            if (value.Length > 0 && !this.ValidateCellPhone(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.DATATIME:
                            if (value != null)
                            {
                                if (value.Length > 0 && !this.IsDate(value))
                                {
                                    msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                    msg.Replace("{fieldlabel}", fieldlabel);
                                    ret.AddInnerException(fieldname, msg);
                                }
                            }
                                                      
                            break;

                        case FieldType.EMAIL:
                            if (value.Length > 0 && !this.ValidateEmail(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.TIME:
                            if (value.Length > 0 && !this.IsTime(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.URL:
                            if (value.Length > 0 && !this.ValidateURL(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-Field").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                            }
                            break;

                        case FieldType.USERNAME:
                            if (value.Length > 0 && !this.ValidateUserName(value))
                            {
                                msg = GW.Localization.GetItem("Validation-Invalid-UserName").Text;
                                msg.Replace("{fieldlabel}", fieldlabel);
                                ret.AddInnerException(fieldname, msg);
                           
                            }
                            break;
                    }
                }
                if (ret.InnerExceptions.Count > 0)
                {
                    ret.Status = false;
                }
            }

            /// <summary>
            ///Valida um valor do tipo CPF
            /// </summary>
            public bool ValidateCPF(string vrCPF)
            {

                string valor = vrCPF.Replace(".", "");

                valor = valor.Replace("-", "");


                if (!this.IsNumeric(valor))
                    return false;

                if (valor.Length != 11 || !this.IsNumeric(valor))
                    return false;

                bool igual = true;

                for (int i = 1; i < 11 && igual; i++)
                    if (valor[i] != valor[0])
                        igual = false;

                if (igual || valor == "12345678909")
                    return false;

                int[] numeros = new int[11];

                for (int i = 0; i < 11; i++)
                    numeros[i] = int.Parse(
                        valor[i].ToString());
                int soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += (10 - i) * numeros[i];

                int resultado = soma % 11;

                if (resultado == 1 || resultado == 0)
                {
                    if (numeros[9] != 0)
                        return false;
                }

                else if (numeros[9] != 11 - resultado)
                    return false;

                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += (11 - i) * numeros[i];
                resultado = soma % 11;

                if (resultado == 1 || resultado == 0)
                {
                    if (numeros[10] != 0)
                        return false;
                }

                else

                    if (numeros[10] != 11 - resultado)
                        return false;

                return true;

            }

            /// <summary>
            ///Valida um valor do tipo CNPJ
            /// </summary>
            public bool ValidateCNPJ(string vrCNPJ)
            {
                string CNPJ = vrCNPJ.Replace(".", "");
                CNPJ = CNPJ.Replace("/", "");
                CNPJ = CNPJ.Replace("-", "");

                if (!this.IsNumeric(vrCNPJ))
                    return false;

                int[] digitos, soma, resultado;
                int nrDig;
                string ftmt;
                bool[] CNPJOk;

                ftmt = "6543298765432";
                digitos = new int[14];
                soma = new int[2];
                soma[0] = 0;
                soma[1] = 0;
                resultado = new int[2];
                resultado[0] = 0;
                resultado[1] = 0;
                CNPJOk = new bool[2];

                CNPJOk[0] = false;
                CNPJOk[1] = false;

                try
                {
                    for (nrDig = 0; nrDig < 14; nrDig++)
                    {
                        digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));

                        if (nrDig <= 11)
                            soma[0] += (digitos[nrDig] *
                                int.Parse(ftmt.Substring(nrDig + 1, 1)));

                        if (nrDig <= 12)
                            soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));

                    }

                    for (nrDig = 0; nrDig < 2; nrDig++)
                    {
                        resultado[nrDig] = (soma[nrDig] % 11);
                        if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1)) CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);

                        else

                            CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                    }
                    return (CNPJOk[0] && CNPJOk[1]);
                }

                catch
                {
                    return false;
                }

            }

            /// <summary>
            ///Valida um valor do tipo CEP
            /// </summary>
            public bool ValidateCEP(string value) //62360-000
            {
                bool ret = true;

                string pattern = (@"^\d{5}-\d{3}$");

                ret = MatchRegex(pattern, value);
                
                if (!ret)
                {
                    pattern = (@"^\d{8}$");

                    ret = MatchRegex(pattern, value);
                 }

                return ret;
            }

            /// <summary>
            ///Valida um valor do tipo Telefone com ddd
            /// </summary>
            public bool ValidatePhone(string value) //(99)9999-9999 
            {
                bool ret = true;

                string pattern = (@"^[(]{1}\d{2}[)]{1}\d{4}[-]{1}\d{4}$");
                ret = MatchRegex(pattern, value);

                if (!ret)
                {
                    pattern = (@"^\d{10}$");

                    ret = MatchRegex(pattern, value);
                }

                return ret;
            }

            public bool ECel9Digitos(string ddd)
            {
                var ret = false;
                string[] ddds = { "11", "12", "13", "14", "15", "16", "17", "18", "19", "21", "22", "24", "27", "28" };

                for (int i = 0; i < ddds.Length - 1; i++)
                {
                    if (ddds[i] == ddd)
                    {
                        ret = true;
                        break;
                    }
                }
                return ret;
            }

            public bool MatchRegex(string pattern, string value)
            {
                bool ret = false;
                Regex rg = new Regex(pattern);
                Match match = rg.Match(value);
                ret = match.Success;

                return ret;
            }


            /// <summary>
            ///Valida um valor do tipo Telefone Celular com ddd e novo dígito
            /// </summary>
            /// 
            public bool ValidateCellPhone(string value) //(99)9999-9999 ou (99)99999-9999 
            {
                bool ret = true;

                string pattern = (@"^[(]{1}\d{2}[)]{1}\d{5}[-]{1}\d{4}$");
                ret = MatchRegex(pattern, value);

                if (!ret)
                {
                    pattern = (@"^\d{11}$");

                    ret = MatchRegex(pattern, value);
                }

                return ret;
            }

            /// <summary>
            ///Valida um valor do tipo E-mail
            /// </summary>
            public bool ValidateEmail(string value)
            {
                bool ret = true;
                string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

                ret = MatchRegex(pattern, value);
                return ret;
            }

            /// <summary>
            ///Valida um valor do tipo URL
            /// </summary>
            public bool ValidateURL(string value)
            {
                bool ret = true;
                string pattern = @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                ret = MatchRegex(pattern, value);

                return ret;
            }

            /// <summary>
            ///Valida um valor do tipo URL 
            /// tudo menos tag html [a-zA-Z0-9!¡$%&/\()=?¿*+-_{};:,áéíóú'.\s][^>][^<]{1,250}
            /// </summary>
            public bool ValidateUserName(string value)
            {
                bool ret = true;
                string pattern = @"^(?=.{1,255}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$";

                ret = MatchRegex(pattern, value);

                return ret;
            }

        }
    

}
