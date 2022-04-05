using Acr.UserDialogs;
using Firebase.Database;
using MyGymPartner.Models;
using MyGymPartner.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
    public class AdminExcerciseViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        private List<ExerciseModel> _excercises;
        private ExerciseModel _selectedExercise;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
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
        //Initializing commands and getting exercise from firebase
        public AdminExcerciseViewModel()
        {
            GetExercises();
            RefreshCommand = new Command(CmdRefresh);
            AddCommand = new Command(AddExercise);
            EditCommand = new Command(EditExercise);
            DeleteCommand = new Command(DeleteExercise);
        }
        #endregion
        #region Methods 

        private async void GetExercises() //Gets latest exercises from firebase
        {
            List<ExerciseModel> AllExercises = await FirebaseServices.GetAllExercises();
            Exercises = AllExercises;
        }
        private async void DeleteExercise() //Delete Selected Exercise from firebase
        {
            try
            {
                if (SelectedExercise != null)
                {
                    await FirebaseServices.DeleteExercise(SelectedExercise.Key);
                    CmdRefresh();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message,  "OK");

            }
        }
        private void EditExercise() //Update Existing Exercise from firebase
        {
            try
            {
                if (SelectedExercise != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new AddEditExercisePage(SelectedExercise));
                    MessagingCenter.Subscribe<string>(this, "Refresh", (v) => { CmdRefresh(); });
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message,  "OK");

            }
        }
        private void AddExercise() //Adds new Exercise in firebase
        {
            try
            {
                Application.Current.MainPage.Navigation.PushAsync(new AddEditExercisePage());
                MessagingCenter.Subscribe<string>(this, "Refresh", (v) => { CmdRefresh(); });
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message,  "OK");

            }
        }

        private  void CmdRefresh() //Refreshes Page with Latest exercises
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
