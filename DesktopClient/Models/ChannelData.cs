using ManagedBass;

namespace DesktopClient.Models
{
    public class ChannelData
    {
        public ChannelData(string device, int Handle)
        {
            _device = device;
            this.Handle = Handle;
        }

        readonly string _device;

        public int Handle;

        public Vector3D Position = new Vector3D();

        public readonly Vector3D Velocity = new Vector3D();

        public override string ToString() => _device;
    }
}
