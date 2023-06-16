using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using WeatherApp.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Controllers
{
    public class WeatherWebApiClient
    {
        string Path { get; set; }
        string ApiKey { get; set; }
        string LocalizationID { get; set; }
        static HttpClient httpclient { get; set; }

        public WeatherWebApiClient(string path, string apikey, string localizationId)    //konstruktor przyjmujący adres serwera to odpytania
        {
            Path = path;
            ApiKey = apikey;
            LocalizationID = localizationId;
            httpclient = new HttpClient();
            httpclient.BaseAddress = new Uri(Path);
        }

        public WeatherWebApiClient()    //konstruktor nie przyjmujący argumentów, w ciele tej metody możemy zainicjalizować atrybuty
        {                               // np. z pliku ustawień aplikacji.

            httpclient = new HttpClient();
        }

        public async Task<WeatherModel> GetWeatherAsync()
        {
            WeatherModel weather = new WeatherModel();
            string weatherString = string.Empty;
            try
            {
                HttpResponseMessage response = await httpclient.GetAsync("?id=" + LocalizationID + "&APPID=" + ApiKey + "&units=metric");
                if (response.IsSuccessStatusCode)
                {
                    weatherString = await response.Content.ReadAsStringAsync(); //odczytujemy odpowiedź

                }

                if (weatherString != string.Empty)
                {
                    var parsedObject = JObject.Parse(weatherString);    //Tworzymy JObject
                    var menuJson = parsedObject["main"].ToString();     //Wycinamy kawałek odpowiedzialny za obiekt który nas intersuje
                    weather = JsonConvert.DeserializeObject<WeatherModel>(menuJson); //Deserializujemy nasz JSON
                }
                // return weatherString + "  "+response.StatusCode +" klucz:"+ ApiKey + httpclient.BaseAddress;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Błąd: " + e.Message);
            }

            return weather;
        }

        public async Task<List<HourlyModel>> GetWeatherHourlyAsync()
        {
            List<HourlyModel> weather = new List<HourlyModel>();
            string weatherString = string.Empty;
            try
            {
                HttpResponseMessage response = await httpclient.GetAsync("?id=" + LocalizationID + "&APPID=" + ApiKey + "&units=metric");
                if (response.IsSuccessStatusCode)
                {
                    weatherString = await response.Content.ReadAsStringAsync(); //odczytujemy odpowiedź

                }

                if (weatherString != string.Empty)
                {
                    var parsedObject = JObject.Parse(weatherString);    //Tworzymy JObject
                    String Miasto = parsedObject["city"]["name"].ToString();
                    List<String> Dates = new List<String>();
                    List<String> Temps = new List<String>();
                    foreach (var record in parsedObject["list"])
                    {
                        
                        Temps.Add(record["main"]["temp"].ToString());
                        Dates.Add(record["dt_txt"].ToString());

                        HourlyModel temp = new HourlyModel();
                        
                        temp.miasto = Miasto;
                        temp.temp = record["main"]["temp"].ToString();
                        temp.data = record["dt_txt"].ToString();

                        weather.Add(temp);
                    }

                    //weather = JsonConvert.DeserializeObject<List<HourlyModel>>(resultSequence); //Deserializujemy nasz JSON
                }
                // return weatherString + "  "+response.StatusCode +" klucz:"+ ApiKey + httpclient.BaseAddress;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Błąd: " + e.Message);
            }

            return weather;
        }

        public static String Time(String time)
        {

            double unixTimeStamp = Convert.ToDouble(time);
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            
            return dateTime.ToString("hh:mm tt");
        }

        public async Task<UserPanelModel> GetWeatherUserPanelAsync()
        {
            UserPanelModel weather = new UserPanelModel();
            string weatherString = string.Empty;
            try
            {
                HttpResponseMessage response = await httpclient.GetAsync("?id=" + LocalizationID + "&APPID=" + ApiKey + "&units=metric");
                if (response.IsSuccessStatusCode)
                {
                    weatherString = await response.Content.ReadAsStringAsync(); //odczytujemy odpowiedź
                }

                if (weatherString != string.Empty)
                {
                    var parsedObject = JObject.Parse(weatherString);    //Tworzymy JObject
                    weather.miasto = parsedObject["name"].ToString();
                    weather.temp = parsedObject["main"]["temp"].ToString();
                    weather.wind = parsedObject["wind"]["speed"].ToString();
                    weather.pressure = parsedObject["main"]["pressure"].ToString();
                    weather.humidity = parsedObject["main"]["humidity"].ToString();

                    weather.sunrise = Time(parsedObject["sys"]["sunrise"].ToString());
                    weather.sunset = Time(parsedObject["sys"]["sunset"].ToString());
                    weather.cloud_cover = parsedObject["clouds"]["all"].ToString();
                    weather.rain_perc = parsedObject["rain"]["1h"].ToString();
                    weather.description = parsedObject["weather"]["main"].ToString() + " - " + parsedObject["weather"]["description"].ToString();
                    

                    //weather = JsonConvert.DeserializeObject<List<HourlyModel>>(resultSequence); //Deserializujemy nasz JSON
                }
                // return weatherString + "  "+response.StatusCode +" klucz:"+ ApiKey + httpclient.BaseAddress;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Błąd: " + e.Message);
            }

            return weather;
        }

    }
}
