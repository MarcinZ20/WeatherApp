using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Areas.Identity.Data;
using System.Text.Encodings.Web;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApp.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {

        // GET: /<controller>/
        public IActionResult Index(string city = "Warsaw")
        {
            UserPanelModel model = Database_controller.ListUserPanel(city);
            List<HourlyModel> mod = Database_controller.ListHourly(city);

            ViewData["T"] = ParseTemperature(model.temp);
            ViewData["W"] = model.wind;
            ViewData["P"] = model.pressure;
            ViewData["H"] = model.humidity;
            ViewData["sunrise"] = model.sunrise;
            ViewData["sunset"] = model.sunset;
            ViewData["cloud"] = model.cloud_cover;
            ViewData["rain"] = model.rain_perc;
            ViewData["des"] = model.description;

            ViewData["miasto"] = model.miasto;

            ViewData["today"] = ParseTemperature(mod[1].temp);
            ViewData["tom"] = ParseTemperature(mod[9].temp);
            ViewData["atom"] = ParseTemperature(mod[17].temp);

            ViewData["czas"] = DateTime.Now.ToString("hh:mm tt");
            ViewData["data"] = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }

        // GET: /UserPanel/city
        [HttpPost]
        public string City(string city)
        {

            return $"This is for {city}";
        }

        private static string ParseTemperature(string temp)
        {
            return Convert.ToInt64(double.Parse(temp)).ToString();
        }
    }
}

