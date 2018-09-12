using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherMVPapp.Model;

namespace WeatherMVPapp.Services
{
    class JSON_Save : IFileSaver
    {
        //--------------------------------------------------------------------------------------------------------
        public IEnumerable<object> Load(string filename)
        {
            IEnumerable<object> list;

            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                list = JsonConvert.DeserializeObject<IEnumerable<object>>(json);
            }
            else
            {
                list = new List<string>();
            }
            return list;
        }
        //--------------------------------------------------------------------------------------------------------
        public void Save(IEnumerable<object> CityList, string filename)
        {
            var json = JsonConvert.SerializeObject(CityList);
            File.WriteAllText(filename, json);
        }
        //--------------------------------------------------------------------------------------------------------
    }
}
