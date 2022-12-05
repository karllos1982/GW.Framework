using GW.Core;

namespace GW.ApplicationHelpers
{

    public interface IAppSettingsManager<T> where T : ISettings
    {
        void LoadSettings(HttpClient http = null);

        T Settings { get; set; }
        object EnvironmentSettings { get; set; }
        
    }   


}
