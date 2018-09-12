using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeatherMVPapp.Model;

namespace WeatherMVPapp.Services
{
    public class WeatherAPI
    {
        //--------------------------------------------------------------------------------------------------------
        private const string APPID = "babc914c7d24672584419e75857edcd7";
        //--------------------------------------------------------------------------------------------------------
        public CurrentWeather getWeather(string city)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={APPID}&units=metric&cnt=1";
                string json = "";

                try
                {
                    json = web.DownloadString(url);
                }
                catch (WebException ex)
                {
                    throw ex;
                }

                return JsonConvert.DeserializeObject<CurrentWeather>(json);
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public weatherForcast getForcast(string city)
        {
            string url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&APPID={APPID}";
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(url);
                return JsonConvert.DeserializeObject<weatherForcast>(json);
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void DownloadIcon(string iconID)
        {
            string url = $"http://openweathermap.org/img/w/{iconID}.png";
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            using (var weatherIcon = response.GetResponseStream())
            {
                var weatherImg = Image.FromStream(weatherIcon);
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icons");

                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, $"{iconID}.png");
                weatherImg.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
