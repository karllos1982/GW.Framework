using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace GW.ApplicationHelpers
{

    public interface IAppSettingsManager<T> where T : AppSettings
    {
        void LoadSettings(HttpClient http = null);

        T Settings { get; set; }
        object EnvironmentSettings { get; set; }
        
    } 

    public interface AppSettings
    {        

    }



}
