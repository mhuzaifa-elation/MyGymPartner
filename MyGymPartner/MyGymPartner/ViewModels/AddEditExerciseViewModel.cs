using Firebase.Database;
using Firebase.Database.Query;
using MyGymPartner.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System;
using System.IO;

namespace MyGymPartner.ViewModels
{

    public class AddEditExerciseViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        string ExerciseName = "";
        string Type = "";
        string Url = "";
        string Description = "";
        string ImageBase64Text = "";
        private ImageSource image = null;
        public ICommand SaveExerciseCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
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
        public AddEditExerciseViewModel(ExerciseModel selectedExercise)
        {
            if (selectedExercise != null)
            {
                this.Key = selectedExercise.Key ?? "";
                ExerciseNameB = selectedExercise.ExerciseName;
                TypeB = selectedExercise.Type;
                DescriptionB = selectedExercise.Description;
                URLB = selectedExercise.URL;
                if ((selectedExercise.ImageText ?? "").Length>0)
                {
                    ImageSourceB = ConvertBase64TexttoImage(selectedExercise.ImageText);
                }
            }
            SaveExerciseCommand = new Command(Save);
            SelectImageCommand = new Command(SelectImage);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

        }
        #endregion
        #region Methods 

        //To Save the Exercise in Firebase(both update and new )
        [Obsolete]
        public async void SelectImage()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions() { 
            Title="Please Select an Image."
            });
            if (result!=null)
            {
                var stream =await result.OpenReadAsync();
                ImageSourceB = ImageSource.FromStream(()=>stream);
            }
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                await ConvertImagetoBase64Text(stream);
            }
        }
        //To Save the Exercise in Firebase(both update and new )
        public async void Save()
        {
            try
            {
                
                var Exercise = new ExerciseModel() 
                { 
                    ExerciseName = ExerciseNameB, 
                    Type = TypeB, 
                    Description = DescriptionB,
                    URL=URLB,
                    ImageText= ImageBase64Text

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
                MessagingCenter.Send<string>(string.Empty, "Refresh");
                await Application.Current.MainPage.Navigation.PopAsync();


            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private ImageSource ConvertBase64TexttoImage(string Base64Text)
        {
            var bytes = Convert.FromBase64String(Base64Text);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        [Obsolete]
        private async Task ConvertImagetoBase64Text(Stream stream)
        {
            byte[] data;
            var ImgMemoryStream = new MemoryStream();
            stream.CopyTo(ImgMemoryStream);
            data = ImgMemoryStream.ToArray();
            ImageBase64Text = Convert.ToBase64String(data);
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
