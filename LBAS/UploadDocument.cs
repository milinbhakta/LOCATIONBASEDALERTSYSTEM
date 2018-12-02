
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Storage;
using Java.Lang;
using Org.Apache.Http.Authentication;

namespace LBAS
{
    [Activity(Label = "UploadDocument", Theme = "@style/MyTheme")]
    public class UploadDocument : AppCompatActivity, IOnCompleteListener
    { 
    private Button btntakepic;
        private Button btnchoosepic;
        private ImageView imageviewpic;
        private Button UploadDoc;
        private Android.Net.Uri FilePath;
        private byte[] bitmapData;
        private const int PICK_IMAGE_REQUEST = 143;
        RelativeLayout activity;

        FirebaseStorage firebaseStorage;
        StorageReference storageReference;
       



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UploadDocument);




            firebaseStorage = FirebaseStorage.Instance;
            storageReference = firebaseStorage.GetReferenceFromUrl("gs://locationbasedalertsystem.appspot.com");
            Android.Support.V7.App.ActionBar actionBar = this.SupportActionBar;
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);
            activity = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
            btntakepic = FindViewById<Button>(Resource.Id.takephoto);
            btnchoosepic = FindViewById<Button>(Resource.Id.choosephoto);
            imageviewpic = FindViewById<ImageView>(Resource.Id.imageView1);
            UploadDoc = FindViewById<Button>(Resource.Id.uploadDoc);
            btnchoosepic.Click += (object sender, EventArgs e) => {
                ChooseImage();
            
            };

            btntakepic.Click += (object sender, EventArgs e) => {
                Intent intent = new Intent(MediaStore.ActionImageCapture);                 StartActivityForResult(intent, 0);
            };

            UploadDoc.Click+= (object sender, EventArgs e) => { 
                if(FilePath != null)
                {

                    var images = storageReference.Child("Images/" + Guid.NewGuid().ToString());
                    images.PutFile(FilePath).AddOnCompleteListener(this);
                }
            };

            // Create your application here
        }





        private void ChooseImage()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent,"Select Picture"),PICK_IMAGE_REQUEST);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PICK_IMAGE_REQUEST && resultCode == Result.Ok && data != null && data.Data != null)
            {
                FilePath = data.Data;
                try{
                    Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, FilePath);
                    imageviewpic.SetImageBitmap(bitmap);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if ((requestCode == 0) && (resultCode == Result.Ok) && (data != null) && (data.Data != null))             {                 FilePath = data.Data;                 Bitmap bitmap = (Bitmap)data.Extras.Get("data");                 imageviewpic.SetImageBitmap(bitmap);                  using (var stream = new MemoryStream())                 {                     bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();                 }              } 
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Snackbar snackbar = Snackbar.Make(activity, "Uploaded!!", Snackbar.LengthShort);
                snackbar.Show();
            }
            else
            {
                Snackbar snackbar = Snackbar.Make(activity, "Uploading Failed!", Snackbar.LengthShort);
                snackbar.Show();

            }
        }
    }
}
