using MyGymPartner.Models;
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
        private List<ExerciseModel> _excercises;
        private ExerciseModel _selectedExercise;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public List<ExerciseModel> Exercises
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


        public ExerciseModel SelectedExercise
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
            ShowCommand = new Command(ShowExercise);
            DeleteCommand = new Command(DeleteExercise);
        }
        #endregion
        #region Methods 
        private void GetExercises() //Gets Saved Favorites exercises in the Device
        {
            var AlreadySaved = Preferences.Get("MyFavExcercises", "");
            List<ExerciseModel> AllExercises = new List<ExerciseModel>();
            if (AlreadySaved.Length>0)
                AllExercises = JsonConvert.DeserializeObject<List<ExerciseModel>>(AlreadySaved);
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
        private async void ShowExercise() //Display Selected Exercise As A pop up Alert
        {
            if (SelectedExercise != null)
            {
                string ExerciseText = string.Format("Exercise Name : {0}\n\nType :{1}\n\nDescripion : {2}",
                    SelectedExercise.ExerciseName, SelectedExercise.Type, SelectedExercise.Description);
                await Application.Current.MainPage.DisplayAlert("Details", ExerciseText, "OK");
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
