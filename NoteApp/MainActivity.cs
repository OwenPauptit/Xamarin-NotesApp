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

namespace NoteApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        TextInputEditText title;
        TextInputEditText body;
        TextInputEditText noteChoice;
        ListView noteListView;
        List<string> allNoteNames;

        const string NOTELIST = "NoteList";

        public string MakeFilePath(string title)
        {
            var fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), (title + ".txt"));
            return fileName;
        }

        [Java.Interop.Export("saveNote")]
        public void saveNote(View v)
        {
            if (title.Text != NOTELIST)
            {
                string file = MakeFilePath(title.Text);
                string contents = body.Text;
                File.WriteAllText(file, contents);
                if (!allNoteNames.Contains(title.Text))
                {
                    allNoteNames.Add(title.Text);
                    File.AppendAllText(MakeFilePath(NOTELIST), "\n");
                    File.AppendAllText(MakeFilePath(NOTELIST), title.Text);
                }
                LaunchMainActivity();
            }
        }

        public void readNote(string file)
        {
            if (File.Exists(MakeFilePath(file)))
            {
                body.Text = File.ReadAllText(MakeFilePath(file));
            }
            title.Text = file;
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

        public List<string> GetAllNotes()
        {
            var notes = new List<string>();
            if (File.Exists(MakeFilePath(NOTELIST)))
            {
                string[] n = File.ReadAllLines(MakeFilePath(NOTELIST));
                notes = n.ToList();
            }
            return notes;
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            LaunchMainActivity();

        }

        private void LaunchMainActivity()
        {

            SetContentView(Resource.Layout.activity_main);

            noteChoice = FindViewById<TextInputEditText>(Resource.Id.noteChoiceText);
            noteListView = FindViewById<ListView>(Resource.Id.notesListView);

            allNoteNames = GetAllNotes();

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allNoteNames);

            noteListView.Adapter = adapter;
            noteListView.ItemClick += noteListView_ItemClick;
        }

        private void noteListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            string file = allNoteNames[e.Position];
            SetContentView(Resource.Layout.activity_note);
            body = FindViewById<TextInputEditText>(Resource.Id.editBody);
            title = FindViewById<TextInputEditText>(Resource.Id.editTitle);
            readNote(file);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}