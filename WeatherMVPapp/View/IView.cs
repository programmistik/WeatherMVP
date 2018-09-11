using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherMVPapp.Model;

namespace WeatherMVPapp.View
{
    public interface IView
    {
        void UpdateList(IEnumerable<string> CityList);
        void AddWeatherToList(FullWeather full_weather);
        void UpdateText(CurrentWeather outPut);
        void updateForecast(List<ForecastList> l, DateTime onDate);
        void emptyAll();
        event Action<string> AddCity;
        event Action<string> DelCity;
        event Action<string> Import;
        event Action<string, IEnumerable<object>> Export;
        event Action<string> getFull;
        event Action<string> DownloadIcon;
    }
}
