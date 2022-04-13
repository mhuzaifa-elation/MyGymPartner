using MyGymPartner.Models;
using MyGymPartner.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<FavExerciseModel> _excercises;
        private FavExerciseModel _selectedExercise;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public ICommand EnterWorkoutCommand { get; set; }
        public ICommand ShowWorkoutCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public List<FavExerciseModel> Exercises
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


        public FavExerciseModel SelectedExercise
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

        public FavoritesViewModel()
        {
            GetExercises();
            RefreshCommand = new Command(CmdRefresh);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
            EnterWorkoutCommand = new Command(EnterWorkout);
            ShowWorkoutCommand = new Command(ShowWorkout);
            ShowCommand = new Command(ShowExercise);
            DeleteCommand = new Command(DeleteExercise);
        }
        #endregion
        #region Methods 
        private void GetExercises() //Gets Saved Favorites exercises in the Device
        {
            var AlreadySaved = Preferences.Get("MyFavExcercises", "");
            List<FavExerciseModel> AllExercises = new List<FavExerciseModel>();
            if (AlreadySaved.Length>0)
                AllExercises = JsonConvert.DeserializeObject<List<FavExerciseModel>>(AlreadySaved);
            Exercises = AllExercises;
        }
        private async void DeleteExercise() //Delete Selected Exercise in the Device
        {
            try
            {
                if (SelectedExercise != null)
                {

                    Exercises.RemoveAll(x => x.Key == SelectedExercise.Key);
                    var serializedcontnet = JsonConvert.SerializeObject(Exercises);
                    Preferences.Set("MyFavExcercises", serializedcontnet);
                    await Application.Current.MainPage.DisplayAlert("Information", "Exercise removed from favorites successfully.", "OK");
                    CmdRefresh();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        private async void EnterWorkout()
        {
            if (SelectedExercise != null)
            {
                string Weight = await Application.Current.MainPage.DisplayPromptAsync("Workout", "Enter Weight.", "OK", "Cancel", null, -1, Keyboard.Numeric)??"0";
                string Reps = await Application.Current.MainPage.DisplayPromptAsync("Workout", "Enter Reps.", "OK", "Cancel", null, -1, Keyboard.Numeric)??"0";
                foreach (var item in Exercises)
                {
                    if (item.Key == SelectedExercise.Key)
                    {
                        var newWorkout = new Workout()
                        {
                            Weight = Convert.ToInt32(Weight ?? "0"),
                            Reps = Convert.ToInt32(Reps ?? "0")
                        };
                        item.WorkoutDetails.Add(newWorkout);
                    }
                }

                var serializedcontnet = JsonConvert.SerializeObject(Exercises);
                Preferences.Set("MyFavExcercises", serializedcontnet);
            }

        }
        private async void ShowExercise() //Display Selected Exercise As A pop up Alert
        {
            if (SelectedExercise != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new DisplayExercise(SelectedExercise));
            }

        }
        private async void ShowWorkout() //Display Selected Exercise As A pop up Alert
        {
            if (SelectedExercise != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new DisplayWorkout(SelectedExercise));
            }

        }
        private void CmdRefresh() //Refreshes Page with Latest exercises
        {
            IsRefreshing = true;
            GetExercises();
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
