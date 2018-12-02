using System;
using Android.App;
using Firebase.Iid;

namespace LBAS
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class MyFirebaseIdService : FirebaseInstanceIdService
    {

        public override void OnTokenRefresh()
        {
            base.OnTokenRefresh();
            Android.Util.Log.Debug("Refreshed Token:", FirebaseInstanceId.Instance.Token);
        }
    }
}
