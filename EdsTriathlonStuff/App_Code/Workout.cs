using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdsTriathlonStuff.App_Code
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string AthleteName { get; set; }
        public string Description { get; set; }
        public int WarmUpStepCount { get; set; }
        public int MainStepCount { get; set; }
        public int CoolDownStepCount { get; set; }
        public int TotalYards { get; set; }
        public int TotalMeters { get;set; }
    }
}
