
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Xamarin.Database;

namespace LBAS
{
    [Activity(Label = "ReportAlert", Theme = "@style/MyTheme")]
    public class ReportAlert : AppCompatActivity
    {
        private EditText titletext;
        private EditText subtitletext;
        private Button submitalert;
        RelativeLayout relativeLayout;
        private const string FirebaseURL = "https://locationbasedalertsystem.firebaseio.com/";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReportAlert);

            titletext = FindViewById<EditText>(Resource.Id.Titlealert);
            subtitletext = FindViewById<EditText>(Resource.Id.subtitlealert);

            submitalert = FindViewById<Button>(Resource.Id.submitalert);
            relativeLayout = FindViewById<RelativeLayout>(Resource.Id.reportalert_activity);
            submitalert.Click += (object sender, EventArgs e) => {
                CreateAlert();
            
            };

            // Create your application here
        }

        private async void CreateAlert()
        {
            
            Listitem user = new Listitem();
            user.Uid = String.Empty;
            user.Title = titletext.Text;
            user.SubTitle = subtitletext.Text;
            var firebase = new FirebaseClient(FirebaseURL);

            //Add item
           var  item = await firebase.Child("alerts").PostAsync<Listitem>(user);
            if(item != null)
            {
                OnComplete(true);
            }
            else
            {
                OnComplete(false);
            }


        }

        public void OnComplete(bool task)
        {
            if (task)
            {
                Snackbar snackBar = Snackbar.Make(relativeLayout, "Alert Submitted successfully", Snackbar.LengthShort);
                snackBar.Show();
                StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                Finish();
            }
            else
            {
                Snackbar snackBar = Snackbar.Make(relativeLayout, "Alert Submission Failed ", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}
