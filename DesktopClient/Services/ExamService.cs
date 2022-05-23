using DesktopClient.Helpers;
using DesktopClient.Models;
using Newtonsoft.Json;
using System;
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
                RequestUri = new Uri(ServiceConfig.ServiceUrl + "/exam"),
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

        public async Task<bool> SendExamAnswers(Exam exam)
        {
            var json = JsonConvert.SerializeObject(exam);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServiceConfig.ServiceUrl + "/exam"),
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Headers = { { "IsSubmit", "false" } }
            };

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception)
                {
                    return false;
                }

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

        public async Task<bool> SubmitExamAnswers(Exam examAnswers)
        {
            var json = JsonConvert.SerializeObject(examAnswers);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServiceConfig.ServiceUrl + "/exam"),
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Headers = { { "IsSubmit", "true" } }
            };

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception)
                {
                    return false;
                }

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
