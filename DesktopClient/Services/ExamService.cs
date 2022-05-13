using DesktopClient.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    public class ExamService
    {
        public async Task<Exam> GetExamAsync(string examCode)
        {
            var json = JsonConvert.SerializeObject(examCode);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new System.Uri(ServiceConfig.ServiceUrl + "/exam"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Exam>();
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }
    }
}
