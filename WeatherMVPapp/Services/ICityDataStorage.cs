﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMVPapp.Services
{
    public interface ICityDataStorage
    {
        void Add(string city);
        void Remove(string city);
        IEnumerable<string> GetCityList();
        void SetCityList(IEnumerable<string> CityList);
    }
}
