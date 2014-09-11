using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {
	
	public GUISkin MenuSkin;
	
	public enum ToggleType{
		A=0,
		B=1,
		C=2,
		D=3
	};
	
	ToggleType thisToggle;
	
	bool toggleTxt;
	int toolbarInt = 0;
	string[] toolbarStrings = new string[]{"Toolbar1", "Test2", "PIE!"};
	int selGridInt = 0;
	string[] selStrings = new string[]{"Grid 1", "Grid 2", "Grid 3", "Grid 4"};
	float hSliderValue = 0f;
	float hSbarValue;
	
	// Use this for initialization
	void Start () {
		if(MenuSkin == null)
		{
			MenuSkin = (GUISkin)Resources.Load ("GUI/CustomSkin");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = MenuSkin;
		
		Rect genRect
			= new Rect(Screen.width*1/10,Screen.height*1/20,
			           Screen.width*4/5, Screen.height*9/10);
			           
		GUI.skin.box.fontSize = Screen.width/35;
		GUI.skin.button.fontSize = Screen.width/30;
		// Change the font size of the TextBox custom style
		GUI.skin.customStyles[1].fontSize = Screen.width/34;
		GUI.skin.customStyles[2].fontSize = Screen.width/30;
				
		GUI.BeginGroup(genRect);
		GUI.Box (new Rect(0,0,Screen.width*4/5,Screen.height*9/10), "", "BoxTitle");
		
		toolbarInt = GUI.Toolbar (new Rect (Screen.width*1/10, 0, Screen.width*3/5, Screen.height/10), toolbarInt, toolbarStrings, "Toolbar");
		print("Toolbar is: " + toolbarInt);
		
		thisToggle = (ToggleType)toolbarInt;
		print(thisToggle);
		
		if(thisToggle == ToggleType.B)
		{
			GUI.Button(new Rect(0,25,100,20),"I am a button");
			GUI.Label (new Rect (0, 50, 100, 20), "I'm a Label!");
			toggleTxt = GUI.Toggle(new Rect(0, 75, 200, 30), toggleTxt, "I am a Toggle button");
			selGridInt = GUI.SelectionGrid (new Rect (0, 170, 200, 40), selGridInt, selStrings, 2);
			hSliderValue = GUI.HorizontalSlider (new Rect (0, 210, 100, 30), hSliderValue, 0f, 1f);
			hSbarValue = GUI.HorizontalScrollbar (new Rect (0, 240, 100, 30), hSbarValue, 1f, 0f, 10f);
			GUI.Box(new Rect(0,270,50,30), "Test Box!");
		}
		
		GUI.EndGroup ();
	}
	
	/*		void OnGUI() {
		GUI.skin = MenuSkin;
		
		Rect genRect
			= new Rect(Screen.width*1/10,Screen.height*1/20,
			           Screen.width*4/5, Screen.height*9/10);
			           
		GUI.skin.box.fontSize = Screen.width/35;
		GUI.skin.button.fontSize = Screen.width/30;
		// Change the font size of the TextBox custom style
		GUI.skin.customStyles[1].fontSize = Screen.width/34;
				
		GUI.BeginGroup(genRect);
		GUI.Box (new Rect(0,0,Screen.width*4/5,Screen.height*9/10), "", "BoxTitle");
		
//		Rect nameBox = new Rect(Screen.width*1/5,0, Screen.width*2/5, Screen.height/10);
//		GUI.Box(nameBox, "Testing Tabular GUI");
		
		toolbarInt = GUI.Toolbar (new Rect (Screen.width*1/10, 0, Screen.width*3/5, Screen.height/10), toolbarInt, toolbarStrings);
	//	print("Toolbar is: " + toolbarInt);
		
		thisToggle = (ToggleType)toolbarInt;
	//	print(thisToggle);
		
		if(thisToggle == ToggleType.B)
		{
			GUI.Button(new Rect(0,25,100,20),"I am a button");
			GUI.Label (new Rect (0, 50, 100, 20), "I'm a Label!");
			toggleTxt = GUI.Toggle(new Rect(0, 75, 200, 30), toggleTxt, "I am a Toggle button");
			selGridInt = GUI.SelectionGrid (new Rect (0, 170, 200, 40), selGridInt, selStrings, 2);
			hSliderValue = GUI.HorizontalSlider (new Rect (0, 210, 100, 30), hSliderValue, 0f, 1f);
			hSbarValue = GUI.HorizontalScrollbar (new Rect (0, 240, 100, 30), hSbarValue, 1f, 0f, 10f);
			GUI.Box(new Rect(0,270,50,30), "Test Box!");
		}
		
		GUI.EndGroup ();
	}
*/
}
