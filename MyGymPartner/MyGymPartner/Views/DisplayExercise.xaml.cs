using MyGymPartner.Models;
using MyGymPartner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGymPartner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayExercise : ContentPage
    {
        public DisplayExercise(ExerciseModel selectedExercise)
        {
            InitializeComponent();
            BindingContext = new DisplayExerciseViewModel(selectedExercise);
        }
        public DisplayExercise(FavExerciseModel selectedExercise)
        {
            InitializeComponent();
            BindingContext = new DisplayExerciseViewModel(selectedExercise);
        }
    }
}