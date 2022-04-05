using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyGymPartner.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        public ICommand BackCommand { get; set; }
        public ICommand SignupCommand { get; set; }
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
        public RegisterViewModel()
        {
            SignupCommand = new Command(SignupClicked);
            BackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }
        #endregion
        #region Methods 
        private async void SignupClicked()//Log the user using firebase Authentication
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
                if (!IsValidEmail(UsernameB))
                {
                    throw new Exception("Please Enter Valid Email ");

                }
                if (PasswordB.Length < 6)
                {
                    throw new Exception("Password Cannot be less than 6 characters");
                }
                FirebaseAuthLink content = await FirebaseServices.Signup(UsernameB, PasswordB);
                await Application.Current.MainPage.DisplayAlert("Information", "User/Admin Added Successfully", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
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
