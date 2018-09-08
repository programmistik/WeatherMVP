using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherMVPapp.Model;
using WeatherMVPapp.Services;
using WeatherMVPapp.View;

namespace WeatherMVPapp.Presenter
{
    public class WeatherPresenter
    {
        //--------------------------------------------------------------------------------------------------------
        public MainForm View { get; set; }
        public ICityDataStorage Storage { get; set; } = new CityMemoryDataStorage();
        public IFileSaver Saver { get; set; } = new JSON_Save();
        //--------------------------------------------------------------------------------------------------------
        private const string APPID = "babc914c7d24672584419e75857edcd7";
        //--------------------------------------------------------------------------------------------------------
        public void getWeather(string city)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={APPID}&units=metric&cnt=1";
                string json = "";

                try
                {
                    json = web.DownloadString(url);
                }
                catch(WebException ex)
                {
                    throw ex;                   
                }

                var result = JsonConvert.DeserializeObject<CurrentWeather>(json);
                View.UpdateText(result);
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void getForcast(string city, int days)
        {
            string url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&APPID={APPID}";
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(url);
                var forcast = JsonConvert.DeserializeObject<weatherForcast>(json);

                var tt = forcast.list.Where(d => d.dt_txt.Day == DateTime.Now.Day + days).ToList();

                View.updateForecast(tt, DateTime.Now.AddDays(days));
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public Image setIcon(string iconID)
        {
            string url = $"http://openweathermap.org/img/w/{iconID}.png"; 
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            using (var weatherIcon = response.GetResponseStream())
            {
                var weatherImg = Image.FromStream(weatherIcon);
                return weatherImg;
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void ImportCityList(string filename)
        {
            Storage.SetCityList(Saver.Load(filename));
            View.UpdateList(Storage.GetCityList());
        }
        //--------------------------------------------------------------------------------------------------------
        public void ExportCityList(string filename) => Saver.Save(Storage.GetCityList(), filename);
        //--------------------------------------------------------------------------------------------------------
        public void AddCity(string city)
        {
            Storage.Add(city);
            View.UpdateList(Storage.GetCityList());
        }
        //--------------------------------------------------------------------------------------------------------
        public void DeleteCity(string city)
        {
            Storage.Remove(city);
            View.UpdateList(Storage.GetCityList());
        }
        //--------------------------------------------------------------------------------------------------------
    }
}
