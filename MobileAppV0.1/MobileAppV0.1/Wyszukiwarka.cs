using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Data;

namespace MobileAppV0._1
{
    [Activity(Label = "Wyszukiwarka")]
    public class Wyszukiwarka : Activity
    {
        private Button FindButton;
        private ListView ItemList;
        private List<string> Items;
        private Button LogoutButton;
        private EditText FindField;
        private Button QRButton;
        private DataSet dataSet;
        private string connectionString = "data source=eadierp.database.windows.net;initial catalog=ERP;persist security info=True;user id=admin1;password=Ab123456;MultipleActiveResultSets=True;App=EntityFramework&quot;";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Wyszukiwarka);

            FindButton = FindViewById<Button>(Resource.Id.FindButton);
            ItemList = FindViewById<ListView>(Resource.Id.ItemList);
            LogoutButton = FindViewById<Button>(Resource.Id.LogoutButton);
            FindField = FindViewById<EditText>(Resource.Id.FindField);
            QRButton = FindViewById<Button>(Resource.Id.QRButton);
            
            LogoutButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(Panel_Logowania));
                this.StartActivity(intent);
                Finish();
            };

            FindButton.Click += delegate
            {
                string ItemFinder = FindField.Text;
                dataSet = new DataSet();
                string selectItems = string.Format("SELECT Name, Description, Price FROM [dbo].[Item] WHERE Name LIKE '{0}%'", ItemFinder);
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand ItemListCommand = new SqlCommand(selectItems, connection);
                        ItemListCommand.Connection = connection;
                        var dataAdapter = new SqlDataAdapter { SelectCommand = ItemListCommand };
                        dataAdapter.Fill(dataSet);

                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            Items = new List<string>();
                            foreach (DataRow item in dataSet.Tables[0].Rows)
                            {
                                string Item = string.Join("", item.ItemArray);
                                var strings = Item.Split(' ');
                                strings = Array.FindAll(strings, (x => x != ""));
                                Item = string.Join(" ", strings);
                                Items.Add(Item);
                            }
                            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Items);
                            ItemList.Adapter = adapter;
                        }
                        else
                        {
                            FindButton.Text = "Nie ma";
                        }
                    }
                }
                catch (SqlException exc)
                {
                    FindButton.Text = exc.Message;
                }
            };

            QRButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(QR_Code));
                this.StartActivity(intent);
                Finish();
            };
            /*
            ItemList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
               // FindField.Text = Items[e.Position];
            };*/
        }
    }
}