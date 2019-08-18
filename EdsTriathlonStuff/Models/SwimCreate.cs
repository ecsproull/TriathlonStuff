using EdsTriathlonStuff.App_Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdsTriathlonStuff.Models
{
    public class SwimCreate
    {
        public SwimCreate() { }
        public SwimCreate(int warmupSteps, int mainSteps, int cooldownSteps)
        {
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
                    Speed = "Fast",
                    RepCount = 0,
                    LengthCount = 0,
                    Distance = 0,
                    Total = 0,
                    Comment = string.Empty
                });
            }
            SwimSpeedSelectList = GetSelectListItems();
        }

        public IEnumerable<SelectListItem> SwimSpeedSelectList { get; set; }
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


        public IEnumerable<SelectListItem> GetSelectListItems()
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
            "Endurance",
            "Threshold",
            "Fast",
            "All Out",
            "Descend",
            "Ascend"
        };
    }
}
