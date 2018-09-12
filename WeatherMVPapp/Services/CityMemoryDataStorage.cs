using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMVPapp.Services
{
    public class CityMemoryDataStorage : ICityDataStorage
    {
        //--------------------------------------------------------------------------------------------------------
        public IEnumerable<object> CityList { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------
        public IEnumerable<object> GetCityList() => CityList;
        //--------------------------------------------------------------------------------------------------------
        public void SetCityList(IEnumerable<object> citylist) => CityList = new List<object>(citylist);
        //--------------------------------------------------------------------------------------------------------
        public void Add(string city)
        {
            if (!String.IsNullOrWhiteSpace(city))
                if (!CityList.Contains(city))
                {
                    var cl = CityList.ToList();
                    cl.Add(city);
                    CityList = cl;
                }
        }
        //--------------------------------------------------------------------------------------------------------
        public void Remove(string city)
        {
            var cl = CityList.ToList();
            cl.Remove(city);
            CityList = cl;
        }
        //--------------------------------------------------------------------------------------------------------
    }
}
