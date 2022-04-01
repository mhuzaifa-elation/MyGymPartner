using MyGymPartner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyGymPartner.ViewModels
{

    public class AddEditExerciseViewModel : INotifyPropertyChanged
    {
        string ExerciseName = "";
        string Type = "";
        string Description = "";

        public string ExerciseNameB
        {
            get => ExerciseName;
            set
            {
                if (value == ExerciseName)
                {
                    return;
                }
                ExerciseName = value;
                OnPropertyChanged(nameof(ExerciseNameB));
            }
        }
        public string TypeB
        {
            get => Type;
            set
            {
                if (value == Type)
                {
                    return;
                }
                Type = value;
                OnPropertyChanged(nameof(TypeB));
            }
        }
        public string DescriptionB
        {
            get => Description;
            set
            {
                if (value == Description)
                {
                    return;
                }
                Description = value;
                OnPropertyChanged(nameof(DescriptionB));
            }
        }

        public AddEditExerciseViewModel(ExcerciseModel selectedExercise)
        {
            if (selectedExercise!=null)
            {
                ExerciseNameB = selectedExercise.TrainingName;
                TypeB = selectedExercise.Type;
                DescriptionB = selectedExercise.Description;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
