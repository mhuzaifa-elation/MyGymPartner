using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using MyGymPartner.Views;
using Acr.UserDialogs;

namespace MyGymPartner
{
    public class LoginViewModel : INotifyPropertyChanged
    {


        public ICommand LoginC { get; set; }
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
        public LoginViewModel()
        {
            LoginC = new Command(LoginClicked);
            UsernameB = "ADMIN";
            PasswordB = "123";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        private void LoginClicked()
        {
            try
            {
                if (Username.Length == 0)
                {
                    throw new Exception("Username Cannot be Empty");
                }
                if (Password.Length == 0)
                {
                    throw new Exception("Password Cannot be Empty");
                }
                if (Username == "ADMIN" && Password == "123")
                {
                    Application.Current.MainPage.Navigation.PushAsync(new AdminExercisePage());
                }
                else
                {
                    throw new Exception("Invalid Username/Password.\nTryAgain!");
                }
            }
            catch (Exception ex)
            {
                 Application.Current.MainPage.DisplayAlert("Error", ex.Message,"OK");
            }
            

        }
    }
}
