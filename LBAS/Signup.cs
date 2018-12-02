
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;

namespace LBAS
{
    [Activity(Label = "Signup", Theme = "@style/MyTheme")]
    public class Signup : AppCompatActivity, IOnCompleteListener
    {

        RelativeLayout sign_up;
        private EditText username;
        private EditText password;
        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.signup);

            auth = FirebaseAuth.GetInstance(MainActivity.app);
            Typeface tf = Typeface.CreateFromAsset(Assets, "DEEP.ttf");

            var logintext = FindViewById<TextView>(Resource.Id.login_title);
            logintext.SetTypeface(tf, TypefaceStyle.Normal);

            var alreadyaccount = FindViewById<TextView>(Resource.Id.AlreadyAccount);
            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);

            alreadyaccount.Click += (object sender, EventArgs e) => {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            };
            sign_up = FindViewById<RelativeLayout>(Resource.Id.sign_upactivity);

            var signup = FindViewById<Button>(Resource.Id.sign_up);
            signup.Click += (object sender, EventArgs e) => {
                SignUpUser(username.Text,password.Text);
            };


        }
        private void SignUpUser(string email, string password)
        {
            auth.CreateUserWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this, this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful == true)
            {
                Snackbar snackBar = Snackbar.Make(sign_up, "Register successfully", Snackbar.LengthShort);
                snackBar.Show();
            }
            else
            {
                Snackbar snackBar = Snackbar.Make(sign_up, "Register Failed ", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}
