using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherMVPapp.View;
//using WeatherMVPapp.Presenter;
//using System.Globalization;
//using System.Threading;

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

            var Model = new Model.FullWeather();
            var View = new ViewForm();
            var Presenter = new Presenter.WeatherPresenter(Model, View);
            Application.Run(View);

        }
    }
}
