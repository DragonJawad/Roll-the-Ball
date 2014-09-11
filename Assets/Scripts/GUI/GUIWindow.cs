using UnityEngine;
using System.Collections;

public class GUIWindow : MonoBehaviour {

	public enum TypeWindow{
		None,
		SecretsIndicator,
		InfoMenu,
		DeleteData,
		WorldInner,
		Tabular,
		Settings
	}

	public TypeWindow thisWindow;
	public string windowName;	// Label of the window created
	public string message;		// The message shown in the Window
	public TextMesh thisText;

	public GUISkin guiSkin;	// The custom skin

	// Both tabular and settings variables
	int tabInt = 0; // Int representation of the currently selected toolbar

/* Special Tabular Variables */	
	[TextBlock]
	public string[] tabNames = new string[3];	// Names for each tab
	[TextBlock]
	public string[] tabMessages = new string[3]; // Message within each tab
	
	// Following three have not been optimized for work correctly... yet...
	public Rect defaultSize = new Rect(10f, 0f, 60f, 10f); // Default tab size rect
	public Rect sizeMultiplier = new Rect(10f, 0f, 60f, 10f); // Input for size of the customTab;
	public bool customTabSize = false; // Flag for choosing a custom tab header size 
	Rect customTabRect; // Holds the size for the customTabRect

/* End Tabular Variables */

/* Special Settings variables */
	string[] settingsTabs = new string[]{"About", "Controls"};
	string aboutMsg = "This menu will give you access to the different editable settings. In the next update, you will be able to " +
		"toggle BGMs and SFX as well.";
	int tiltSensitivitySlider, joystickSizeSlider;
/* End Settings Variables */

	Rect secretRect;
	Rect infoRect;
	Rect deleteRect;
	Rect wNameRect;
	Rect tabRect; // Holds the size for the tabular window

	bool showWindow = false;

	// Use this for initialization
	void Start () {
//		print("----: " + defaultSize);
		if(guiSkin == null)
		{ // If no skin is attached, use default custom skin
			guiSkin = (GUISkin)Resources.Load ("GUI/CustomSkin");
		}
		
		if(thisText == null)
		{ // Just get this object's TextMesh
			thisText = this.GetComponent<TextMesh>();
		}
		
		if(thisWindow == TypeWindow.SecretsIndicator)
		{
			secretRect = new Rect(Screen.width/5,Screen.height/5,Screen.width*3/5,Screen.height*3/5);
		}
		else if(thisWindow == TypeWindow.Tabular || thisWindow == TypeWindow.Settings)
		{
			// Rect for the tabular window
			tabRect = new Rect(Screen.width*1/10,Screen.height*1/20,
			                   Screen.width*4/5, Screen.height*9/10);
		}
		
		infoRect = new Rect(Screen.width/10,Screen.height/10,Screen.width*4/5,Screen.height*4/5);
		deleteRect = new Rect(Screen.width/5,Screen.height/5,Screen.width*3/5,Screen.height*3/5);
		wNameRect = new Rect(Screen.width/5,Screen.height/5,Screen.width*3/5,Screen.height*3/5);
		
		if(thisWindow == TypeWindow.InfoMenu || thisWindow == TypeWindow.DeleteData){
			thisText = this.GetComponent<TextMesh>();
		}
	}
	
	void OnMouseEnter(){
		if(thisWindow == TypeWindow.InfoMenu || thisWindow == TypeWindow.DeleteData
		   || thisWindow == TypeWindow.WorldInner || thisWindow == TypeWindow.Tabular
		   || thisWindow == TypeWindow.Settings){
			Color color;
			color = thisText.color;
			color.a = 0.2f;
			thisText.color = color;
		}
	}
	
	void OnMouseExit(){
		if(thisWindow == TypeWindow.InfoMenu || thisWindow == TypeWindow.DeleteData
		   || thisWindow == TypeWindow.WorldInner || thisWindow == TypeWindow.Tabular
		   || thisWindow == TypeWindow.Settings){
			Color color;
			color = thisText.color;
			color.a = 1f;
			thisText.color = color;
		}
	}

	void OnGUI(){
		ActivateGUISkin();
		
		if(showWindow){
			if(thisWindow == TypeWindow.SecretsIndicator){
				GUI.Window(0,secretRect,DoWindow, "");
			}
			else if(thisWindow == TypeWindow.InfoMenu){
				GUI.Window(0,infoRect,DoWindowInfo, "");
			}
			else if(thisWindow == TypeWindow.DeleteData){
				GUI.Window(0,deleteRect,DoWindowDelete, "");
			}
			else if(thisWindow == TypeWindow.WorldInner){
				GUI.Window(0, wNameRect,DoWindow, "");
			}
			else if(thisWindow == TypeWindow.Tabular){
				GUI.Window(0, tabRect,TabularWindow, "");
			}
			else if(thisWindow == TypeWindow.Settings){
				GUI.Window(0, tabRect,SettingsWindow, "");
			}
		}
	}

