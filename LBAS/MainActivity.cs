using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using Firebase;
using Android.Util;
using Firebase.Auth;
using Firebase.Iid;
using Firebase.Messaging;

namespace LBAS
{
    [Activity(Label = "LBAS", MainLauncher = true,Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity, IOnCompleteListener
    {
        const string TAG = "MainActivity";
        EditText username;
        EditText password;
        public static FirebaseApp app;
        FirebaseAuth auth;
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        private const string FirebaseURL = "https://locationbasedalertsystem.firebaseio.com/";
        RelativeLayout Main_activity;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //InitFirebaseAuth();
            FirebaseApp.InitializeApp(this);
            auth = FirebaseAuth.Instance;
            Typeface tf = Typeface.CreateFromAsset(Assets, "DEEP.ttf");
           
            var logintext = FindViewById<TextView>(Resource.Id.login_title);
            logintext.SetTypeface(tf,TypefaceStyle.Normal);

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            var forgotpassword = FindViewById<TextView>(Resource.Id.forgotpassword);
            var signin = FindViewById<Button>(Resource.Id.submit);
            var signup = FindViewById<Button>(Resource.Id.Signup);
            Main_activity = FindViewById<RelativeLayout>(Resource.Id.Main_aactivity);
            forgotpassword.Click += (object sender, EventArgs e) => {
                StartActivity(new Intent(this, typeof(ForgotPassword)));
                Finish();
            };
            signin.Click += (object sender, EventArgs e) =>
            {
                LoginUser(username.Text,password.Text);
            };

            signup.Click += (object sender, EventArgs e) =>
            {
                StartActivity(new Intent(this, typeof(Signup)));
                Finish();
            };


            if (!GetString(Resource.String.google_app_id).Equals("1:373383567274:android:0aa0561262b11790"))
                throw new System.Exception("Invalid Json file");

            System.Threading.Tasks.Task.Run(() =>
             {
                var instanceId = FirebaseInstanceId.Instance;
                instanceId.DeleteInstanceId();
                Android.Util.Log.Debug("TAG", "{0} {1}", instanceId.Token, instanceId.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope));
                 CreateNotificationChannel();
                 FirebaseMessaging.Instance.SubscribeToTopic("news");
                 Log.Debug(TAG, "Subscribed to remote notifications");

             });

        }
       

           

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "news", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    

    private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
            .SetApplicationId("1:373383567274:android:0aa0561262b11790")
            .SetApiKey("AIzaSyAGqoPS0yQlgc0W8nKUCPhBnfCV-JudqNc")
            .SetDatabaseUrl("https://locationbasedalertsystem.firebaseio.com/")
            .SetGcmSenderId("373383567274")
            .SetStorageBucket("gs://locationbasedalertsystem.appspot.com")
            .Build();

            if (app == null)
            {
                app = FirebaseApp.InitializeApp(this, options);
            }

            auth = FirebaseAuth.GetInstance(app);
        }

        private void LoginUser(string email, string password)
        {
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                Finish();
            }
            else
            {
                Snackbar snackbar = Snackbar.Make(Main_activity,"Login Failed",Snackbar.LengthShort);
                snackbar.Show();

            }
        }
    }
}

