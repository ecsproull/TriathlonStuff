using EdsTriathlonStuff.App_Code;
using EdsTriathlonStuff.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EdsTriathlonStuff.Controllers
{
    public class HomeController : Controller
    {
        static SwimCalc Swim = new SwimCalc();
        public IActionResult Index()
        {
            return View(Swim);
        }

        [HttpPost]
        public IActionResult Calculate (SwimCalc sc)
        {
            Swim.SwimWorkout = sc.SwimWorkout;
            StringReader sr = new StringReader(sc.SwimWorkout);
            List<string> wo = new List<string>();
            while (true)
            {
                string s = sr.ReadLine();
                if (!string.IsNullOrEmpty(s))
                {
                    wo.Add(s);
                }
                else
                {
                    break;
                }
            }

            IEnumerable<string> calculatedWorkout = SwimCalculate.CalcWorkoutDistance(wo, sc.Metric);
            string calculated = string.Empty;
            foreach (string woLine in calculatedWorkout)
            {
                calculated += woLine + "\r\n";
            }

            Swim.SwimWorkout = sc.SwimWorkout;
            Swim.Calculated = calculated;
            Swim.Calculated += "\r\n" + ZoneCalculate.FormatZones(sc.Zones, sc.Metric);
            Swim.Zones = sc.Zones;
            Swim.Metric = sc.Metric;
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
