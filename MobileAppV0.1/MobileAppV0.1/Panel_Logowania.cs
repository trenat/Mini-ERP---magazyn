using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Data.SqlClient;

namespace MobileAppV0._1
{
    [Activity(Label = "Panel logowania", MainLauncher = true, Icon = "@drawable/icon")]
    public class Panel_Logowania : Activity
    {
        private Button LoginButton;
        private EditText LoginField;
        private EditText PasswordField;
        private TextView ErrorConnectionTextView;
        private string UserName;
        private string Password;
        private string connectionString = "data source=eadierp.database.windows.net;initial catalog=ERP;persist security info=True;user id=admin1;password=Ab123456;MultipleActiveResultSets=True;App=EntityFramework&quot;";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Panel_Logowania);

            PasswordField = FindViewById<EditText>(Resource.Id.PasswordField);
            LoginField = FindViewById<EditText>(Resource.Id.LoginField);
            LoginButton = FindViewById<Button>(Resource.Id.LoginButton);
            ErrorConnectionTextView = FindViewById<TextView>(Resource.Id.ErrorConnectionTextView);

            LoginButton.Click += delegate
            {
                UserName = LoginField.Text;
                string selectUserName = string.Format("SELECT Login FROM [dbo].[User] WHERE Login = '{0}'", UserName);
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(selectUserName, connection);
                        command.Connection = connection;
                        var result = command.ExecuteReader();
                        
                        if(result.HasRows)
                        {
                            ErrorConnectionTextView.Text = UserName;
                        }
                        else
                        {
                            ErrorConnectionTextView.Text = "Bad login or password";
                        }
                    }

                }
                catch(SqlException exc)
                {
                    ErrorConnectionTextView.Text = exc.Message;
                }
                /*
                Intent intent = new Intent(this, typeof(Wyszukiwarka));
                this.StartActivity(intent);
                Finish();
                */
            };
        }
    }
}

