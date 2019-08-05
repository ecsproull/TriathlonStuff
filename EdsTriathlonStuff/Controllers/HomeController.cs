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
            Swim.Zones = "Easy 59:59 - 02:05 100/y 00:16 00:00 Smooth 02:04 - 02:00 100/y 00:20 00:00 Moderate 01:59 - 01:55 100/y 00:12 00:00 Threshold 01:54 - 01:47 100/y 00:12 00:00 Fast 01:46 - 01:39 100/y 00:00 00:00 All Out 01:38 - 00:00 100/y 00:00 00:00";
            return View(Swim);
        }

        [HttpPost]
        public IActionResult Calculate (SwimCalc sc)
        {
            Swim.Calculated = string.Empty;
            Swim.SwimWorkout = sc.SwimWorkout;
            if (!string.IsNullOrEmpty(sc.SwimWorkout))
            {
                StringReader sr = new StringReader(sc.SwimWorkout);
                List<string> wo = new List<string>();
                while (true)
                {
                    string s = sr.ReadLine();
                    if (s != null)
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
            }

            Swim.Zones = sc.Zones;
            if (!string.IsNullOrEmpty(sc.Zones))
            {
                Swim.Calculated += "\r\n" + ZoneCalculate.FormatZones(sc.Zones, sc.Metric);
            }

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
