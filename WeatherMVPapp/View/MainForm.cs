using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WeatherMVPapp.Model;
using WeatherMVPapp.Presenter;

namespace WeatherMVPapp.View
{
    public partial class MainForm : Form
    {
        //--------------------------------------------------------------------------------------------------------
        public WeatherPresenter Presenter { get; set; }
        public List<string> CityList { get; set; }
        //--------------------------------------------------------------------------------------------------------
        public MainForm(WeatherPresenter presenter)
        {
            InitializeComponent();
            emptyAll();
            Presenter = presenter;
            ActiveControl = textBoxCity;
        }
        //--------------------------------------------------------------------------------------------------------
        private void button_AddCity_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.getWeather(textBoxCity.Text);
                Presenter.AddCity(cityName.Text);
                listBox_City.Focus();
                listBox_City.SelectedIndex = listBox_City.FindString(cityName.Text);
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
            
            pictureBox1.Image = Presenter.setIcon(l[0].weather[0].icon);
            pictureBox2.Image = Presenter.setIcon(l[1].weather[0].icon);
            pictureBox3.Image = Presenter.setIcon(l[2].weather[0].icon);
            pictureBox4.Image = Presenter.setIcon(l[3].weather[0].icon);
            pictureBox5.Image = Presenter.setIcon(l[4].weather[0].icon);
            pictureBox6.Image = Presenter.setIcon(l[5].weather[0].icon);
            pictureBox7.Image = Presenter.setIcon(l[6].weather[0].icon);
            pictureBox8.Image = Presenter.setIcon(l[7].weather[0].icon);
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
        public void UpdateList(List<string> CityList)
        {
            listBox_City.DataSource = null;
            CityList.Sort();
            listBox_City.DataSource = CityList;
        }
        //--------------------------------------------------------------------------------------------------------
        public void UpdateText(CurrentWeather outPut)
        {
            cityName.Text = outPut.name;
            countryName.Text = outPut.sys.country;
            Temp_Main.Text = outPut.main.temp.ToString()+" \u00B0C";

            picture_Main.Image = Presenter.setIcon(outPut.weather[0].icon);
        }
        //--------------------------------------------------------------------------------------------------------
        private void textBoxCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button_AddCity.PerformClick();
        }
        //--------------------------------------------------------------------------------------------------------
        private void listBox_City_DoubleClick(object sender, EventArgs e)
        {
            var city = listBox_City.SelectedItem.ToString();
            Presenter.getWeather(city);
            Presenter.getForcast(city, 1);
        }
        //--------------------------------------------------------------------------------------------------------
        private void listBox_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_City.SelectedIndex != -1)
            {
                var city = listBox_City.SelectedItem.ToString();
                Presenter.getWeather(city);
                Presenter.getForcast(city, 1);
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) => Presenter.ExportCityList("CityList.json");
        //--------------------------------------------------------------------------------------------------------
        private void getForecast(int days)
        {
            var city = listBox_City.SelectedItem.ToString();
            Presenter.getForcast(city, days);
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
                Presenter.DeleteCity(selectedCity);
                if (listBox_City.SelectedIndex == -1)
                {
                    if (listBox_City.Items.Count == 0)
                    {
                        emptyAll();
                    }
                    else
                        listBox_City.SelectedIndex = 0;
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------
        private void emptyAll()
        {
            cityName.Text = "";
            countryName.Text = "";
            Temp_Main.Text = "";
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
    }
}
