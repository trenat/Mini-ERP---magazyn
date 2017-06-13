using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;


namespace MobileAppV0._1
{
    [Activity(Label = "Panel logowania", MainLauncher = true, Icon = "@drawable/icon")]
    public class Panel_Logowania : Activity
    {
        private Button LoginButton;
        private EditText LoginField;
        private EditText PasswordField;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Panel_Logowania);

            PasswordField = FindViewById<EditText>(Resource.Id.PasswordField);
            LoginField = FindViewById<EditText>(Resource.Id.LoginField);
            LoginButton = FindViewById<Button>(Resource.Id.LoginButton);

            LoginButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(Wyszukiwarka));
                this.StartActivity(intent);
                Finish();
            };
        }
    }
}

