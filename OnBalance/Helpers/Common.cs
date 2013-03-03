using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;

namespace OnBalance.Helpers
{
    public class Common
    {

        /// <summary>
        /// Unix epoch start, used for timestamp calculations
        /// </summary>
        private static DateTime m_unixStart = new DateTime(1970, 1, 1, 0, 0, 0);

        public static Dictionary<string, object> DynamicObjectToDictionaryInsensitive(object o)
        {
            Dictionary<string, object> ar = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(o))
            {
                ar[pd.Name] = pd.GetValue(o);
            }
            return ar;
        }

        public static Dictionary<string, object> DynamicObjectToDictionary(object o)
        {
            Dictionary<string, object> ar = new Dictionary<string, object>();
            foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(o))
            {
                ar[pd.Name] = pd.GetValue(o);
            }
            return ar;
        }

        /// <summary>
        /// Converts DateTime value to Unix timestamp
        /// </summary>
        /// <param name="dt">DateTime to convert</param>
        public static double GetTimestamp(DateTime dt)
        {
            return Math.Floor((dt - m_unixStart).TotalSeconds);
        }

        /// <summary>
        /// Returns current Unix timestamp
        /// </summary>
        public static double GetTimestamp()
        {
            return GetTimestamp(DateTime.Now);
        }

        /// <summary>
        /// Converts Unix timestamp to DateTime object
        /// </summary>
        /// <param name="timestamp">Timestamp to convert</param>
        public static DateTime TimestampToDateTime(double timestamp)
        {
            return m_unixStart.AddSeconds(timestamp);
        }

        public static string TimeSpanToStringAgo(DateTime date)
        {
            int s = (int)(DateTime.Now - date).TotalSeconds;
            string fmt = "";

            if(s < 3600)
            {
                fmt = "%minutes% %seconds%";
            }
            else if(s < 24 * 3600)
            {
                fmt = "%hours% %minutes%";
            }
            else
            {
                fmt = "%days% %hours%";
            }

            return TimeSpanToStringAgo(date, DateTime.Now, fmt);
        }

        /// <summary>
        /// Returns string, replacing %VAR% w/ values, i.e. "34 hours 13 seconds ago"
        /// </summary>
        /// <param name="date">Date in past</param>
        /// <param name="now">Date in future</param>
        /// <param name="fmt">String could contains %days% %hours% %minutes% %seconds%</param>
        public static string TimeSpanToStringAgo(DateTime date, DateTime now, string fmt)
        {
            TimeSpan ts = now - date;
            int v = 0;

            if(fmt.IndexOf("%days%") != -1)
            {
                v = (int)Math.Floor(ts.TotalDays);
                fmt = fmt.Replace("%days%", v > 0 ? v.ToString() + " days" : "");
                now = now.AddDays(0 - v);
                ts = now - date;
            }

            if(fmt.IndexOf("%hours%") != -1)
            {
                v = (int)Math.Floor(ts.TotalHours);
                fmt = fmt.Replace("%hours%", v > 0 ? v.ToString() + " hours" : "");
                now = now.AddHours(0 - v);
                ts = now - date;
            }

            if(fmt.IndexOf("%minutes%") != -1)
            {
                v = (int)Math.Floor(ts.TotalMinutes);
                fmt = fmt.Replace("%minutes%", v > 0 ? v.ToString() + " minutes" : "");
                now = now.AddMinutes(0 - v);
                ts = now - date;
            }

            if(fmt.IndexOf("%seconds%") != -1)
            {
                v = (int)Math.Floor(ts.TotalSeconds);
                fmt = fmt.Replace("%seconds%", v > 0 ? v.ToString() + " seconds" : "");
                now = now.AddSeconds(0 - v);
                ts = now - date;
            }

            return fmt.Trim();
        }

        public static string SafeCutTo(string s, int len, string appendix)
        {
            if(len < 0)
            {
                throw new ArgumentException("Length to cut should be greater than 0!", "len");
            }

            StringBuilder cut = new StringBuilder(len);

            // Need cutting
            if(s.Length > len)
            {
                string[] ar = s.Split(new char[] { ' ' });
                int p = 0;
                while(cut.Length + ar[p].Length <= len)
                {
                    cut.Append(ar[p] + " ");
                    p++;
                }

                if(cut.Length < 1)
                {
                    cut.Append(s.Substring(0, len));
                }

                // Add only if string was cut
                cut.Append(appendix);

            }
            else
            {
                cut = new StringBuilder(s);
            }

            return cut.Length < 1 ? "-" : cut.ToString();
        }

    }
}
