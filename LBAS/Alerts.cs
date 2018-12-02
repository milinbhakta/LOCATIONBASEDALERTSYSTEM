
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Firebase.Xamarin.Database;
using Java.Lang;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace LBAS
{
    [Activity(Label = "Alerts", Theme = "@style/MyTheme")]
    public class Alerts : AppCompatActivity
    {
        List<Listitem> Datalist = new List<Listitem>();
        private ProgressBar circular_progress;
        ListView list_view;
        private ListAdapter adapter;

        private const string FirebaseURL = "https://locationbasedalertsystem.firebaseio.com/";

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.alerts);

            Android.Support.V7.App.ActionBar actionBar = this.SupportActionBar;
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);
            circular_progress = FindViewById<ProgressBar>(Resource.Id.circularProgress);
            list_view = FindViewById<ListView>(Resource.Id.list_of_messages);
            await LoadData();

           
            //list_view.Touch += async (object sender, View.TouchEventArgs e) => {

            //    await LoadData();
            //};



        }









        private async Task LoadData()
        {
            circular_progress.Visibility = ViewStates.Visible;
            var firebase = new FirebaseClient(FirebaseURL);
            var items = await firebase
                .Child("alerts")
                .OnceAsync<Listitem>();

            foreach (var item in items)
            {
                Listitem acc = new Listitem();
                acc.Uid = item.Key;
                acc.Title = item.Object.Title;
                acc.SubTitle = item.Object.SubTitle;
                acc.Image = item.Object.Image;

                Datalist.Add(acc);
            }




            adapter = null;
            adapter = new ListAdapter(this, Datalist);
            adapter.NotifyDataSetChanged();
            
            list_view.Adapter = adapter;

            circular_progress.Visibility = ViewStates.Invisible;
        }

       
        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.menu_alert, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    StartActivity(new Android.Content.Intent(this, typeof(Dashboard)));
                    Finish();
                    return true;
                case Resource.Id.action_insert:
                    StartActivity(new Android.Content.Intent(this, typeof(ReportAlert)));
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

       

    }

    internal class ListAdapter : BaseAdapter<Listitem>
    {
        private Activity context;
        private List<Listitem>  datalist;

        public ListAdapter(Activity alerts, List<Listitem>  datalist)
        {
            this.context = alerts;
            this.datalist = datalist;
        }

        public override Listitem this[int position] {
            get
            {

                return datalist[position];

            }

        }

        public override int Count {
            get
            {

                return datalist.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (convertView == null)
            {

                view = context.LayoutInflater.Inflate(Resource.Layout.List_item, null, false);

            }
            Listitem item = this[position];
            view.FindViewById<TextView>(Resource.Id.title).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.subtitle).Text = item.SubTitle;
           

            ImageView myimage = view.FindViewById<ImageView>(Resource.Id.imageView1);
            Glide.With(context).Load("http://icons-for-free.com/free-icons/png/512/718953.png").Into(myimage);
            return view;

        }
    }
}
