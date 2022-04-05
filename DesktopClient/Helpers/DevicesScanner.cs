using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;

namespace DesktopClient.Helpers
{
    public static class DevicesScanner
    {
        public static  List<DeviceInfo> getOutputDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            // we start from index 1 because 0 means no device
            for (int i = 1; i < Bass.DeviceCount; i++)
            {
                devices.Add(Bass.GetDeviceInfo(i));
            }
            return devices;
        }

        public static List<DeviceInfo> getInputAudioDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            // we start from index 1 because 0 means no device
            for (int i = 1; i < Bass.RecordingDeviceCount; i++)
            {
                devices.Add(Bass.GetDeviceInfo(i));
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
