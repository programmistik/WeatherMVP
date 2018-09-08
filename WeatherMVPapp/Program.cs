using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherMVPapp.View;
using WeatherMVPapp.Presenter;
using System.Globalization;
using System.Threading;

namespace WeatherMVPapp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");
            var WeatherPresenter = new WeatherPresenter();
            var MainForm = new WeatherMVPapp.View.MainForm(WeatherPresenter);
            WeatherPresenter.View = MainForm;

            WeatherPresenter.ImportCityList("CityList.json");

            Application.Run(MainForm);
        }
    }
}
