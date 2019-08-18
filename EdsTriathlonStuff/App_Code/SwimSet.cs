using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EdsTriathlonStuff.App_Code
{
    public class SwimSet
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int StepNumber { get; set; }
        public int RepCount { get; set; }
        public int LengthCount { get; set; }
        public int Rest { get; set; }
        public string Speed { get; set; }
        public string Comment { get; set; }

        [NotMapped]
        public int Distance { get; set; }

        [NotMapped]
        public int Total { get; set; }
    }
}
