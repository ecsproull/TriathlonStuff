using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EdsTriathlonStuff.App_Code
{
    public static class SwimCalculate
    {
        enum WorkOutSection { WarmUp, MainSet, CoolDown };
        public static IEnumerable<string> CalcWorkoutDistance(List<string> workoutSteps, bool metric)
        {
            const string regMultiSet = @"^[0-9]+\ ?x\ ?[0-9]+";
            const string regVariableSet = @"^[0-9]-[0-9]\ x\ [0-9]+";
            const string regBS = @"^[0-9]\ x\ [a-zA-Z]";
            const string regInitalNum = @"^[0-9]+";
            const string regMultiRounds = @"^[0-9]+ Rounds";

            int multipleRounds = 1;
            int warmUp = 0;
            int mainSet = 0;
            int coolDown = 0;
            WorkOutSection currentSection = WorkOutSection.WarmUp;

            foreach (string step in workoutSteps)
            {
                switch (step)
                {
                    case "Warm Up":
                        currentSection = WorkOutSection.WarmUp;
                        multipleRounds = 1;
                        break;
                    case "Main Set":
                        currentSection = WorkOutSection.MainSet;
                        multipleRounds = 1;
                        break;
                    case "Cool Down":
                        currentSection = WorkOutSection.CoolDown;
                        multipleRounds = 1;
                        break;
                    case "":
                        multipleRounds = 1;
                        break;
                    default:
                        break;
                }

                if (Regex.IsMatch(step, regMultiRounds, RegexOptions.Multiline))
                {
                    Match repeat = Regex.Match(step, regInitalNum, RegexOptions.Multiline);
                    multipleRounds = Convert.ToInt32(repeat.Value);
                    continue;
                }

                int stepLength = 0;
                // check for 123 x 123 format.
                if (Regex.IsMatch(step, regMultiSet))
                {
                    MatchCollection mc = Regex.Matches(step, @"[0-9]+");
                    stepLength = Convert.ToInt32(mc[0].Value) * Convert.ToInt32(mc[1].Value);
                }
                else if (Regex.IsMatch(step, regBS))
                {
                    // skip bs like sink down.
                }
                else if (Regex.IsMatch(step, regVariableSet, RegexOptions.Multiline))
                {
                    MatchCollection mc = Regex.Matches(step, @"[0-9]+", RegexOptions.Multiline);
                    stepLength = Convert.ToInt32(mc[1].Value) * Convert.ToInt32(mc[2].Value);
                }
                else if (Regex.IsMatch(step, regInitalNum, RegexOptions.Multiline))
                {
                    Match m = Regex.Match(step, regInitalNum);
                    stepLength = Convert.ToInt32(m.Value);
                }

                stepLength *= multipleRounds;

                switch (currentSection)
                {
                    case WorkOutSection.WarmUp:
                        warmUp += stepLength;
                        break;
                    case WorkOutSection.MainSet:
                        mainSet += stepLength;
                        break;
                    case WorkOutSection.CoolDown:
                        coolDown += stepLength;
                        break;
                }
            }

            string units = metric ? " meters" : " yards";
            List<string> formattedWorkout = new List<string>();
            foreach (string step in workoutSteps)
            {
                switch (step)
                {
                    case "Warm Up":
                        formattedWorkout.Add(step + " " + warmUp + units);
                        break;
                    case "Main Set":
                        formattedWorkout.Add(" ");
                        formattedWorkout.Add(step + " " + mainSet + units);
                        break;
                    case "Cool Down":
                        formattedWorkout.Add(" ");
                        formattedWorkout.Add(step + " " + coolDown + units);
                        break;
                    default:
                        formattedWorkout.Add(step);
                        break;
                }
            }

            return formattedWorkout;
        }
    }
}
