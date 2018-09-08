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
    }
    //--------------------------------------------------------------------------------------------------------
    public class CurrentWeather
    {
        public string name { get; set; }
        public sys sys { get; set; }
        public double dt { get; set; }
        public wind wind { get; set; }
        public main main { get; set; }
        public List<weather> weather { get; set; }
        public int cod { get; set; }
    }
    //--------------------------------------------------------------------------------------------------------
}

