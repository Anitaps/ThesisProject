using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace LatestApp
{
    [Activity(Label = "UserLogin", MainLauncher =true)]
    public class UserLogin : Activity
    {
        private Button Login;
        private EditText username, password;
        private static string User;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SecondPage);
            // Create your application here

            username = FindViewById<EditText>(Resource.Id.username);
           password = FindViewById<EditText>(Resource.Id.password);
           Login = FindViewById<Button>(Resource.Id.Login1);
           Login.Click += Login_Click;


            }

          private void Login_Click (Object sender , EventArgs e){
            if ((username.Text.Trim().Length < 8)||(password.Text.Trim().Length < 9))
            {
                new AlertDialog.Builder(this)
                .SetMessage("Enter Username and Password: ")
                .SetTitle("Invalid Input ")
                .Show();

            }
            else 
                {
                    User = username.Text;
                var MainActivity = new Intent(this, typeof(MainActivity));
                MainActivity.PutExtra("clientId", User);
                    StartActivity(MainActivity);
                }
               

          }

    }
}