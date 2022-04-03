using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using MyGymPartner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGymPartner
{
   //Firebase Services is custom class for using firebase operations
    public static class FirebaseServices
    {
        #region Class Variables
        static FirebaseClient fc = new FirebaseClient(Utils.RealtimeDbURL);
        #endregion
        #region Methods
        public static async Task<FirebaseAuthLink> Login(string Username,string Password) //Login Method for firebase Authentication
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Utils.WebAPIKey));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(Username, Password);
            var content = await auth.GetFreshAuthAsync();
            return content;
        }
        public static async Task<List<ExerciseModel>> GetAllExercises() //Method for getting All Exercises in firebase Realtime DB
        {
            FirebaseClient fc = new FirebaseClient(Utils.RealtimeDbURL);
            var AllExercises = (await fc
              .Child("Exercises")
              .OnceAsync<ExerciseModel>()).Select(item => new ExerciseModel
              {
                  Key = item.Key,
                  ExerciseName = item.Object.ExerciseName,
                  Description = item.Object.Description,
                  Type = item.Object.Type
              }).ToList();
            return AllExercises;
        }
        public static async Task UpdateExercise(string Key, ExerciseModel Exercise) //Method for Updating existing Exercise in firebase Realtime DB
        {
            await fc
            .Child("Exercises").Child(Key)
            .PutAsync(Exercise);
        }
        public static async Task AddExercise(ExerciseModel Exercise) //Method for adding new Exercise in firebase Realtime DB
        {
             await fc
                  .Child("Exercises")
                  .PostAsync(Exercise);
        }
        public static async Task DeleteExercise(string Key) //Method for Deleting existing Exercise in firebase Realtime DB
        {
            await fc
                 .Child("Exercises").Child(Key).DeleteAsync();
        }
        #endregion
    }
}
