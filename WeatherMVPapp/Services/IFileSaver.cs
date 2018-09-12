using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherMVPapp.Model;

namespace WeatherMVPapp.Services
{
    public interface IFileSaver
    {
        void Save(IEnumerable<object> CityList, string filename);
        IEnumerable<object> Load(string filename);
    }
}
