using System.Text;
using System.Reflection;

namespace GW.ApplicationHelpers
{
    public class HttpConnection
    {
        public HttpConnection(HttpClient http, string baseurl)
        {                                 
            Encoding  = Encoding.UTF8;
            BaseURL = baseurl;
            _http = http;
        }

        private HttpClient _http;
        private HttpRequestMessage request; 

        public void ClearHeaders()
        {
            request.Headers.Clear();
        }

        public void InitRequest()
        {
            request = new HttpRequestMessage();
        }


        public void AddHeader(string name, string value )
        {
            request.Headers.Add( name, value );
        }

        public string BaseURL { get; set; }

        public Encoding Encoding { get; set; }

        
        public async Task<HttpResponseMessage> Get(string endpoint,object[]param)
        {
            HttpResponseMessage ret = null;

            BuildRequest(System.Net.Http.HttpMethod.Get,endpoint,param);
            ret = await _http.SendAsync(request);
            
            return ret;
        }

        public async Task<HttpResponseMessage> PostAsJson(string endpoint, object[] param,
            string content )
        {
            HttpResponseMessage ret = null;

            BuildRequest(System.Net.Http.HttpMethod.Post, endpoint, param);

            StringContent cont = new StringContent(content,
                Encoding, "application/json");

            request.Content = cont;

            ret = await _http.SendAsync(request);

            return ret;
        }

        public async Task<HttpResponseMessage> PostAsStream(string endpoint, object[] param,
         byte[] content)
        {
            HttpResponseMessage ret = null;

            BuildRequest(System.Net.Http.HttpMethod.Post, endpoint, param);            

            ByteArrayContent cont = new ByteArrayContent(content);
           
            request.Content = cont;

            ret = await _http.SendAsync(request);

            return ret;
        }

        //

        private void BuildRequest(System.Net.Http.HttpMethod method, string endpoint, object[] param)
        {
            
            string url = this.BaseURL + endpoint + "?";

            if (param != null)
            {
                foreach (object p in param)
                {
                    var aux = BuildGetQueryString(p);
                    url = url + aux;
                }
            }

            request.Method = method;
            request.RequestUri = new Uri(url);
            
        }

        public static string BuildGetQueryString(object paramobj)
        {
            string ret = "";

            if (paramobj != null)
            {
                Type in_type = paramobj.GetType();
                PropertyInfo[] prop = in_type.GetProperties();

                foreach (PropertyInfo p in prop)
                {
                    if (ret == "")
                    {
                        ret = p.Name + "=" + p.GetValue(paramobj);
                    }
                    else
                    {
                        ret = ret + "&" + p.Name + "=" + p.GetValue(paramobj);
                    }
                }
            }

            return ret;
        }

    //    HttpResponseMessage response = client.GetAsync("api/customer/GetAll").Result;  // Blocking call!  
    //if (response.IsSuccessStatusCode)
    //{
    //    Console.WriteLine("Request Message Information:- \n\n" + response.RequestMessage + "\n");
    //    Console.WriteLine("Response Message Header \n\n" + response.Content.Headers + "\n");
    //    // Get the response
    //    var customerJsonString = await response.Content.ReadAsStringAsync();
    //    Console.WriteLine("Your response data is: " + customerJsonString);

    //    // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
    //    var deserialized = JsonConvert.DeserializeObject<IEnumerable<Customer>>(custome‌​rJsonString);
    //    // Do something with it
    //}

}
}
