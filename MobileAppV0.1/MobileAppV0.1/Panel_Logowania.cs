﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Data;

namespace MobileAppV0._1
{
    [Activity(Label = "Panel logowania", MainLauncher = true, Icon = "@drawable/icon")]
    public class Panel_Logowania : Activity
    {
        private Button LoginButton;
        private EditText LoginField;
        private EditText PasswordField;
        private string connectionString = "data source=eadierp.database.windows.net;initial catalog=ERP;persist security info=True;user id=admin1;password=Ab123456;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        private DataSet dataSet;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Panel_Logowania);

            PasswordField = FindViewById<EditText>(Resource.Id.PasswordField);
            LoginField = FindViewById<EditText>(Resource.Id.LoginField);
            LoginButton = FindViewById<Button>(Resource.Id.LoginButton);
            //przypisanie kontrolek widoku do zmiennych


            LoginButton.Click += delegate //obsługa przycisku logowania
            {
                string UserName = LoginField.Text;
                string Password = PasswordField.Text;
                //pobranie danych logowania
                string HashPassword;
                dataSet = new DataSet(); 
                string selectLoginData = string.Format("SELECT Login, HashPassword FROM [dbo].[User] WHERE Login = '{0}'", UserName); //polecenie pobierające login i hasło
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))  //łączenie z bazą danych
                    {
                        connection.Open();
                        SqlCommand LoginCommand = new SqlCommand(selectLoginData, connection);
                        LoginCommand.Connection = connection;
                        var dataAdapter = new SqlDataAdapter { SelectCommand = LoginCommand };
                        dataAdapter.Fill(dataSet);

                        if(dataSet.Tables[0].Rows.Count==1) //sprawdzenie zgodności loginu
                        {
                            HashPassword = dataSet.Tables[0].Rows[0]["HashPassword"].ToString(); //wyłuskanie hasła z pobranych danych
                            if (VerifyHashedPassword(HashPassword, Password)) //sprawdzenie zgodności hasła
                            {
                                LoginData.CurrentLogin = UserName;
                                Intent intent = new Intent(this, typeof(Wyszukiwarka));
                                this.StartActivity(intent);
                                Finish();
                                //otworzenie okna wyszukiwarki
                            }
                            else
                            {
                                BadLogin();  //wyświetlenie komunikatu o błędnych danych logowania
                            }
                        }
                        else
                        {
                            BadLogin(); //wyświetlenie komunikatu o błędnych danych logowania
                        }                     
                    }
                }
                catch(SqlException exc)
                {
                    SqlError(exc.Message); //obsługa błędów komunikacji z bazą danych, np. brak połączenia z internetem
                }
            };
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)  //weryfikacja hasła użytkownika
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return AreHashesEqual(buffer3, buffer4);
        }

        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash) //porównanie haszów
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }

        void BadLogin() //funkcja komunikatu o błędnych danych logowania
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog error = builder.Create();
            error.SetTitle("Błąd!");
            error.SetMessage("Zły login lub hasło!");
            error.SetButton("OK", (s, ev) => { });
            error.Show();
        }

        void SqlError(string exception) //funkcja komunikatu o błędzie z połączeniem; pobiera parametr błędu SqlException i go wyświetla
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog error = builder.Create();
            error.SetTitle("Błąd!");
            error.SetMessage(exception);
            error.SetButton("OK", (s, ev) => { });
            error.Show();
        }
    }
}

