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


namespace NoteApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        CoordinatorLayout rootView;
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

            var toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);
            SetSupportActionBar(toolBar);
            SupportActionBar.Title = "Notes!";

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
            return CheckForEmptyNotes(notes);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            LaunchMainActivity();
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

            LaunchMainActivity();
        }

        [Java.Interop.Export("LaunchMainActivity")]
        private void LaunchMainActivity(View v = null)
        {

            SetContentView(Resource.Layout.activity_main);

            var toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);
            SetSupportActionBar(toolBar);
            SupportActionBar.Title = "Notes!";

            rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);
            noteChoice = FindViewById<TextInputEditText>(Resource.Id.noteChoiceText);
            noteListView = FindViewById<ListView>(Resource.Id.notesListView);

            if (allNoteNames != null) { allNoteNames.Clear(); }
            allNoteNames = GetAllNotes();

            RefreshAdapter();

            noteListView.ItemClick += noteListView_ItemClick;
            noteListView.ItemLongClick += NoteListView_ItemLongClick;
        }

        private void NoteListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Snackbar.Make(rootView, $"Delete '{allNoteNames[e.Position]}'?", Snackbar.LengthShort).SetAction("Yes", v => DeleteNote(e.Position)).Show();
        }

        private void RefreshAdapter()
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, allNoteNames);

            noteListView.Adapter = adapter;
        }

        private void DeleteNote(int position)
        {
            File.Delete(MakeFilePath(allNoteNames[position]));
            allNoteNames.RemoveAt(position);
            OverWriteNoteList();
            RefreshAdapter();
        }

        private List<string> CheckForEmptyNotes(List<string> notes)
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
        }

        private void OverWriteNoteList()
        {
            File.Delete(MakeFilePath(NOTELIST));
            foreach (var note in allNoteNames)
            {
                File.AppendAllText(MakeFilePath(NOTELIST), "\n");
                File.AppendAllText(MakeFilePath(NOTELIST), note);
            }
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

