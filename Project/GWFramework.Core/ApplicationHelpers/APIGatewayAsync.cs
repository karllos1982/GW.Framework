using GW.Common;
using GW.Helpers;
using Newtonsoft.Json;
using System.Net;


namespace GW.ApplicationHelpers
{
    public class APIGatewayResponse
    {
        public APIGatewayResponse()
        {
            Message = "";
            StatusOK = false;
        }

        public HttpResponseMessage Response { get; set; }

        public string Message { get; set; }

        public bool StatusOK { get; set; }
       
        public List<InnerException> ExecutionExeceptions { get; set; }

    }

    public class APIGatewayManagerAsync
    {
       
        public APIGatewayManagerAsync()
        {

        }

        public void InitializeAPI(HttpClient http, string baseurl, string token)
        {
            this.Requestor = new HttpConnection(http, baseurl); 
            this.BaseURL = baseurl;
            this.Token = new AuthToken() { TokenValue = token };            
        }

        public bool IsAuthenticated { get; set; }

        public HttpConnection Requestor { get; set; }

        public APIGatewayResponse APIResponse { get; set; }
        
        public string BaseURL { get; set; }

        public AuthToken Token { get; set; }

        private void SetRequest(string url)
        {
            Requestor.InitRequest(); 
            Requestor.ClearHeaders();            
            Requestor.AddHeader("Authorization", "Bearer " + Token.TokenValue); 
        }

      
        private async Task  GetStatusRequest (HttpResponseMessage response, string lang="")
        {
             APIResponse = new APIGatewayResponse();
            
             APIResponse.Response = response;   

            if (!response.IsSuccessStatusCode)
            {
                APIResponse.StatusOK = false;
                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        APIResponse.Message = GW.Localization.GetItem("Http-Unauthorized", lang).Text;
                        break;

                    case HttpStatusCode.NotFound:
                        APIResponse.Message = GW.Localization.GetItem("Http-NotFound", lang).Text;
                        break;

                    case HttpStatusCode.Forbidden:
                        APIResponse.Message = GW.Localization.GetItem("Http-Forbidden", lang).Text;
                        break;

                    case HttpStatusCode.InternalServerError:
                        APIResponse.Message = GW.Localization.GetItem("Http-500Error", lang).Text;
                                               
                        break;

                    case HttpStatusCode.ServiceUnavailable:
                        APIResponse.Message = GW.Localization.GetItem("Http-ServiceUnavailable", lang).Text;
                        break;
                }

                try
                {
                       
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (jsonString != null)
                    {
                        APIResponse.ExecutionExeceptions
                            = JsonConvert.DeserializeObject<List<InnerException>>(jsonString);
                    }
                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                APIResponse.StatusOK = true;
            }
  
        }

        //

        public async Task<T> GetAsJSON<T>(string endpoint,object[] param)            
        {
            T ? ret = default(T);

            SetRequest(endpoint);

            HttpResponseMessage response = await Requestor.Get(endpoint, param);

             await GetStatusRequest(response);

            if (APIResponse.StatusOK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString != null)
                {                   
                   ret = JsonConvert.DeserializeObject<T>(jsonString);                    
                }                
            }

            return ret;
        }

        public async Task<T> PostAsJSON<T>(string endpoint, string data, object[] param)
        {
            T ? ret = default(T);

            SetRequest(endpoint);

            HttpResponseMessage response = await Requestor.PostAsJson(endpoint, param,data);

            await GetStatusRequest(response);

            if (APIResponse.StatusOK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString != null)
                {
                    ret = JsonConvert.DeserializeObject<T>(jsonString);
                }
            }

            return ret;
        }
        
        public async Task<T> ? PostAsStream<T>(string endpoint, byte[] data, object[] param)
        {
            T ? ret = default(T);

            SetRequest(endpoint);

            HttpResponseMessage response = await Requestor.PostAsStream(endpoint, param, data);

            await GetStatusRequest(response);

            if (APIResponse.StatusOK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString != null)
                {
                    ret = JsonConvert.DeserializeObject<T>(jsonString);
                }
            }

            return ret;
        }


        public List<InnerException> GetInnerExceptions(ref Exception defaulterror)
        {            
            if (this.APIResponse.ExecutionExeceptions != null)
            {
                if (this.APIResponse.ExecutionExeceptions.Count > 0)
                {
                    InnerException inner = this.APIResponse.ExecutionExeceptions[0];
                    defaulterror = new Exception(inner.Description); 
                }
                    
                return this.APIResponse.ExecutionExeceptions;
            }
            else
            {
                defaulterror = new Exception(GW.Localization.GetItem("API-Unexpected-Exception","eng").Text);
                return null;
            }

        }

    }


}