using MyGymPartner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
   public class DisplayExerciseViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        string ExerciseName = "";
        string Type = "";
        string Url = "";
        string Description = "";
        private ImageSource image = null;
        public ICommand URLTapCommand { get; set; }
        public string Key { get; set; } = string.Empty;
        public ImageSource ImageSourceB
        {
            get { return this.image; }
            set
            {
                this.image = value;
                this.OnPropertyChanged(nameof(ImageSourceB));
            }
        }
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
        public string URLB
        {
            get => Url;
            set
            {
                if (value == Url)
                {
                    return;
                }
                Url = value;
                OnPropertyChanged(nameof(URLB));
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
        public DisplayExerciseViewModel(ExerciseModel selectedExercise)
        {
            if (selectedExercise != null)
            {
                this.Key = selectedExercise.Key ?? "";
                ExerciseNameB = selectedExercise.ExerciseName;
                TypeB = selectedExercise.Type;
                DescriptionB = selectedExercise.Description;
                URLB = selectedExercise.URL;
                if ((selectedExercise.ImageText ?? "").Length > 0)
                {
                    ImageSourceB = ConvertBase64TexttoImage(selectedExercise.ImageText);
                }
            }
            URLTapCommand = new Command(OpenHyperLink);
           

        }

        private async void OpenHyperLink(object obj)
        {
            try
            {
                await Launcher.OpenAsync(URLB);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        #endregion
        #region Methods 
        private ImageSource ConvertBase64TexttoImage(string Base64Text)
        {
            var bytes = Convert.FromBase64String(Base64Text);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
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
