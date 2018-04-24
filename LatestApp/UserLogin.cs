using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace LatestApp
{
[Activity(Label = "UserLogin", MainLauncher = true)]
public class UserLogin : Activity
{
    private Button Login;
    private Button CreateAccount;
    private TextView databasetxt;
    private EditText username, password;
    public static string User;
    private static SqlConnectionStringBuilder databaseAddress;
    private static SqlConnection connection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SecondPage);
            // Create your application here

            databaseAddress = createAddress();
            connection = new SqlConnection(databaseAddress.ConnectionString);
            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            Login = FindViewById<Button>(Resource.Id.Login1);
            Login.Click += Login_Click;
            TextView databasetxt = FindViewById<TextView>(Resource.Id.databasetxt);

            Button CreateAccount = FindViewById<Button>(Resource.Id.accountbtn);

            CreateAccount.Click += delegate
            {
                if ((username.Text.Trim().Length < 8) || (password.Text.Trim().Length < 9))
                {
                    Toast.MakeText(this, "Invalid input! Username and Password should be at least 8 characters.", ToastLength.Long).Show();
                }
                else
                {
                    CreateAccount_Click();
                    
                }


            };
        }

    private void Login_Click(Object sender, EventArgs e) {
            if ((username.Text.Trim().Length >= 8) && (password.Text.Trim().Length >= 8))
            {
                //{
                //    new AlertDialog.Builder(this)
                //    .SetMessage("Enter Username and Password: ")
                //    .SetTitle("Invalid Input ")
                //    .Show();

                //}
                StringBuilder query = new StringBuilder();
                query.Append("SELECT COUNT (*) FROM Registrations WHERE Username like @username AND Password like @password ");
                string sqlquery = query.ToString();
                connection.Open();
                SqlCommand command = new SqlCommand(sqlquery, connection);
                command.Parameters.AddWithValue("@username", username.Text);
                command.Parameters.AddWithValue("@password", password.Text);
                int result = (int)command.ExecuteScalar();
                connection.Close();
                if (result == 0)
                {
                    Toast.MakeText(this, "No such user!", ToastLength.Long).Show();
                }
                else
                {
                    User = username.Text;
                    var MainActivity = new Intent(this, typeof(MainActivity));
                    MainActivity.PutExtra("clientId", User);
                    StartActivity(MainActivity);
                }
            }
            else
            
                {
                    Toast.MakeText(this, "Invalid input! Username and Password should be at least 8 characters.", ToastLength.Long).Show();
                }
            

    }

    private void CreateAccount_Click()
    {
           StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT (*) FROM Registrations WHERE Username like @username ");
            string sqlquery = query.ToString();
            connection.Open();
            SqlCommand command = new SqlCommand(sqlquery, connection);
            command.Parameters.AddWithValue("@username", username.Text);
            int result = (int)command.ExecuteScalar();
            connection.Close();
                if (result > 0)
                {
                    Toast.MakeText(this, "Username already exists!", ToastLength.Long).Show();
                 }
                else
                {
                    StringBuilder sqlq = new StringBuilder();
                    sqlq.Append("INSERT INTO Registrations(Username, Password)VALUES('" + username.Text + "', '" + password.Text + "')");
                    string sqlquery1 = sqlq.ToString();
                    connection.Open();
                    SqlCommand sqlcommand = new SqlCommand(sqlquery1, connection);
                    sqlcommand.ExecuteNonQuery();
                    connection.Close();
                    Toast.MakeText(this, "User added!", ToastLength.Long).Show();
                }
        }

       
    
        
        private SqlConnectionStringBuilder createAddress()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
          
  
            return builder;
        }

    }
}
    

