using System;
using System.Collections.Generic;
using System.Text;

namespace MyGymPartner.Models
{
   // Exercise Model to Save this as table in Firebase DB
    public class ExerciseModel
    {
        internal string ExerciseNameB;

        public string Key { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }
        public string ImageText { get; set; }
    }
    public class FavExerciseModel
    {
        public string Key { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }
        public string ImageText { get; set; }
        public List<Workout> WorkoutDetails { get; set; }
    }

    public class Workout
    {
        public int Weight { get; set; }
        public int Reps { get; set; }
    }
}
