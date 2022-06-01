using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    public class AuthenticationService
    {
        public static bool IsAuthenticated { get; set; }

        public static string Token { get; set; }


        public async Task<HttpResponseMessage> Login(string email, string password)
        {
            var json = JsonConvert.SerializeObject(new { email = email, password = password });
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ServiceConfig.ServiceUrl + "/user"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                return response;
            }
        }
        public async Task SendCodeToEmail()
        {
            await Task.Delay(1000);
        }

        public async Task SendCodeToSMS()
        {
            await Task.Delay(1000);
        }

        public async Task<bool> VerifyCode(string code)
        {
            await Task.Delay(1000);
            if (code == "0000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
