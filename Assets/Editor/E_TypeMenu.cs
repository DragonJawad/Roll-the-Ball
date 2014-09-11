using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TypeMenu))]
public class E_TypeMenu : Editor 
{
	// Flag variables for folds
	bool showDefault = false;	// Show default Inspector?
	bool showAttachments = false; // Show the needed attachment fields
	
	public override void OnInspectorGUI()
	{
		// Get the current script and its values
		TypeMenu myTarget = (TypeMenu) target;
		// Get the current type of Terrain
		myTarget.thisMenu = (TypeMenu.MenuType)EditorGUILayout.EnumPopup(
			"Terrain Type: ", myTarget.thisMenu);
		// Check if there is a special mode
		myTarget.specialMode = EditorGUILayout.IntSlider("Special Mode",myTarget.specialMode, -1, 3);
		
		EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		
		if(myTarget.specialMode == 0)
		{ // If specialMode type 0
			if(TypeMenu.MenuType.GUIWindow == myTarget.thisMenu)
			{ // If GUIMenu (WorldName indicator)
				EditorGUILayout.HelpBox("World Name Indicator! Used for showing off the world name and opening world description!",
				                        MessageType.Info);
				myTarget.textName = EditorGUILayout.ObjectField("TextMesh: Name", myTarget.textName,
				                                                typeof(TextMesh), true) as TextMesh;
				myTarget.worldID = EditorGUILayout.TextField("World ID", myTarget.worldID, GUILayout.MaxWidth(150));
			}
		} // End special mode 0
		else if(myTarget.specialMode == 1)
		{ // If specialMode type 1
			if(TypeMenu.MenuType.GUIWindow == myTarget.thisMenu)
			{ // If GUIMenu (ColletionIndicator)	
				EditorGUILayout.HelpBox("Collection Indicator! Show how many items collected in this world!",
				                        MessageType.Info);
				myTarget.textName = EditorGUILayout.ObjectField("TextMesh: Name", myTarget.textName,
				                                                typeof(TextMesh), true) as TextMesh;
				myTarget.worldID = EditorGUILayout.TextField("World ID", myTarget.worldID, GUILayout.MaxWidth(150));
			}
		} // End special mode 1
		else if(TypeMenu.MenuType.MainSelect == myTarget.thisMenu)
		{
			EditorGUILayout.HelpBox("General scene selection code, ie for Back buttons and such",
			                        MessageType.Info);
			myTarget.worldDestination = EditorGUILayout.TextField("World Destination", myTarget.worldDestination);
		} // End if MainSelect
		else if(TypeMenu.MenuType.WorldSelect == myTarget.thisMenu)
		{
			// Show the necessary attachments fields
			showAttachments = EditorGUILayout.Foldout (showAttachments, "Show Needed Attachments");
			if(showAttachments){
				myTarget.textName = EditorGUILayout.ObjectField("TextMesh: Name", myTarget.textName,
				                                              typeof(TextMesh), true) as TextMesh;
				myTarget.unlockIndicator = EditorGUILayout.ObjectField("TM: Need Indicator", myTarget.unlockIndicator,
				                                                   typeof(TextMesh), true) as TextMesh;
				myTarget.lockObject = EditorGUILayout.ObjectField("Lock Object", myTarget.lockObject,
				                                                  typeof(GameObject), true) as GameObject;
				
				EditorGUILayout.LabelField("----"); // Spacer/Separator
				
			} // end showAttachments
			
			myTarget.worldID = EditorGUILayout.TextField("World ID", myTarget.worldID, GUILayout.MaxWidth(150));
			myTarget.worldDestination = EditorGUILayout.TextField("World Destination", myTarget.worldDestination,
			                                                      GUILayout.MaxWidth(170));
		} // End if WorldSelect
			else if(TypeMenu.MenuType.LevelSelect == myTarget.thisMenu)
		{
			// Show the necessary attachments fields
			showAttachments = EditorGUILayout.Foldout (showAttachments, "Show Needed Attachments");
			if(showAttachments){
				myTarget.textID = EditorGUILayout.ObjectField("TextMesh: Name", myTarget.textID,
					typeof(TextMesh), true) as TextMesh;
				myTarget.textPickups = EditorGUILayout.ObjectField("TextMesh: Pickups", myTarget.textPickups,
				                                              typeof(TextMesh), true) as TextMesh;
				myTarget.textTopTime = EditorGUILayout.ObjectField("TextMesh: Top Time", myTarget.textTopTime,
				                                              typeof(TextMesh), true) as TextMesh;
				myTarget.lockObject = EditorGUILayout.ObjectField("Lock Object", myTarget.lockObject,
															typeof(GameObject), true) as GameObject;
															
				EditorGUILayout.LabelField("----"); // Spacer/Separator
				
			} // end showAttachments
			
			EditorGUILayout.LabelField("-----------"); // Spacer/Separator
			
			myTarget.worldID = EditorGUILayout.TextField("World ID", myTarget.worldID, GUILayout.MaxWidth(150));
			myTarget.levelID = EditorGUILayout.TextField("World ID", myTarget.levelID, GUILayout.MaxWidth(150));
			myTarget.worldDestination = EditorGUILayout.TextField("World Destination", myTarget.worldDestination,
							 GUILayout.MaxWidth(170));
		} // End if MenuType is LevelSelect
		
		else if(TypeMenu.MenuType.GUIWindow == myTarget.thisMenu)
		{ // A menu dud, basically
			EditorGUILayout.HelpBox("Hallo! This is just as a placeholder so the MenuControl can interact with this object!",
									MessageType.Info);
		} // endif MenuType is GUIWindow (dud)
		
		else if(TypeMenu.MenuType.CamTransition == myTarget.thisMenu)
		{
			EditorGUILayout.HelpBox("Input the ID of where the camera should move to",
			                        MessageType.Info);
			myTarget.worldDestination = EditorGUILayout.TextField("CameraPos ID", myTarget.worldDestination,
			                                                      GUILayout.MaxWidth(145));
			
		} // End if MenuType is CamTransition
		EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		
		// Code for showing the default Inspector
		showDefault = EditorGUILayout.Foldout(showDefault, "Show Default Inspector");
		if(showDefault)
		{
			DrawDefaultInspector();
		}
	}
}