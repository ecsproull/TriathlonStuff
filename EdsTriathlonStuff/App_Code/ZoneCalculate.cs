using System;
using System.Text.RegularExpressions;

namespace EdsTriathlonStuff.App_Code
{
    public class ZoneCalculate
    {
        static string inputData = string.Empty;
        static string regZones = @"\b[A-Za-z \]{3}[A-Za-z]*\b [0-9:]* - [0-9:]*";
        static string regTime = @"[0-9]{2}";
        //static string regWorkTimes = @"00:[0-9]{2}(?= 00:[0-9]{2})";

        public static string FormatZones(string inputData, bool metric)
        {
            double multiplier = metric ? 1.09361 : 1;
            string output = string.Empty;
            MatchCollection zones = Regex.Matches(inputData, regZones);
            //MatchCollection workload = Regex.Matches(inputData, regWorkTimes);
            for (int i = 0; i < 6; i++)
            {
                string zoneValue = zones[i].Value;
                MatchCollection times = Regex.Matches(zoneValue, regTime);

                if (times.Count == 4)
                {
                    double time1 = (Convert.ToInt32(times[0].Value) * 60 + Convert.ToInt32(times[1].Value)) * multiplier;
                    double time2 = (Convert.ToInt32(times[2].Value) * 60 + Convert.ToInt32(times[3].Value)) * multiplier;

                    double lengthTime;
                    if (time1 > 1000) //Easy Max time
                    {
                        lengthTime = (4 + time2) / 4;
                    }
                    else if (time2 == 0)
                    {
                        lengthTime = (time1 - 4) / 4;
                    }
                    else
                    {
                        lengthTime = (time1 + time2) / 8;
                    }

                    int time1Minutes = (int)(time1 / 60) > 5 ? 5 : (int)(time1 / 60);
                    int time2Minutes = (int)(time2 / 60);

                    int time1Seconds = (int)(time1 % 60);
                    int time2Seconds = (int)(time2 % 60);

                    Match zoneName = Regex.Match(zoneValue.TrimStart(' '), @"^[A-Za-z]", RegexOptions.Multiline);

                    string seconds1 = (time1Seconds < 10) ? "0" + time1Seconds.ToString() : time1Seconds.ToString();
                    string seconds2 = (time2Seconds < 10) ? "0" + time2Seconds.ToString() : time2Seconds.ToString();

                    string length = metric ? " 100/m  LT: " : " 100/y  LT: ";

                    zoneValue = zoneName + " " + time1Minutes + ":" + seconds1 + " - " + time2Minutes + ":" + seconds2 + length + lengthTime.ToString("0.##");
                    output += zoneValue + "\r\n";
                }
            }

            return output;
        }
    }
}
