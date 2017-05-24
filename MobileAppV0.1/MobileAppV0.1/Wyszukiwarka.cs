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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ScrollView ItemScroll;
            Button FindButton;
            ListView ItemList;

            // Create your application here
            SetContentView(Resource.Layout.Wyszukiwarka);

            ItemScroll = FindViewById<ScrollView>(Resource.Id.ItemScroll);
            FindButton = FindViewById<Button>(Resource.Id.FindButton);
            ItemList = FindViewById<ListView>(Resource.Id.ItemList);
        }
    }
}