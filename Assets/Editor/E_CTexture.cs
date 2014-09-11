using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Control_Texture))]
public class E_CTexture : Editor 
{
	// Flag variable for folds
	bool showDefault = false;	// Show default Inspector?

	public override void OnInspectorGUI()
	{		
		// Get the current script and its values
		Control_Texture myTarget = (Control_Texture) target;
		
		// Get the current type of Terrain
		myTarget.thisTexture = (Control_Texture.TextureType)EditorGUILayout.EnumPopup(
			"Texture Type: ", myTarget.thisTexture);
		// showGUI toggle
		myTarget.showGUI = EditorGUILayout.Toggle ("Show the GUI?", myTarget.showGUI);
		
		// Settings if this is a Return
		if(myTarget.thisTexture == Control_Texture.TextureType.WorldPanL || 
				myTarget.thisTexture == Control_Texture.TextureType.WorldPanR)
		{
			EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		
			myTarget.Cam = EditorGUILayout.ObjectField("Camera", myTarget.Cam,
			                                           typeof(GameObject), true) as GameObject;
			myTarget.destinationCheck = EditorGUILayout.Toggle ("Show the GUI?", myTarget.destinationCheck);
		}
		
		EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		
		// Code for showing the default Inspector
		showDefault = EditorGUILayout.Foldout(showDefault, "Show Default Inspector");
		if(showDefault)
		{
			DrawDefaultInspector();
		}
	}
}