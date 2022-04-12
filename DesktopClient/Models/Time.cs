using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Models
{
    public struct Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public Time(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public Time(TimeSpan time)
        {
            Hour = time.Hours;
            Minute = time.Minutes;
            Second = time.Seconds;
        }

        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Hour, Minute, Second);
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} : {2}", Hour, Minute, Second);
        }

        public static Time Parse(string time)
        {
            string[] parts = time.Split(':');
            return new Time(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
    }
}
