using Avalonia.Controls.Notifications;
using DesktopClient.Models.DataTransferObjects;
using DesktopClient.Views;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace DesktopClient.Helpers
{
    internal class ApiNotifier
    {
        public static void Notify(HttpResponseMessage response)
        {
            string responseContent = response.Content.ReadAsStringAsync().Result;

            NodeJsApiResponseObject Object = JsonConvert.DeserializeObject<NodeJsApiResponseObject>(responseContent);
            MainWindow.WindowNotificationManager.Show(new Notification(((int)response.StatusCode) + " " + response.StatusCode.ToString(),
                                          Object.message,
                                          getNotificationType(response.StatusCode)));
        }

        private static NotificationType getNotificationType(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Found:
                case HttpStatusCode.Continue:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.SwitchingProtocols:
                case HttpStatusCode.Processing:
                case HttpStatusCode.EarlyHints:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                case HttpStatusCode.OK: return NotificationType.Success;

                case HttpStatusCode.UnsupportedMediaType:
                case HttpStatusCode.Gone:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.PaymentRequired:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.NotAcceptable:
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.RequestUriTooLong:
                case HttpStatusCode.RequestEntityTooLarge:
                case HttpStatusCode.PreconditionFailed:
                case HttpStatusCode.UnprocessableEntity:
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.Locked:
                case HttpStatusCode.FailedDependency:
                case HttpStatusCode.ExpectationFailed:
                case HttpStatusCode.MisdirectedRequest:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.ProxyAuthenticationRequired:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.HttpVersionNotSupported:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NetworkAuthenticationRequired:
                case HttpStatusCode.Forbidden: return NotificationType.Error;

                case HttpStatusCode.LengthRequired:
                case HttpStatusCode.UpgradeRequired:
                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.NonAuthoritativeInformation:
                case HttpStatusCode.Moved:
                case HttpStatusCode.AlreadyReported:
                case HttpStatusCode.IMUsed:
                case HttpStatusCode.PreconditionRequired:
                case HttpStatusCode.TooManyRequests:
                case HttpStatusCode.RequestHeaderFieldsTooLarge:
                case HttpStatusCode.UnavailableForLegalReasons:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.VariantAlsoNegotiates:
                case HttpStatusCode.InsufficientStorage:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.ResetContent:
                case HttpStatusCode.PartialContent:
                case HttpStatusCode.MultiStatus:
                case HttpStatusCode.LoopDetected:
                case HttpStatusCode.NotModified:
                case HttpStatusCode.UseProxy:
                case HttpStatusCode.Unused:
                case HttpStatusCode.RedirectKeepVerb:
                case HttpStatusCode.PermanentRedirect:
                case HttpStatusCode.NotExtended: return NotificationType.Warning;
                default:
                    return NotificationType.Information;
            }
        }
    }
}