	void DoWindow(int windowID){	
		GUI.skin.box.fontSize = Screen.width/28;
		GUI.skin.button.fontSize = Screen.width/25;
		GUI.Box (new Rect(Screen.width*1/10,0,Screen.width*2/5,Screen.height*1/10),windowName);

		GUI.skin.box.fontSize = Screen.width/32;
		GUI.skin.box.wordWrap = true;
		GUI.Box (new Rect(Screen.width*1/20, Screen.height*3/20, Screen.width*5/10,
		                  Screen.height*7/20), message);
		GUI.skin.box.wordWrap = false;

		if(GUI.Button (new Rect(Screen.width*1/5, Screen.height*10/20,
		                        Screen.width*1/5, Screen.height*1/10), "Close!")){
			Close();
		}
	}

	void DoWindowInfo(int windowID){
		GUI.Box (new Rect(Screen.width*3/20,0,Screen.width*5/10,Screen.height*1/10),windowName);

		GUI.skin.box.fontSize = Screen.width/30;
		GUI.skin.box.wordWrap = true;
		GUI.Box (new Rect(Screen.width*1/20, Screen.height*3/20, Screen.width*7/10,
		                  Screen.height*10/20), message);
		GUI.skin.box.wordWrap = false;
		
		if(GUI.Button (new Rect(Screen.width*3/10, Screen.height*14/20,
		                        Screen.width*1/5, Screen.height*1/10), "Close!")){
			Close();
		}
	}

	void DoWindowDelete(int windowID){
		GUI.skin.box.fontSize = Screen.width/28;
		GUI.skin.button.fontSize = Screen.width/25;
		GUI.Box (new Rect(Screen.width*1/10,0,Screen.width*2/5,Screen.height*1/10),windowName);

		GUI.skin.box.fontSize = Screen.width/30;
		GUI.skin.box.wordWrap = true;
		GUI.Box (new Rect(Screen.width*1/20, Screen.height*3/20, Screen.width*5/10,
		                  Screen.height*7/20), message);
		GUI.skin.box.wordWrap = false;

		if(GUI.Button (new Rect(Screen.width*1/10, Screen.height*10/20,
		                        Screen.width*1/5, Screen.height*1/10), "Delete!")){
		#if UNITY_EDITOR
			Debug.Log("DELTED ALL DATA!");
		#endif
			PlayerPrefs.DeleteAll(); 
			Close();
		}

		if(GUI.Button (new Rect(Screen.width*3/10, Screen.height*10/20,
		                        Screen.width*1/5, Screen.height*1/10), "Cancel!")){
			Close();
		}
	}	// End DoWindowDelete
	
	// Function for creating a tabbed window
	void TabularWindow(int windowID)
	{
		// Create the toolbar/tabs
		tabInt = GUI.Toolbar (new Rect (Screen.width*1/10, 0, Screen.width*3/5, Screen.height/10), tabInt, tabNames, "Toolbar");
		
	/*	if(!customTabSize)
	//	{
			tabInt = GUI.Toolbar (new Rect (Screen.width*1/10, 0, Screen.width*3/5, Screen.height/10), tabInt, tabNames, "Toolbar");
	//	}
		// Else use a custom tab size
		// Doesn't work correctly... YET
		else
		{
			customTabRect = new Rect(Screen.width * sizeMultiplier.x, Screen.height*sizeMultiplier.y,
			                         Screen.width*sizeMultiplier.width, Screen.height*sizeMultiplier.height);
			tabInt = GUI.Toolbar (customTabRect, tabInt, tabNames, "Toolbar");
		}
	*/	
		
		// Change the size of the font for the TextBox style
		GUI.skin.customStyles[1].fontSize = Screen.width/32;
		
		// Dynamically show the appropriate text
		GUI.Box (new Rect(Screen.width*1/10, Screen.height*5/20, Screen.width*3/5,
		                  Screen.height*5/10), tabMessages[tabInt]);
		
		if(GUI.Button (new Rect(Screen.width*3/10, Screen.height*8/10,
		                        Screen.width*1/5, Screen.height*1/10), "Close")){
			Close();
		}
	}

