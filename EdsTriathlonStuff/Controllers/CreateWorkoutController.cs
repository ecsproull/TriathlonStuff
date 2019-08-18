using EdsTriathlonStuff.App_Code;
using EdsTriathlonStuff.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EdsTriathlonStuff.Controllers
{
    public class CreateWorkoutController : Controller
    {
        enum WorkoutSection  {WarmUp, Main, CoolDown};
        static SwimCreate model = new SwimCreate(5, 10, 5);
        private SwimDataContext context;
        public CreateWorkoutController(SwimDataContext ctx)
        {
            context = ctx;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult Calculate(SwimCreate sc)
        {
            bool yards = true;
            string[] poolinfo = sc.PoolLength.Split(" ");
            int poolLength = Convert.ToInt32(poolinfo[0]);
            if (poolinfo[1] == "Meters")
            {
                yards = false;
            }
            const string endline = "\r\n";
            sc.Calculated = string.Empty;
            if (yards)
            {
                sc.Calculated = "Total Yards " + sc.TotalYards + endline;
            }
            else
            {
                sc.Calculated = "Total Meters " + sc.TotalMeters + endline;
            }
            sc.Calculated += "Warm Up" + endline;
            sc.Calculated += CreateSets(sc, poolLength, endline, WorkoutSection.WarmUp) + endline;
            sc.Calculated += "Main Set" + endline;
            sc.Calculated += CreateSets(sc, poolLength, endline, WorkoutSection.Main) + endline;
            sc.Calculated += "Cool Down" + endline;
            sc.Calculated += CreateSets(sc, poolLength, endline, WorkoutSection.CoolDown);
            sc.SwimSpeedSelectList = sc.GetSelectListItems();
            model = sc;
            return RedirectToAction("Index");
        }

        private static string CreateSets(SwimCreate sc, int poolLength, string endline, WorkoutSection section)
        {
            string retString = string.Empty;
            int beginStep = 0, endStep = 0;
            switch(section)
            {
                case WorkoutSection.WarmUp:
                    beginStep = sc.WarmUpBeginSteps;
                    endStep = sc.WarmUpEndSteps;
                    break;
                case WorkoutSection.Main:
                    beginStep = sc.MainBeginSteps;
                    endStep = sc.MainEndSteps;
                    break;
                case WorkoutSection.CoolDown:
                    beginStep = sc.CoolDownBeginSteps;
                    endStep = sc.CoolDownEndSteps;
                    break;
            }

            for (int i = beginStep; i < endStep; i++)
            {
                if (sc.SwimSets[i].RepCount > 0 && sc.SwimSets[i].LengthCount > 0)
                {
                    string format = string.Empty;
                    if (sc.SwimSets[i].RepCount > 1)
                    {
                        format = "{0} x {1} @{2} {3}s {4}";

                    }
                    else
                    {
                        format = "{1} @{2} {3}s {4}";
                    }

                    retString += string.Format(format,
                            sc.SwimSets[i].RepCount,
                            sc.SwimSets[i].LengthCount * poolLength,
                            sc.SwimSets[i].Speed,
                            sc.SwimSets[i].Rest,
                            sc.SwimSets[i].Comment) + endline;
                }
            }

            return retString;
        }
    }
}
