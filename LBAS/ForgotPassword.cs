
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
    [Activity(Label = "ForgotPassword", Theme = "@style/MyTheme")]
    public class ForgotPassword : AppCompatActivity, IOnCompleteListener
    {
        private FirebaseAuth auth;
        private RelativeLayout activity_forgot;
        private EditText resetpass;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Forgotpass);
            // Create your application here
            auth = FirebaseAuth.GetInstance(MainActivity.app);
            Typeface tf = Typeface.CreateFromAsset(Assets, "DEEP.ttf");

            var logintext = FindViewById<TextView>(Resource.Id.login_title);
            logintext.SetTypeface(tf, TypefaceStyle.Normal);

            activity_forgot = FindViewById<RelativeLayout>(Resource.Id.forgot_passwordactivity);
            var resetpassword = FindViewById<Button>(Resource.Id.ResetPassword);
            resetpass = FindViewById<EditText>(Resource.Id.resetpass); 
            resetpassword.Click += (object sender, EventArgs e) => {
                ResetPassword(resetpass.Text);
            };

            var back = FindViewById<TextView>(Resource.Id.Back_forgot);

            back.Click += (object sender, EventArgs e) => {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            };
        }

        private void ResetPassword(string email)
        {
            auth.SendPasswordResetEmail(email)
                .AddOnCompleteListener(this, this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful == false)
            {
                Snackbar snackBar = Snackbar.Make(activity_forgot, "Reset password failed", Snackbar.LengthShort);
                snackBar.Show();
            }
            else
            {
                Snackbar snackBar = Snackbar.Make(activity_forgot, "Reset password link sent to email : " + resetpass.Text, Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}
