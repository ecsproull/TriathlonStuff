using EdsTriathlonStuff.Controllers;

namespace EdsTriathlonStuff.App_Code
{
    public class SwimSet
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int StepNumber { get; set; }
        public int RepCount { get; set; }
        public int Rest { get; set; }
        public string Speed { get; set; }
        public string Comment { get; set; }
        public string Distance { get; set; }
        public int Total { get; set; }
        public WorkoutSection Group { get; set; }
    }
}
