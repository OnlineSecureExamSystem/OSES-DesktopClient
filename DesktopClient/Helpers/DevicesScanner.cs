using ManagedBass;
using System.Collections.Generic;

namespace DesktopClient.Helpers
{
    public static class DevicesScanner
    {


        public static List<DeviceInfo> getOutputDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            DeviceInfo device;
            // we start from index 1 because 0 means no device
            for (int i = 1; Bass.GetDeviceInfo(i, out device); i++)
            {
                devices.Add(device);
            }
            return devices;
        }

        public static List<DeviceInfo> getInputAudioDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            DeviceInfo device;

            // we start from index 1 because 0 means no device
            for (int i = 0; Bass.RecordGetDeviceInfo(i, out device); i++)
            {
                devices.Add(device);
            }

            return devices;
        }

        public static void useOutputDevice(int index, int PLAYBACK_SAMPLE_RATE = 8000)
        {
            Bass.Init(index, PLAYBACK_SAMPLE_RATE);
        }

        public static void useInputAudioDevice(int index)
        {
            Bass.RecordInit(index);
        }
    }
}
