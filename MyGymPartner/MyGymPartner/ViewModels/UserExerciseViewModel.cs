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
        public ICommand ShowNearbyGyms { get; set; }
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
            ShowNearbyGyms = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new NearbyGymPage()));
            ShowFavorites = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new FavoritesPage()));
            ShowCommand = new Command(ShowExercise);
            AddtoFavCommand = new Command(AddtoFavorites);
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
                await Application.Current.MainPage.Navigation.PushAsync(new DisplayExercise(SelectedExercise));
            }

        }
        private async void AddtoFavorites()//Adds new Exercise in Device Favorites
        {
            try
            {
                if (SelectedExercise != null)
                {
                    var AlreadySaved = Preferences.Get("MyFavExcercises", "");
                    List<FavExerciseModel> exercises;
                    if (AlreadySaved.Length == 0)
                    {
                        exercises = new List<FavExerciseModel>();

                    }
                    else
                    {
                        exercises = JsonConvert.DeserializeObject<List<FavExerciseModel>>(AlreadySaved);
                        if (exercises.Exists(x => x.Key == SelectedExercise.Key))
                        {
                            throw new Exception("Exercise is already Added to Favorites.");
                        }
                    }
                    exercises.Add(new FavExerciseModel()
                    {
                        Key = SelectedExercise.Key,
                        Description = SelectedExercise.Description,
                        ExerciseName = SelectedExercise.ExerciseName,
                        ImageText = SelectedExercise.ImageText,
                        Type = SelectedExercise.Type,
                        URL= SelectedExercise.URL,
                        Weight=0,
                        Reps=0

                    }); 
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
