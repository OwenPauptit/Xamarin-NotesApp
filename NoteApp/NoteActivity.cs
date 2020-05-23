using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using System.IO;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using System;
using Android.Support.V7.Widget;
using Java.Util.Zip;

namespace NoteApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class NoteActivity : AppCompatActivity
    {
        TextInputEditText title;
        TextInputEditText body;

        [Java.Interop.Export("saveNote")]
        public void saveNote(View v)
        {
            if (title.Text == MainActivity.NOTELIST || title.Text == "")
            {
                Toast.MakeText(Application.Context, "Invalid Title", ToastLength.Short).Show();
            }
            else
            {
                string file = MainActivity.MakeFilePath(title.Text);
                string contents = body.Text;
                File.WriteAllText(file, contents);
                if (!MainActivity.allNoteNames.Contains(title.Text))
                {
                    MainActivity.allNoteNames.Add(title.Text);
                    File.AppendAllText(MainActivity.MakeFilePath(MainActivity.NOTELIST), "\n");
                    File.AppendAllText(MainActivity.MakeFilePath(MainActivity.NOTELIST), title.Text);
                }
                StartActivity(typeof(MainActivity));
            }
        }

        public void readNote(string file)
        {
            if (File.Exists(MainActivity.MakeFilePath(file)))
            {
                body.Text = File.ReadAllText(MainActivity.MakeFilePath(file));
            }
            title.Text = file;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            StartActivity(typeof(MainActivity));
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolBarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            LaunchNoteActivity();
        }

        [Java.Interop.Export("LaunchNoteActivity")]
        private void LaunchNoteActivity(View v = null)
        {

            SetContentView(Resource.Layout.activity_note);

            var toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarNote);
            SetSupportActionBar(toolBar);
            SupportActionBar.Title = "Notes!";

            title = FindViewById<TextInputEditText>(Resource.Id.editTitle);
            body = FindViewById<TextInputEditText>(Resource.Id.editBody);

            readNote(MainActivity.fileToOpen);
        }


        
    }
}

