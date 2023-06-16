using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Areas.Identity.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApp.Controllers
{
    public class UserPanelController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            UserPanelModel model =  Database_controller.ListUserPanel("Warsaw");

            List<HourlyModel> mod = Database_controller.ListHourly("Warsaw");


            ViewData["T"] = model.temp;
            ViewData["W"] = model.wind;
            ViewData["P"] = model.pressure;
            ViewData["H"] = model.humidity;
            ViewData["sunrise"] = model.sunrise;
            ViewData["sunset"] = model.sunset;
            ViewData["cloud"] = model.cloud_cover;
            ViewData["rain"] = model.rain_perc;
            ViewData["des"] = model.description;

            ViewData["miasto"]  =model.miasto;

            ViewData["today"] = mod[1].temp;
            ViewData["tom"] = mod[9].temp;
            ViewData["atom"] = mod[17].temp;

            ViewData["czas"] = DateTime.Now.ToString("hh:mm tt");
            ViewData["data"] = DateTime.Now.ToString("dd/MM/yyyy");
            return View();
        }
    }
}

