#pragma warning disable 0219 // variable assigned but not used.
// above is to ignore the warning that window is assigned but never used
	// Comment it out before building the game

/*\ Note to self:
		 *  1) Create more options
		 *  2) Make more general/expansive to work with multiple types
		 * 	3) Make another function so another file can easily be created
		 * 			without closing the window
		\*/

using UnityEngine;
using UnityEditor;

public class CreateLevelData : EditorWindow {
	string fileName = "";
	string worldID = "";
	string levelID = "";
	string levelName = "";
	string levelDesc = "";
	string unlockID = "";
	string other = "";
	int totalPickups = -1;
	bool secretCheck = false;

	bool groupEnabled;		// Flag for optional field(s)
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Data/Create Level Data")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		CreateLevelData window = (CreateLevelData)EditorWindow.GetWindow (typeof (CreateLevelData));
	}
	
	void OnGUI () {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		fileName = EditorGUILayout.TextField ("Insert file name", fileName);
		worldID = EditorGUILayout.TextField ("Insert the world ID", worldID);
		levelID = EditorGUILayout.TextField ("Insert the level ID", levelID);
		levelName = EditorGUILayout.TextField ("Inner Level Name", levelName);
		levelDesc = EditorGUILayout.TextField ("Inner Level Description", levelDesc);
		unlockID = EditorGUILayout.TextField ("Insert the unlock ID of the next level", unlockID);
		other = EditorGUILayout.TextField ("Any other notes or such?", other);

		totalPickups = EditorGUILayout.IntField ("Insert total pickups in level", totalPickups);
		secretCheck = EditorGUILayout.Toggle ("Secret in level?", secretCheck);

		/*
		// Optional stuff below
		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		levelName = EditorGUILayout.TextField ("Inner Level Name", levelName);
		EditorGUILayout.EndToggleGroup ();
		*/
	
		// Button to finalize the data, create the file, and close the window...
		if(GUILayout.Button("Create!")){
			Create ();
		}
	}

	void Create(){

		LevelData data = ScriptableObject.CreateInstance<LevelData>();
		data.fileName = fileName;
		data.worldID = worldID;
		data.levelID = levelID;
		data.levelName = levelName;	
		data.levelDesc = levelDesc;
		data.unlockID = unlockID;
		data.other = other;
		data.totalPickups = totalPickups;
		data.secretCheck = secretCheck;
		AssetDatabase.CreateAsset(data, "Assets/Resources/" + fileName + ".asset");
		this.Close ();
	}
}