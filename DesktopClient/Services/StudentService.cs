using DesktopClient.Helpers;
using DesktopClient.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    public class StudentService
    {
        public async Task<bool> SendStudentInformation(StudentInformation studentInformation)
        {
            var json = JsonConvert.SerializeObject(studentInformation);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServiceConfig.ServiceUrl + "/student"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    ExceptionNotifier.NotifyError(result);
                    return false;
                }
            }
        }
    }
}
