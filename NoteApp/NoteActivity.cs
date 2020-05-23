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

        const string NOTELIST = "NoteList";

        /*public string MakeFilePath(string title)
        {
            var fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), (title + ".txt"));
            return fileName;
        }*/

        [Java.Interop.Export("saveNote")]
        public void saveNote(View v)
        {
            if (title.Text != NOTELIST && title.Text != "")
            {
                string file = MainActivity.MakeFilePath(title.Text);
                string contents = body.Text;
                File.WriteAllText(file, contents);
                if (!MainActivity.allNoteNames.Contains(title.Text))
                {
                    MainActivity.allNoteNames.Add(title.Text);
                    File.AppendAllText(MainActivity.MakeFilePath(NOTELIST), "\n");
                    File.AppendAllText(MainActivity.MakeFilePath(NOTELIST), title.Text);
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

        /*[Java.Interop.Export("LoadNote")]
        public void LoadNote(View v)
        {
            string file = noteChoice.Text;
            readNote(file);
        }*/

        /*public List<string> GetAllNotes()
        {
            var notes = new List<string>();
            if (File.Exists(MakeFilePath(NOTELIST)))
            {
                string[] n = File.ReadAllLines(MakeFilePath(NOTELIST));
                notes = n.ToList();
            }
            return CheckForEmptyNotes(notes);
        }*/

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

            readNote(MainActivity.f);
        }

        /*private void NoteListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Snackbar.Make(rootView, $"Delete '{allNoteNames[e.Position]}'?", Snackbar.LengthShort).SetAction("Yes", v => DeleteNote(e.Position)).Show();
        }*/

        /*private void RefreshAdapter()
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allNoteNames);

            noteListView.Adapter = adapter;
        }*/

        /*private void DeleteNote(int position)
        {
            File.Delete(MakeFilePath(allNoteNames[position]));
            allNoteNames.RemoveAt(position);
            OverWriteNoteList();
            RefreshAdapter();
        }*/

        /*private List<string> CheckForEmptyNotes(List<string> notes)
        {
            var indexToDelete = new List<int>();
            foreach (var n in notes)
            {
                if (n == "")
                {
                    indexToDelete.Add(notes.IndexOf(n));
                }
            }
            foreach (var i in indexToDelete)
            {
                notes.RemoveAt(i);
            }
            return notes;
        }*/

        /*private void OverWriteNoteList()
        {
            File.Delete(MakeFilePath(NOTELIST));
            foreach (var note in allNoteNames)
            {
                File.AppendAllText(MakeFilePath(NOTELIST), "\n");
                File.AppendAllText(MakeFilePath(NOTELIST), note);
            }
        }*/

        /*private void noteListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            string file = allNoteNames[e.Position];
            SetContentView(Resource.Layout.activity_note);
            body = FindViewById<TextInputEditText>(Resource.Id.editBody);
            title = FindViewById<TextInputEditText>(Resource.Id.editTitle);
            readNote(file);
        }*/

        
    }
}

