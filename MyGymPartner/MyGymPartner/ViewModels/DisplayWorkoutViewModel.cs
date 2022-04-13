using MyGymPartner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
   public class DisplayWorkoutViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<Workout> _excercises;
        private Workout _selectedExercise;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public List<Workout> Exercises
        {
            get
            {
                return _excercises;
            }
            set
            {
                _excercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }


        public Workout SelectedExercise
        {
            get
            {
                return _selectedExercise;
            }
            set
            {
                _selectedExercise = value;
                OnPropertyChanged(nameof(SelectedExercise));
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        #endregion
        #region Constructor
        //Initializing commands and getting Saved Favorite Exercises

        public DisplayWorkoutViewModel(FavExerciseModel favExercise)
        {
            Exercises = favExercise.WorkoutDetails;
            RefreshCommand = new Command(CmdRefresh);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 
        private void CmdRefresh() //Refreshes Page with Latest exercises
        {
            IsRefreshing = true;
            IsRefreshing = false;
        }
        #endregion
        #region  INotifyPropertyChanged Methods
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion

    }
}
