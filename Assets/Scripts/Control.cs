using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	/*\	Begin variable declarations \*/
	public LevelType thisType;
	public string thisLevelID;
	public string thisWorldID;

	// Get level info
	static public int pastTopCount;
	static public float pastTopTime;
	static public string pastTopTimeText;
	static public string worldID;
	static public string levelDesc;
	static public bool secretCheck = false;
	static public bool secretEarned = false;
	static public int secretsTotal = 0;		// Total secrets in the world
	static public string unlockID;	// What the level unlocks
	static public int aTotal;
	static public int secretsCounter = 0;	// Changed by TypeMenu

	// Settings Variables
	static public bool controlsChangeCheck = false; // Flag for changing controls
	static public int joystickSize; // in percent, so 100 = 100%, from 50 to 150
	static public int tiltSensitivity; // in percent, from 20% to 200%

	static public int currentControl = 0;	// Get the type of control, 0 = tilt, 1 = joystick
	
	// Special variables
	// Currently only used for MenuControl- stopping GUI double action
	static public bool blockAction = false; // Flag for blocking actions
	/*\ End variable declarations \*/

	/*\ Start class declarations \*/
	public enum LevelType{
		Other,
		WorldSelect,
		LevelSelect,
		Game
	}

	public LevelData thisLevel;
	public WorldData thisWorld;

	// Use this for initialization
	void Start () {
		secretsCounter = 0;
		Resources.UnloadUnusedAssets ();
		Time.timeScale = 1;
		if (thisType == LevelType.Other) {
		//	print ("Undefined LevelType?");
		}

		else if (thisType == LevelType.WorldSelect) {	
			importWorldData(thisWorldID);
		}

		else if (thisType == LevelType.LevelSelect){
			importSingleWorldData(thisWorldID);
		}

		else if(thisType == LevelType.Game){

			importSingleWorldData(thisWorldID);
			importSingleLevelData(thisLevelID);

			importSaveData();
			GetControlSettings();	// Get all the control settings, from joystick size to default control scheme
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void WinLevel(int pickCount, float winTime){;
		if(pickCount > pastTopCount){
			PlayerPrefs.SetInt (Application.loadedLevelName + "topCount", pickCount);
		}

		if (pastTopTime == 0)
		{
			PlayerPrefs.SetFloat (Application.loadedLevelName + "topTime", winTime);
		}
		else if(winTime < pastTopTime){
			PlayerPrefs.SetFloat (Application.loadedLevelName + "topTime", winTime);
		}
		if(unlockID != ""){
			PlayerPrefs.SetInt (unlockID+"unlock",1);
		}
	}

	public static void GotSecret(){
		if(secretCheck)
			PlayerPrefs.SetInt (Application.loadedLevelName+"secretEarned",1);
		else
			Debug.LogError("Control: secretCheck is false! Forgot to add to database that there's a secret here?");
	}

	void importSaveData(){
		pastTopCount = PlayerPrefs.GetInt (Application.loadedLevelName + "topCount");
		pastTopTime = PlayerPrefs.GetFloat (Application.loadedLevelName + "topTime");
		
		if(pastTopTime != 0){
			int minutes = (int)pastTopTime / 60;
			int seconds = (int)pastTopTime % 60;
			int fraction = (int)(pastTopTime * 100f) % 100; 
			pastTopTimeText= System.String.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		}
		else pastTopTimeText = "N/A";
	}
	
	void importWorldData(string tempWorldName){
		thisWorld = (WorldData)Resources.Load (tempWorldName, typeof(WorldData));
	}

	void importSingleWorldData(string tempWorldName){
		thisWorld = (WorldData)Resources.Load (tempWorldName, typeof(WorldData));
		secretsTotal = thisWorld.totalSecrets;


	}

	void importSingleLevelData(string tempLevelName){
		//thisLevel = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Resources/" +
		//                           	 tempLevelName + ".asset", typeof(LevelData));
		thisLevel = (LevelData)Resources.Load (tempLevelName, typeof(LevelData));
		unlockID = thisLevel.unlockID;
		worldID = thisLevel.worldID;
		levelDesc = thisLevel.levelDesc;

		if(thisLevel.secretCheck){
			secretCheck = true;
			int thisSecretCheck = PlayerPrefs.GetInt (tempLevelName+"secretEarned");
			if(thisSecretCheck == 1){
				secretEarned = true;
			}
			else
				secretEarned = false;
		}
		else
			secretCheck = false;
	}
	
	void GetControlSettings()
	{ // Get saved settings for game play
		
		// The default control scheme
		currentControl = PlayerPrefs.GetInt ("ControlType");
		
		#if UNITY_EDITOR
		currentControl = 1;
		#endif
	
		joystickSize = PlayerPrefs.GetInt("joystickSize");
	//	joystickSize = 100;
		tiltSensitivity = PlayerPrefs.GetInt("tiltSensitivity");
	
	}
	
	public static void ChangeControlSettings(int joystickSizeChange, int tiltSensitivityChange)
	{ // Changes the settings for the joystickSize and tiltSensitivity settings0
		
		ToggleControlsChangeCheck(true); // Indicate that the controls have been changed
		
		// Apply changes real time
		joystickSize = joystickSizeChange;
		tiltSensitivity = tiltSensitivityChange;
		
		// Save Changes
		PlayerPrefs.SetInt("joystickSize", joystickSize);
		PlayerPrefs.SetInt("tiltSensitivity", tiltSensitivity);
	}
	
	public static void ToggleControlsChangeCheck(bool changed)
	{ // Simply for toggling the controls
	
		if(changed)
		{ // If the controls have just been changed, indicate that
			controlsChangeCheck = true;
		}
		else
		{ // Mark that changes have been noticed
			controlsChangeCheck = false;
		}
	}
	
	public static void ToggleBlockAction(bool change)
	{
		// If setting it to true..
		if(change)
		{
			// Set it to true, as commanded
			blockAction = true;
		}
		else
		{
			// Set it to false, as commanded
			blockAction = false;
		}
	}

}
