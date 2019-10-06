using EdsTriathlonStuff.App_Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EdsTriathlonStuff.Models
{
    public class SwimCreate
    {
        public SwimCreate() { }
        public SwimCreate(int warmupSteps, int mainSteps, int cooldownSteps, List<Workout> workouts)
        {
            Calculated = "Press Calculate";
            WorkoutId = -1;
            Workouts = workouts;
            PoolLength = "25 Yards";
            WarmUpBeginSteps = 0;
            WarmUpEndSteps = warmupSteps;
            MainBeginSteps = WarmUpEndSteps;
            MainEndSteps = MainBeginSteps + mainSteps;
            CoolDownBeginSteps = MainEndSteps;
            CoolDownEndSteps = CoolDownBeginSteps + cooldownSteps;
            int totalSets = warmupSteps + mainSteps + cooldownSteps;
            SwimSets = new List<SwimSet>();
            for (int i = 0; i < totalSets; i++)
            {
                SwimSets.Add(new SwimSet
                {
                    Speed = "Easy",
                    RepCount = 0,
                    Rest = 30,
                    Total = 0,
                    Comment = string.Empty
                });
            }
            SwimSpeedSelectList = GetPaceSelectListItems();
            SetDistanceSelectList = GetSetDistanceSelectList();

            UserLoggedIn = false;
            UserName = "Log In";
            LoginButtonClass = "loggedout-button";
        }

        public IEnumerable<SelectListItem> SwimSpeedSelectList { get; set; }
        public IEnumerable<SelectListItem> SetDistanceSelectList { get; set; }
        public string PoolLength { get; set; }
        public string WorkoutName { get; set; }
        public string WorkoutDescription { get; set; }
        public string Calculated { get; set; }
        public int WarmUpBeginSteps { get; set; }
        public int WarmUpEndSteps { get; set; }
        public int MainBeginSteps { get; set; }
        public int MainEndSteps { get; set; }
        public int CoolDownBeginSteps { get; set; }
        public int CoolDownEndSteps { get; set; }
        public int TotalYards { get; set; }
        public int TotalMeters { get; set; }
        public List<SwimSet> SwimSets { get; set; }
        public string CoachName { get; set; }
        public string AthleteName { get; set; }
        public List<Workout> Workouts { get; set; }
        public int WorkoutId { get; set; }

        public IEnumerable<SelectListItem> GetSetDistanceSelectList()
        {
            int[] stdSetLengths = { 1, 2, 3, 4, 5, 6, 8, 12, 16, 20, 32, 40};
            int poolLength = Convert.ToInt32(PoolLength.Split(" ")[0]);
            var selectList = new List<SelectListItem>();
            foreach (int i in stdSetLengths)
            {
                string value = (i * poolLength).ToString();
                selectList.Add(new SelectListItem
                {
                    Value = value,
                    Text = value
                });
            }

            selectList.Add(new SelectListItem
            {
                Value = "other",
                Text = "Other"
            });

            return selectList;
        }

        public IEnumerable<SelectListItem> GetPaceSelectListItems()
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in SwimSpeeds)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        private List<string> SwimSpeeds = new List<string>
        {
            "Easy",
            "Marathon",
            "Threshold",
            "Fast",
            "All Out",
            "Descend",
            "Ascend"
        };

        public bool UserLoggedIn { get; set; }
        public string UserName { get; set; }
        [Required]
        public string LoginButtonClass { get; set; }
        public string LoginOutUrl
        {
            get
            {
                if (LoginButtonClass == "loggedin-button")
                {
                    return "/Account/Logout";
                }
                else
                {
                    return "/Account/Login?returnUrl=/Home/Index";
                }
            }
        }
    }
}
