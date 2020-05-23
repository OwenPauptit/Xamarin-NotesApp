package crc64c5260c7c3e0bd5a6;


public class NoteActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_saveNote:(Landroid/view/View;)V:__export__\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"n_onCreateOptionsMenu:(Landroid/view/Menu;)Z:GetOnCreateOptionsMenu_Landroid_view_Menu_Handler\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_LaunchNoteActivity:(Landroid/view/View;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("NoteApp.NoteActivity, NoteApp", NoteActivity.class, __md_methods);
	}


	public NoteActivity ()
	{
		super ();
		if (getClass () == NoteActivity.class)
			mono.android.TypeManager.Activate ("NoteApp.NoteActivity, NoteApp", "", this, new java.lang.Object[] {  });
	}


	public void saveNote (android.view.View p0)
	{
		n_saveNote (p0);
	}

	private native void n_saveNote (android.view.View p0);


	public boolean onOptionsItemSelected (android.view.MenuItem p0)
	{
		return n_onOptionsItemSelected (p0);
	}

	private native boolean n_onOptionsItemSelected (android.view.MenuItem p0);


	public boolean onCreateOptionsMenu (android.view.Menu p0)
	{
		return n_onCreateOptionsMenu (p0);
	}

	private native boolean n_onCreateOptionsMenu (android.view.Menu p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	private void LaunchNoteActivity (android.view.View p0)
	{
		n_LaunchNoteActivity (p0);
	}

	private native void n_LaunchNoteActivity (android.view.View p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
