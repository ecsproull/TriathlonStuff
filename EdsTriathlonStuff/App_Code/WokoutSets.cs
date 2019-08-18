using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdsTriathlonStuff.App_Code
{
    public class WokoutSet
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int SetId { get; set; }
        public string WorkoutSection { get; set; }
        public int Index { get; set; }
    }
}
