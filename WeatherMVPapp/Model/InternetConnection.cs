using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMVPapp.Model
{
    public class InternetConnection
    {
        static private InternetConnection myconnection;
        static public InternetConnection MyConnection
        {
            get
            {
                if (myconnection == null)
                    myconnection = new InternetConnection();
                return myconnection;
            }
        }
        private InternetConnection() {}

        public bool getConnectionStatus()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "openweathermap.org";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
