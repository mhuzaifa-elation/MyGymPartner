﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyGymPartner.Models
{
   // Exercise Model to Save this as table in Firebase DB
    public class ExerciseModel
    {
        public string Key { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
