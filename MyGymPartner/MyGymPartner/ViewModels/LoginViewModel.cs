using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using MyGymPartner.Views;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace MyGymPartner
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        public ICommand RegisterCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        string Username = "";
        string Password = "";
        public string PasswordB
        {
            get => Password;
            set
            {
                if (value == Password)
                {
                    return;
                }
                Password = value;
                OnPropertyChanged(nameof(PasswordB));
            }
        }
        public string UsernameB
        {
            get => Username;
            set
            {
                if (value == Username)
                {
                    return;
                }
                Username = value;
                OnPropertyChanged(nameof(UsernameB));
            }
        }
        #endregion
        #region Constructor
        //Initializing commands 
        public LoginViewModel()
        {
            LoginCommand = new Command(LoginClicked);
            ClearCommand = new Command(ClearClicked);
            RegisterCommand = new Command(async ()=> await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage()));
        }
        #endregion
        #region Methods 
        private  void ClearClicked() //Clears Credentials
        {
            UsernameB = string.Empty;
            PasswordB = string.Empty;
        }
        private async void LoginClicked()//Log the user using firebase Authentication
        {
            try
            {
                if (UsernameB.Length == 0)
                {
                    throw new Exception("Username Cannot be Empty");
                }
                if (PasswordB.Length == 0)
                {
                    throw new Exception("Password Cannot be Empty");
                }
                FirebaseAuthLink content = await FirebaseServices.Login(UsernameB, PasswordB);
                var serializedcontnet = JsonConvert.SerializeObject(content);
                ClearClicked();
                if (content.User.Email.ToUpper().Contains("ADMIN"))
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AdminExercisePage());
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new UserExercisePage());
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid Username/Password.\nTryAgain!", "OK");
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
