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

public class CreateWorldData : EditorWindow {
	string fileName = "";
	string worldID = "";
	string worldName = "";
	string description = "";
	int unlockRequirement = 0;
	int totalLevels = -1;
	int totalSecrets = -1;
	bool unlockOverride;

//	bool groupEnabled = false;		// Flag for optional field(s)
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Data/Create World Data")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		CreateWorldData window = (CreateWorldData)EditorWindow.GetWindow (typeof (CreateWorldData));
	}
	
	void OnGUI () {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		fileName = EditorGUILayout.TextField ("Insert file name", fileName);
		worldID = EditorGUILayout.TextField ("Insert the world ID", worldID);
		worldName = EditorGUILayout.TextField ("Insert world name", worldName);
		worldName = EditorGUILayout.TextField ("Insert world description", worldName);

		unlockRequirement = EditorGUILayout.IntField ("How many items needed to unlock?", unlockRequirement);
		totalLevels = EditorGUILayout.IntField ("Insert Total Levels", totalLevels);
		totalSecrets = EditorGUILayout.IntField ("Insert Total Secrets", totalSecrets);

		unlockOverride = EditorGUILayout.Toggle ("Default unlocked level?", unlockOverride);

		// Button to finalize the data, create the file, and close the window...
		if(GUILayout.Button("Create!")){
			Create ();
		}
	}

	void Create(){

		WorldData data = ScriptableObject.CreateInstance<WorldData>();
		data.fileName = fileName;
		data.worldID = worldID;
		data.worldName = worldName;	
		data.description = description;

		data.unlockRequirement = unlockRequirement;
		data.totalLevels = totalLevels;
		data.totalSecrets = totalSecrets;

		data.unlockOverride = unlockOverride;
		AssetDatabase.CreateAsset(data, "Assets/Resources/" + fileName + ".asset");
		this.Close ();
	}
}