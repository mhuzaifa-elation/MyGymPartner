using Acr.UserDialogs;
using MyGymPartner.Models;
using MyGymPartner.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
    public class AdminExcerciseViewModel : INotifyPropertyChanged
    {
        private List<ExcerciseModel> _excercises;
        private ExcerciseModel _selectedExercise;
        private bool _isRefreshing;
        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public List<ExcerciseModel> Exercises
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

        
        public ExcerciseModel SelectedExercise
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

        

        public AdminExcerciseViewModel()
        {
            var data = new List<ExcerciseModel>();


            for (int i = 1; i <= 15; i++)
            {
                var person = new ExcerciseModel()
                {

                    TrainingName = "Execise" + i,
                    Description = "Desc" + i,
                    Type = i.ToString()
                };
                data.Add(person);

            }
            Exercises = data;
            RefreshCommand = new Command(CmdRefresh);
            AddCommand = new Command(AddExercise);
            EditCommand = new Command(EditExercise);
        }

        private void EditExercise()
        {
            try
            {
                if (SelectedExercise != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new AddEditExercisePage(SelectedExercise));
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message,  "OK");

            }
        }
        private void AddExercise()
        {
            try
            {
             Application.Current.MainPage.Navigation.PushAsync(new AddEditExercisePage());

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message,  "OK");

            }
        }

        private async void CmdRefresh()
        {
            IsRefreshing = true;
            await Task.Delay(3000);
            IsRefreshing = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
       

    }
}
