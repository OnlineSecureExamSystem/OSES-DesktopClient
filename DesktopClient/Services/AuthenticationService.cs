using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    public class AuthenticationService
    {
        public static bool IsAuthenticated { get; set; }

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
