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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Wyszukiwarka);

            FindButton = FindViewById<Button>(Resource.Id.FindButton);
            ItemList = FindViewById<ListView>(Resource.Id.ItemList);
            LogoutButton = FindViewById<Button>(Resource.Id.LogoutButton);
            FindField = FindViewById<EditText>(Resource.Id.FindField);
            
            Items = new List<string>();

            Items.Add("Item1");
            Items.Add("Item2");
            Items.Add("Item1");
            Items.Add("Item2");
            Items.Add("Item1");
            Items.Add("Item2");
            Items.Add("Item1");
            Items.Add("Item1");
            Items.Add("Item2");
            Items.Add("Item2");

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Items);

            ItemList.Adapter = adapter;
            LogoutButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(Panel_Logowania));
                this.StartActivity(intent);
                Finish();
            };

            ItemList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                FindField.Text = Items[e.Position];
            };
        }
    }
}