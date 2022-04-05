using SpeedTest.Net;
using SpeedTest.Net.Enums;
using System.Threading.Tasks;

namespace DesktopClient.Helpers
{
    public class SpeedTest
    {
        public async static Task<double> getInternetSpeed()
        {
            var speed = await FastClient.GetDownloadSpeed(SpeedTestUnit.MegaBytesPerSecond);
            return speed.Speed * 8;
        }
    }
}
