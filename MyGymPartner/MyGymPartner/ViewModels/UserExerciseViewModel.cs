using MyGymPartner.Models;
using MyGymPartner.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
    public class UserExerciseViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<ExerciseModel> _excercises;
        private ExerciseModel _selectedExercise;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand ShowFavorites { get; set; }
        public ICommand AddtoFavCommand { get; set; }
        public ICommand ShowCommand { get; set; }
        public ICommand BackCommand { get; set; }

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
        //Initializing commands and getting exercise from firebase
        public UserExerciseViewModel()
        {
            GetExercises();
            RefreshCommand = new Command(CmdRefresh);
            ShowFavorites = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new FavoritesPage()));
            AddtoFavCommand = new Command(AddtoFavorites);
            ShowCommand = new Command(ShowExercise);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 

        private async void GetExercises()//Gets latest exercises from firebase
        {
            List<ExerciseModel> AllExercises = await FirebaseServices.GetAllExercises();
            Exercises = AllExercises;

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
        private async void AddtoFavorites()//Adds new Exercise in Device Favorites
        {
            try
            {
                if (SelectedExercise != null)
                {
                    var AlreadySaved = Preferences.Get("MyFavExcercises", "");
                    List<ExerciseModel> exercises;
                    if (AlreadySaved.Length == 0)
                    {
                        exercises = new List<ExerciseModel>();

                    }
                    else
                    {
                        exercises = JsonConvert.DeserializeObject<List<ExerciseModel>>(AlreadySaved);
                        if (exercises.Exists(x => x.Key == SelectedExercise.Key))
                        {
                            throw new Exception("Exercise is already Added to Favorites.");
                        }
                    }
                    exercises.Add(SelectedExercise);
                    var serializedcontnet = JsonConvert.SerializeObject(exercises);
                    Preferences.Set("MyFavExcercises", serializedcontnet);
                    await Application.Current.MainPage.DisplayAlert("Information", "Exercise added to favorites successfully.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }

        }
        private void CmdRefresh()//Refreshes Page with Latest exercises
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
