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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        CoordinatorLayout rootView;
        TextInputEditText noteChoice;
        ListView noteListView;
        public static List<string> allNoteNames;
        public static string fileToOpen;

        public const string NOTELIST = "NoteList";

        public static string MakeFilePath(string title)
        {
            var fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), (title + ".txt"));
            return fileName;
        }


        [Java.Interop.Export("LoadNote")]
        public void LoadNote(View v)
        {
            fileToOpen = noteChoice.Text;
            StartActivity(typeof(NoteActivity));
            
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
            fileToOpen = file;
            StartActivity(typeof(NoteActivity));
        }

        
    }
}

