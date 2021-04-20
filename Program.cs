using System.Collections.Generic;
using System.Net.Mime;
using System.Net.Http;
using System;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;


namespace ForFiServ
{
    
    class Program
    {
        private readonly static string _TOKENURL = @"https://testidentity.csiweb.com"; //SOFTCODE THIS CHANGES WHEN GOING OT PRODUCTION
        private readonly static string _CLIENTID = "add_yours";
        private readonly static string _CLIENTSECRET = "add_yours";
        private readonly static string _SCOPES = "cif_read";

        private static HttpClient _tokenClient;

        static async Task Main(string[] args)
        {
            _tokenClient = new HttpClient();


            Console.WriteLine("Hello World!");
            var obj = await GetToken();
        }

        static async Task<string> GetToken(){

            string accessToken = string.Empty;
            var parameters = new Dictionary<string,string>(){ {"grant_type", "client_credentials"},
                                                            {"scope", _SCOPES},
                                                            {"client_id", _CLIENTID},
                                                            {"client_secret", _CLIENTSECRET}};
            var encodedContent = new FormUrlEncodedContent( parameters );
            HttpRequestMessage tokenReq = new HttpRequestMessage(){
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(_TOKENURL), "connect/token"),
                Content =  encodedContent
            };
            tokenReq.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
        

            try{

                var httpResponse = await _tokenClient.SendAsync( tokenReq );
                if(httpResponse.StatusCode != System.Net.HttpStatusCode.OK){
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get token");
                }
                else{
                    accessToken = await httpResponse.Content.ReadAsStringAsync();
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Got the following token { accessToken } ");
                }

            }
            catch( Exception ex )
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to get token { ex.Message }");
            }

            return accessToken;
            
        }
    }
}