	void SettingsWindow(int windowID)
	{
		// Create the toolbar/tabs
		tabInt = GUI.Toolbar (new Rect (Screen.width*1/10, 0, Screen.width*3/5, Screen.height/10), tabInt, settingsTabs, "Toolbar");
		
		// If the first tab...
		if(tabInt == 0) // [About] tab
		{
			
			// General message for the About tab
			GUI.Box (new Rect(Screen.width*1/10, Screen.height*7/20, Screen.width*3/5,
			                  Screen.height*3/10), aboutMsg, guiSkin.box);
			
			// The close button
			if(GUI.Button (new Rect(Screen.width*3/10, Screen.height*8/10,
			                        Screen.width*1/5, Screen.height*1/10), "Close")){
				Close();
			}
		}
		else if(tabInt == 1) // [Controls] tab
		{		
			// JoysticksSize label box
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*1/10,
			                 Screen.width*6/20,Screen.height*1/10),"Joystick Size");
			// Box for showing the percent on the slider
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*2/10,
			                 Screen.width*1/10,Screen.height*1/10),joystickSizeSlider+"%");
			// Actual slider for the joystickSize setting
			Rect slideBox1 = new Rect(Screen.width*5/20,Screen.height*9/40,Screen.width*2/5,Screen.height/10);
			joystickSizeSlider = Mathf.RoundToInt(GUI.HorizontalSlider (slideBox1, joystickSizeSlider, 50, 150));
			
			// Tilt Sensitivity label box
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*3/10,
			                 Screen.width*6/20,Screen.height*1/10),"Tilt Sensitivity");
			// Box for showing the percent on the slider
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*4/10,
			                 Screen.width*1/10,Screen.height*1/10),tiltSensitivitySlider+"%");
			// Actual slider for the tiltSensitivity setting
			Rect slideBox2 = new Rect(Screen.width*5/20,Screen.height*17/40,Screen.width*2/5,Screen.height/10);
			tiltSensitivitySlider = Mathf.RoundToInt(GUI.HorizontalSlider (slideBox2, tiltSensitivitySlider, 20, 200));
			
			// Default button (make all the settings the default values)
			if(GUI.Button (new Rect(Screen.width*1/10,Screen.height*11/20,
			                        Screen.width*2/10, Screen.height*1/10), "Defaults")){
			//	ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				joystickSizeSlider = 100;
				tiltSensitivitySlider = 100;
			}
			
			// Cancel Button (don't save changes)
			if(GUI.Button (new Rect(Screen.width*3/20,Screen.height*7/10,
			                        Screen.width*1/5, Screen.height*2/10), "Cancel")){
			//	ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				// Close the Settings menu
				Close();
			}
			
			// Save changes and close the Settings menu
			if(GUI.Button (new Rect(Screen.width*9/20,Screen.height*7/10,
			                        Screen.width*1/5, Screen.height*2/10), "Save")){
			//	ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				
				// Close the Settings menu
				Close();
				
				// Apply and save changes
				Control.ChangeControlSettings(joystickSizeSlider, tiltSensitivitySlider);
			}
		}
		
	}

	public void Activate(){
		// Show the GUI Window
		showWindow = true;
		// Restrain the MenuControl from opening anything
		Control.ToggleBlockAction(true);
		
		// Unique activation code for Settings window
		if(thisWindow == TypeWindow.Settings)
		{
			// (Re)set the sliders to the current settings
			joystickSizeSlider = Control.joystickSize;
			if(joystickSizeSlider == 0)
			{ // If joystickSize is unassigned, use default value
				joystickSizeSlider = 100;
			}
			
			tiltSensitivitySlider = Control.tiltSensitivity;
			if(tiltSensitivitySlider == 0)
			{ // If tiltSensitivitySlider is unassigned, use default value
				tiltSensitivitySlider = 100;
			}
		}
	}
	
	public void Close(){
		// Close the GUI Window
		showWindow = false;
		// Allow the MenuControl to interact again
		Control.ToggleBlockAction(false);
	}
	
	public void ActivateGUISkin()
	{	// Test to see if can activate GUI Skin remotely
	
		// Put skin into affect
		GUI.skin = guiSkin;
		GUI.skin.box.fontSize = Screen.width/30; // Dynamically change the szie of the font for boxes
		GUI.skin.button.fontSize = Screen.width/30; // Dynamicaly change the size of the font for buttons
		// Change the font size of the TextBox custom style
		GUI.skin.customStyles[1].fontSize = Screen.width/34;
		GUI.skin.customStyles[2].fontSize = Screen.width/30;
	}
}
