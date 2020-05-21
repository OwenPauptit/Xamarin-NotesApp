using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using System.IO;
using Android.Views;

namespace NoteApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        TextInputEditText title;
        TextInputEditText body;
        TextInputEditText noteChoice;

        protected string MakeFilePath(string title)
        {
            var fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), (title + ".txt"));
            return fileName;
        }

        [Java.Interop.Export("saveNote")]
        public void saveNote(View v)
        {
            string file = MakeFilePath(title.Text);
            string contents = body.Text;
            File.WriteAllText(file, contents);
        }

        public void readNote(string file)
        {
            if (File.Exists(MakeFilePath(file)))
            {
                body.Text = File.ReadAllText(MakeFilePath(file));
                title.Text = file;
            }
        }

        [Java.Interop.Export("LoadNote")]
        public void LoadNote(View v)
        {
            string file = noteChoice.Text;
            SetContentView(Resource.Layout.activity_note);
            body = FindViewById<TextInputEditText>(Resource.Id.editBody);
            title = FindViewById<TextInputEditText>(Resource.Id.editTitle);
            readNote(file);
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            noteChoice = FindViewById<TextInputEditText>(Resource.Id.NoteChoiceText);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}