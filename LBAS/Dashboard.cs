
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;


namespace LBAS
{
    [Activity(Label = "Dashboard", Theme = "@style/MyTheme")]
    public class Dashboard : AppCompatActivity
    {
        LinearLayout activity;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Dashboard);
            // Create your application here
            var reportalert = FindViewById<LinearLayout>(Resource.Id.reportalert);
            var uploaddocumentc = FindViewById<LinearLayout>(Resource.Id.uploaddocumentc);
            var maplayoutdash = FindViewById<LinearLayout>(Resource.Id.maplayoutdash);
            var settingsdash = FindViewById<LinearLayout>(Resource.Id.settingdash);
            activity = FindViewById<LinearLayout>(Resource.Id.dashboardactivity);
            reportalert.Click += (object sender, EventArgs e) => {
                StartActivity(new Android.Content.Intent(this, typeof(Alerts)));
                Finish(); 
            };
            uploaddocumentc.Click+= (object sender, EventArgs e) => {
                StartActivity(new Android.Content.Intent(this, typeof(UploadDocument)));
                Finish(); 
            };
            maplayoutdash.Click += (object sender, EventArgs e) => {
                StartActivity(new Android.Content.Intent(this, typeof(Map)));
                Finish();
            };
            settingsdash.Click += (object sender, EventArgs e) => {
                Snackbar snackbar = Snackbar.Make(activity, "Settings!", Snackbar.LengthShort);
                snackbar.Show();
            };
           
        }
        public void makeToast(Context ctx, string str)
        {
            Toast.MakeText(ctx, str, ToastLength.Long).Show();
        }




    }

}
