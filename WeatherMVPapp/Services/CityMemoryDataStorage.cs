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
        public IEnumerable<string> CityList { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------
        public IEnumerable<string> GetCityList() => CityList;
        //--------------------------------------------------------------------------------------------------------
        public void SetCityList(IEnumerable<string> citylist) => CityList = new List<string>(citylist);
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
