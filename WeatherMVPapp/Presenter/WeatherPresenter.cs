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
        IView View;
        IModel Model;
        

        public WeatherPresenter(IModel Model, IView View)
        {
            this.Model = Model;
            this.View = View;

            View.AddCity += AddCity;
            View.DelCity += DeleteCity;
            View.Import += ImportCityList;
            View.Export += ExportCityList;
            View.getFull += getFullWeatherByCity;
            View.DownloadIcon += DownloadIcon;
        }
        public ICityDataStorage Storage { get; set; } = new CityMemoryDataStorage();
        public IFileSaver Saver { get; set; } = new JSON_Save();
        public WeatherAPI wAPI { get; set; } = new WeatherAPI();

        //--------------------------------------------------------------------------------------------------------
        public void ImportCityList(string filename)
        {
            Storage.SetCityList(Saver.Load(filename));
            View.UpdateList(Storage.GetCityList());
        }
        //--------------------------------------------------------------------------------------------------------
        public void ExportCityList(string filename, IEnumerable<object> list) => Saver.Save(list,filename);// Storage.GetCityList(), filename);
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
        public void getFullWeatherByCity(string city)
        {
            CurrentWeather CurrWeather;
            try
            {
                CurrWeather = wAPI.getWeather(city);
            }
            catch (WebException exc)
            {
                throw exc;
            }
            var Forcast = wAPI.getForcast(city);

            View.AddWeatherToList(new FullWeather
            {
                City = Forcast.city.name,
                CurrWeather = CurrWeather,
                Forcast = Forcast
            });
        }
        //--------------------------------------------------------------------------------------------------------
        public void DownloadIcon(string iconID)
        {
            wAPI.DownloadIcon(iconID);
        }
    }
}
