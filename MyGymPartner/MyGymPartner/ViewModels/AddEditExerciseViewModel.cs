using Firebase.Database;
using Firebase.Database.Query;
using MyGymPartner.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{

    public class AddEditExerciseViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        string ExerciseName = "";
        string Type = "";
        string Description = "";
        public ICommand SaveExerciseCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public string Key { get; set; } = string.Empty;

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
        #endregion
        #region Constructor
        //Initializing commands and checking if its an edit or new exercise
        public AddEditExerciseViewModel(ExerciseModel selectedExercise)
        {
            if (selectedExercise != null)
            {
                this.Key = selectedExercise.Key ?? "";
                ExerciseNameB = selectedExercise.ExerciseName;
                TypeB = selectedExercise.Type;
                DescriptionB = selectedExercise.Description;
            }
            SaveExerciseCommand = new Command(Save);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

        }
        #endregion
        #region Methods 
       
        //To Save the Exercise in Firebase(both update and new )
        public async void Save()
        {
            try
            {
                var Exercise = new ExerciseModel() 
                { 
                    ExerciseName = ExerciseNameB, 
                    Type = TypeB, 
                    Description = DescriptionB 
                };
                if (Key.Length > 0)
                {
                    await FirebaseServices.UpdateExercise(Key,Exercise);

                }
                else
                {
                    await FirebaseServices.AddExercise(Exercise);
                }
                await Application.Current.MainPage.DisplayAlert("Information", "Exercise Saved Sucessfully.", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();


            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
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
