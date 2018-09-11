using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMVPapp.Model
{
    //--------------------------------------------------------------------------------------------------------
    public class sys
    {
        public string country { get; set; }
        public double sunrise { get; set; } // Sunrise time, unix, UTC
        public double sunset { get; set; }  // Sunset time, unix, UTC
    }
    //--------------------------------------------------------------------------------------------------------
    public class CurrentWeather
    {
        public coord coord { get; set; }
        public string name { get; set; }
        public sys sys { get; set; }
        public double dt { get; set; }
        public wind wind { get; set; }
        public main main { get; set; }
        public List<weather> weather { get; set; }
        public int cod { get; set; }
    }
    //--------------------------------------------------------------------------------------------------------
    public class coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
    //--------------------------------------------------------------------------------------------------------
    public class FullWeather : IModel
    {
        public string City { get; set; }
        public CurrentWeather CurrWeather { get; set; }
        public weatherForcast Forcast { get; set; }
    }
    //--------------------------------------------------------------------------------------------------------
}

