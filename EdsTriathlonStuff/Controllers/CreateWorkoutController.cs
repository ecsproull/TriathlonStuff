using EdsTriathlonStuff.App_Code;
using EdsTriathlonStuff.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EdsTriathlonStuff.Controllers
{
    public enum WorkoutSection { WarmUp, Main, CoolDown };
    [Authorize(Roles = "Coach, Athlete")]
    public class CreateWorkoutController : Controller
    {
        static SwimCreate model = null; // = new SwimCreate(5, 10, 5, new List<Workout>());
        private SwimDataContext context;
        private UserManager<AppUser> userManager;
        private static string UserId = string.Empty;
        public CreateWorkoutController(SwimDataContext ctx, UserManager<AppUser> usrMgr)
        {
            context = ctx;
            userManager = usrMgr;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var tempUserId = UserId;
            var current_User = userManager.GetUserAsync(HttpContext.User).Result;
            UserId = current_User.Id;

            if (model == null || tempUserId != UserId)
            {
                model = new SwimCreate(5, 10, 5, GetUsersWorkouts());

                model.CoachName = User.Identity.Name;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(SwimCreate sc)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                var current_User = userManager.GetUserAsync(HttpContext.User).Result;
                UserId = current_User.Id;
            }

            Workout wo = (from x in context.Workouts
                          where x.UserId == UserId && x.Name == sc.WorkoutName
                          select x).SingleOrDefault();// FirstOrDefault();

            //wo = context.Workouts.SingleOrDefault(b => b.UserId == UserId && b.Name == sc.WorkoutName);

            if (wo == null)
            {
                wo= new Workout
                {
                    AthleteName = sc.AthleteName,
                    CoolDownStepCount = sc.CoolDownEndSteps - sc.CoolDownBeginSteps,
                    WarmUpStepCount = sc.WarmUpEndSteps - sc.WarmUpBeginSteps,
                    MainStepCount = sc.MainEndSteps - sc.MainBeginSteps,
                    Description = sc.WorkoutDescription,
                    Name = sc.WorkoutName,
                    UserId = UserId,
                    TotalYards = sc.TotalYards,
                    TotalMeters = sc.TotalMeters
                };

                context.Workouts.Add(wo);

                //Save changes to get a valid wo.Id.
                context.SaveChanges();
            }
            else
            {
                wo.AthleteName = sc.AthleteName;
                wo.CoolDownStepCount = sc.CoolDownEndSteps - sc.CoolDownBeginSteps;
                wo.WarmUpStepCount = sc.WarmUpEndSteps - sc.WarmUpBeginSteps;
                wo.MainStepCount = sc.MainEndSteps - sc.MainBeginSteps;
                wo.Description = sc.WorkoutDescription;
                wo.Name = sc.WorkoutName;
                wo.UserId = UserId;
                wo.TotalYards = sc.TotalYards;
                wo.TotalMeters = sc.TotalMeters;
            }

            for (int i = 0; i < sc.SwimSets.Count; i++)
            {
                if (sc.SwimSets[i].RepCount > 0)
                {
                    WorkoutSection wos = WorkoutSection.CoolDown;
                    if (sc.SwimSets[i].StepNumber < wo.WarmUpStepCount)
                    {
                        wos = WorkoutSection.WarmUp;
                    }
                    else if (sc.SwimSets[i].StepNumber < (wo.MainStepCount + wo.WarmUpStepCount))
                    {
                        wos = WorkoutSection.Main;
                    }

                    SwimSet ss = (from x in context.SwimSets
                     where x.WorkoutId  == wo.Id && x.StepNumber == sc.SwimSets[i].StepNumber
                               select x).FirstOrDefault();

                    SwimSet ssNew = new SwimSet
                    {
                         Comment = sc.SwimSets[i].Comment,
                         Distance = sc.SwimSets[i].Distance,
                         RepCount = sc.SwimSets[i].RepCount,
                         Speed = sc.SwimSets[i].Speed,
                         Rest = sc.SwimSets[i].Rest,
                         StepNumber = i,
                         Total = sc.SwimSets[i].Total,
                         WorkoutId = wo.Id,
                         Group = wos
                    };

                    if (ss != null)
                    {
                        ss = ssNew;
                    }
                    else
                    {
                        context.SwimSets.Add(ssNew);
                    }
                }
            }

            context.SaveChanges();

            sc.SwimSpeedSelectList = sc.GetPaceSelectListItems();
            sc.SetDistanceSelectList = sc.GetSetDistanceSelectList();
            sc.Workouts = GetUsersWorkouts();
            sc.WorkoutId = wo.Id;
            model = sc;
            return RedirectToAction("Index");
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
            sc.SwimSpeedSelectList = sc.GetPaceSelectListItems();
            sc.SetDistanceSelectList = sc.GetSetDistanceSelectList();
            sc.Workouts = GetUsersWorkouts();
            model = sc;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LoadWorkout(int id)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                var current_User = userManager.GetUserAsync(HttpContext.User).Result;
                UserId = current_User.Id;
            }

            Workout workout = (from x in context.Workouts
                               where x.UserId == UserId && x.Id == id
                               select x).FirstOrDefault();

            model = new SwimCreate(workout.WarmUpStepCount, 
                workout.MainStepCount, 
                workout.CoolDownStepCount,
                (from x in context.Workouts
                 where x.UserId == UserId
                 select x).ToList());
            model.UserName = User.Identity.Name;
            model.TotalMeters = workout.TotalMeters;
            model.TotalYards = workout.TotalYards;
            model.WorkoutDescription = workout.Description;
            model.AthleteName = workout.AthleteName;
            model.WorkoutName = workout.Name;
            model.CoachName = User.Identity.Name;

            IEnumerable<SwimSet> sets = (from y in context.SwimSets
                                         where y.WorkoutId == workout.Id
                                         select y);
            foreach (SwimSet ss in sets)
            {
                model.SwimSets[ss.StepNumber] = ss;
            }

            model.Workouts = GetUsersWorkouts();
            model.WorkoutId = workout.Id;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string DeleteWorkout(Workout wo)
        {
            Workout woToDelete = (from x in context.Workouts
                                  where x.UserId == UserId && x.Id == wo.Id
                                  select x).FirstOrDefault();
            if (woToDelete != null)
            {
                context.Workouts.Remove(woToDelete);
                context.SaveChanges();
                model.Workouts = GetUsersWorkouts();
                return "success";
            }

            return "failed";
        }

        [HttpPost]
        public string RenameWorkout(Workout wo)
        {
            Workout woToRename = (from x in context.Workouts
                                  where x.UserId == UserId && x.Id == wo.Id
                                  select x).FirstOrDefault();
            if (woToRename != null)
            {
                woToRename.Name = wo.Name;
                context.SaveChanges();
                model.Workouts = GetUsersWorkouts();
                return "success";
            }

            return "failed";
        }

        private List<Workout> GetUsersWorkouts()
        {
            return (from x in context.Workouts
             where x.UserId == UserId
             select x).ToList<Workout>().ToList();
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
                if (sc.SwimSets[i].RepCount > 0)
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
                            sc.SwimSets[i].Distance,
                            sc.SwimSets[i].Speed[0],
                            sc.SwimSets[i].Rest,
                            sc.SwimSets[i].Comment) + endline;
                }
            }

            return retString;
        }
    }
}
