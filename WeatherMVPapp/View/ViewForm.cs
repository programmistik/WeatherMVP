using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WeatherMVPapp.Model;

namespace WeatherMVPapp.View
{
    public partial class ViewForm : Form, IView
    {
        //--------------------------------------------------------------------------------------------------------
        public List<FullWeather> FullWeatherList { get; set; } = new List<FullWeather>();
       
        public event Action<string> AddCity;
        public event Action<string> DelCity;
        public event Action<string> Import;
        public event Action<string, IEnumerable<object>> Export;
        public event Action<string> getFull;
        public event Action<string> DownloadIcon;

        //--------------------------------------------------------------------------------------------------------
        public ViewForm()
        {
            InitializeComponent();
            ActiveControl = textBoxCity;
        }
        //--------------------------------------------------------------------------------------------------------
        private void button_AddCity_Click(object sender, EventArgs e)
        {
            
            try
            {
                getFull?.Invoke(textBoxCity.Text);
                AddCity?.Invoke(textBoxCity.Text);
                listBox_City.Focus();
                listBox_City.SelectedIndex = listBox_City.FindString(textBoxCity.Text);
                
            }
            catch (WebException exc)
            {
                MessageBox.Show(exc.Message, "Error! City not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBoxCity.Text = "";
        }
        //--------------------------------------------------------------------------------------------------------
        public void updateForecast(List<ForecastList> l, DateTime onDate)
        {
            var city = listBox_City.SelectedItem.ToString();
            TempChart.Series.Clear();
            TempChart.Titles.Clear();
            TempChart.Titles.Add($"Temperature in {city} on {onDate.ToShortDateString()}");

            Series series = TempChart.Series.Add("Temperature");

            series.IsVisibleInLegend = false;
            series.ChartType = SeriesChartType.Spline;
            TempChart.Series["Temperature"].BorderWidth = 3;

            foreach (var item in l)
            {
                series.Points.AddXY(item.dt_txt.Hour.ToString() + ":00", item.main.temp);
            }

            setIconPicture(pictureBox1, l[0].weather[0].icon);
            setIconPicture(pictureBox2, l[1].weather[0].icon);
            setIconPicture(pictureBox3, l[2].weather[0].icon);
            setIconPicture(pictureBox4, l[3].weather[0].icon);
            setIconPicture(pictureBox5, l[4].weather[0].icon);
            setIconPicture(pictureBox6, l[5].weather[0].icon);
            setIconPicture(pictureBox7, l[6].weather[0].icon);
            setIconPicture(pictureBox8, l[7].weather[0].icon);

            label1.Text = Math.Truncate(l[0].main.temp).ToString() + " \u00B0C";
            label2.Text = Math.Truncate(l[1].main.temp).ToString() + " \u00B0C";
            label3.Text = Math.Truncate(l[2].main.temp).ToString() + " \u00B0C";
            label4.Text = Math.Truncate(l[3].main.temp).ToString() + " \u00B0C";
            label5.Text = Math.Truncate(l[4].main.temp).ToString() + " \u00B0C";
            label6.Text = Math.Truncate(l[5].main.temp).ToString() + " \u00B0C";
            label7.Text = Math.Truncate(l[6].main.temp).ToString() + " \u00B0C";
            label8.Text = Math.Truncate(l[7].main.temp).ToString() + " \u00B0C";
            label9.Text = "Days:";

            linkLabel1.Visible = true;
            linkLabel2.Visible = true;
            linkLabel3.Visible = true;
            linkLabel4.Visible = true;

            linkLabel1.Text = DateTime.Now.AddDays(1).ToShortDateString();
            linkLabel2.Text = DateTime.Now.AddDays(2).ToShortDateString();
            linkLabel3.Text = DateTime.Now.AddDays(3).ToShortDateString();
            linkLabel4.Text = DateTime.Now.AddDays(4).ToShortDateString();
        }
        //--------------------------------------------------------------------------------------------------------
        public void UpdateList(IEnumerable<string> CityList)
        {
            listBox_City.DataSource = null;
            var cl = CityList.ToList();
            cl.Sort();
            listBox_City.DataSource = cl;
        }
        //--------------------------------------------------------------------------------------------------------
        public void AddWeatherToList(FullWeather full_weather)
        {
            if (FullWeatherList.Where(c => c.City == full_weather.City).Count() == 0)
            //    MessageBox.Show($"{full_weather.City} is already exist.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //else
                FullWeatherList.Add(full_weather);

            textBoxCity.Text = full_weather.City;
        }
        //--------------------------------------------------------------------------------------------------------
        public void setIconPicture(PictureBox pictureBox, string iconPath)
        {
            if (File.Exists($"icons\\{iconPath}.png"))
                pictureBox.Load($"icons\\{iconPath}.png");
            else
            {
                DownloadIcon?.Invoke(iconPath);
                pictureBox.Load($"icons\\{iconPath}.png");
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void UpdateText(CurrentWeather CurrWeather)
        {
            cityName.Text = CurrWeather.name;
            countryName.Text = CurrWeather.sys.country;
            Temp_Main.Text = CurrWeather.main.temp.ToString()+" \u00B0C";
            description.Text = CurrWeather.weather[0].description;
            sunrise.Text = "Sunrise (UTC): " + getDate(CurrWeather.sys.sunrise).ToShortTimeString();
            sunset.Text =  "Sunset  (UTC): " + getDate(CurrWeather.sys.sunset).ToShortTimeString();
            wind.Text = "Wind: "+CurrWeather.wind.speed.ToString()+ " m/sec, "+degToCompass(CurrWeather.wind.deg);

            setIconPicture(picture_Main, CurrWeather.weather[0].icon);
        }
        //--------------------------------------------------------------------------------------------------------
        private void textBoxCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button_AddCity.PerformClick();
        }
        //--------------------------------------------------------------------------------------------------------
        private void listBox_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_City.SelectedIndex != -1 && FullWeatherList.Count > 0)
            {
                var city = listBox_City.SelectedItem.ToString();
                var weather = FullWeatherList.Single(c => c.City == city);
                UpdateText(weather.CurrWeather);
                var tt = weather.Forcast.list.Where(d => d.dt_txt.Day == DateTime.Now.Day + 1).ToList();
                updateForecast(tt, DateTime.Now.AddDays(1));
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<string> CityList = new List<string>();
            foreach (var item in listBox_City.Items)
            {
                CityList.Add(item.ToString());
            }
            Export?.Invoke("CityList.json", CityList);
            Export?.Invoke("LastCondition", FullWeatherList);
        }
        //--------------------------------------------------------------------------------------------------------
        private void getForecast(int days)
        {
            var city = listBox_City.SelectedItem.ToString();

            var tt = FullWeatherList.Single(c => c.City == city).Forcast.list.Where(d => d.dt_txt.Day == DateTime.Now.Day + days).ToList();
            updateForecast(tt, DateTime.Now.AddDays(days));
        }
        //--------------------------------------------------------------------------------------------------------
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => getForecast(1);
        //--------------------------------------------------------------------------------------------------------
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => getForecast(2);
        //--------------------------------------------------------------------------------------------------------
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => getForecast(3);
        //--------------------------------------------------------------------------------------------------------
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => getForecast(4);
        //--------------------------------------------------------------------------------------------------------
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var selectedCity = listBox_City.SelectedItem as string;
            if (selectedCity == null)
                MessageBox.Show("Please, select city!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (listBox_City.SelectedIndex == -1)
                {
                    if (listBox_City.Items.Count == 0)
                    {
                        emptyAll();
                    }
                    else
                    {
                        
                        listBox_City.SelectedIndex = 0;
                    }

                }
                else
                    DelCity?.Invoke(selectedCity);
            }
        }
        //--------------------------------------------------------------------------------------------------------
        public void emptyAll()
        {
            cityName.Text = "";
            countryName.Text = "";
            Temp_Main.Text = "";
            description.Text = "";
            sunrise.Text = "";
            sunset.Text = "";
            wind.Text = "";
            picture_Main.Image = null;

            TempChart.Series.Clear();
            TempChart.Titles.Clear();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
            pictureBox7.Image = null;
            pictureBox8.Image = null;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";

            linkLabel1.Visible = false;
            linkLabel2.Visible = false;
            linkLabel3.Visible = false;
            linkLabel4.Visible = false;
        }
        //--------------------------------------------------------------------------------------------------------
        private void ViewForm_Load(object sender, EventArgs e)
        {
            emptyAll();

            Import?.Invoke("CityList.json");
            string city;

            foreach (var item in listBox_City.Items)
            {
                city = item.ToString();
                getFull?.Invoke(city);
            }

            if (FullWeatherList.Count > 0)
            {
                UpdateText(FullWeatherList[0].CurrWeather);
                var tt = FullWeatherList[0].Forcast.list.Where(d => d.dt_txt.Day == DateTime.Now.Day + 1).ToList();
                updateForecast(tt, DateTime.Now.AddDays(1));
            }
            textBoxCity.Text = "";
        }
        //--------------------------------------------------------------------------------------------------------
        DateTime getDate(double millisecound)
        {
            DateTime day = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            day = day.AddSeconds(millisecound);

            return day;
        }
        //--------------------------------------------------------------------------------------------------------
        string degToCompass(double deg)
        {
            deg *= 10d;

            string[] Compass = { "North", "North-northeast", "Northeast", "East-Northeast",
                                "East", "East-Southeast", "Southeast", "South-southeast", "South",
                                "South-Southwest", "Southwest", "West-Southwest", "West", "West-Northwest",
                                "Northwest", "North-northwest", "North" };
            return Compass[(int)Math.Round((deg % 3600) / 225)];
        }
        //--------------------------------------------------------------------------------------------------------

    }
}
