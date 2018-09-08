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
        public List<string> CityList { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------
        public List<string> GetCityList() => CityList;
        //--------------------------------------------------------------------------------------------------------
        public void SetCityList(List<string> citylist) => CityList = new List<string>(citylist);
        //--------------------------------------------------------------------------------------------------------
        public void Add(string city)
        {
            if (!String.IsNullOrWhiteSpace(city))
            {
                CityList.Add(city);
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void Remove(string city)=> CityList.Remove(city);
        //--------------------------------------------------------------------------------------------------------
    }
}
