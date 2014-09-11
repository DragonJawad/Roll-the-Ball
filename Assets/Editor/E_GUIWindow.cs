using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GUIWindow))]
public class E_GUIWindow : Editor 
{
	// Flag variable for folds
	bool showDefault = false;	// Show default Inspector?
	bool customTextMesh = false;	// Choose something other than this object's TextMesh

	public override void OnInspectorGUI()
	{		
		// Get the current script and its values
		GUIWindow myTarget = (GUIWindow) target;
		
		// Get the current type of Terrain
		myTarget.thisWindow = (GUIWindow.TypeWindow)EditorGUILayout.EnumPopup(
			"Window Type: ", myTarget.thisWindow);
		
		EditorGUILayout.LabelField("-----------"); // Spacer/Separator
		
		// NECESSARY so text wraps around and creates an expanding box
		EditorStyles.textField.wordWrap = true;
		
		// Message to show if GUIWindow hasn't been assigned
		if(myTarget.thisWindow == GUIWindow.TypeWindow.None)
		{	
			EditorGUILayout.HelpBox("This currently does nothing! Absolutely NOTHIN!",
			                        MessageType.Warning);
		}
		
		// Settings if this is a SecretsIndicator window
		else if(myTarget.thisWindow == GUIWindow.TypeWindow.SecretsIndicator)
		{	
			EditorGUILayout.HelpBox("Secrets Indicator! Used for showing information on secrets!",
			                        MessageType.Info);
			
			myTarget.windowName = EditorGUILayout.TextField("Name of Window: ", myTarget.windowName);
			myTarget.message = EditorGUILayout.TextArea("Window Message: ", myTarget.message);
			
		}
		
		// Settings if this is an Info Menu
		else if(myTarget.thisWindow == GUIWindow.TypeWindow.InfoMenu)
		{	
			EditorGUILayout.HelpBox("Used for showing different information!",
			                        MessageType.Info);
			
			// Toggle a non-default Text Mesh or not
			customTextMesh = EditorGUILayout.Foldout(customTextMesh, "Choose Non-Default TextMesh?");
			if(customTextMesh)
			{
				EditorGUILayout.LabelField("TextMesh: Hover Affect");                        
				myTarget.thisText = EditorGUILayout.ObjectField(myTarget.thisText,
				                                                typeof(TextMesh), true) as TextMesh;
			}
			
			EditorGUILayout.LabelField("Window Title");
			myTarget.windowName = EditorGUILayout.TextField(myTarget.windowName);
			EditorGUILayout.LabelField("Message of the Window:");
			myTarget.message = EditorGUILayout.TextArea(myTarget.message);
		}
		
		// Settings if this is a DeleteData window, for deleting all data
		else if(myTarget.thisWindow == GUIWindow.TypeWindow.DeleteData)
		{	
			EditorGUILayout.HelpBox("Window for deleting all data... ='(",
			                        MessageType.Info);
			
			// Toggle a non-default Text Mesh or not
			customTextMesh = EditorGUILayout.Foldout(customTextMesh, "Choose Non-Default TextMesh?");
			if(customTextMesh)
			{
				EditorGUILayout.LabelField("TextMesh: Hover Affect");                        
				myTarget.thisText = EditorGUILayout.ObjectField(myTarget.thisText,
				                                                typeof(TextMesh), true) as TextMesh;
			}
			
			EditorGUILayout.LabelField("Window Title");
			myTarget.windowName = EditorGUILayout.TextField(myTarget.windowName);
			EditorGUILayout.LabelField("Message of the Window:");
			myTarget.message = EditorGUILayout.TextArea(myTarget.message);
		}
		
		// Settings if this is a WorldInner window
		else if(myTarget.thisWindow == GUIWindow.TypeWindow.WorldInner)
		{	
			EditorGUILayout.HelpBox("Used for giving the name and information of the current world",
			                        MessageType.Info);
			
			// Toggle a non-default Text Mesh or not
			customTextMesh = EditorGUILayout.Foldout(customTextMesh, "Choose Non-Default TextMesh?");
			if(customTextMesh)
			{
				EditorGUILayout.LabelField("TextMesh: Hover Affect");                        
				myTarget.thisText = EditorGUILayout.ObjectField(myTarget.thisText,
				                                                typeof(TextMesh), true) as TextMesh;
			}
			
			EditorGUILayout.LabelField("Window Title");
			myTarget.windowName = EditorGUILayout.TextField(myTarget.windowName);
			EditorGUILayout.LabelField("Message of the Window:");
			myTarget.message = EditorGUILayout.TextArea(myTarget.message);
		}
		
		// Settings if this is a Tabbed window
		else if(myTarget.thisWindow == GUIWindow.TypeWindow.Tabular)
		{	
			EditorGUILayout.HelpBox("Used for making tabs",
			                        MessageType.Info);
			
			// Toggle a non-default Text Mesh or not
			customTextMesh = EditorGUILayout.Foldout(customTextMesh, "Choose Non-Default TextMesh?");
			if(customTextMesh)
			{
				EditorGUILayout.LabelField("TextMesh: Hover Affect");                        
				myTarget.thisText = EditorGUILayout.ObjectField(myTarget.thisText,
				                                                typeof(TextMesh), true) as TextMesh;
			}
			
			myTarget.customTabSize = EditorGUILayout.Toggle ("Custom Tab Size?", myTarget.customTabSize);
			if(myTarget.customTabSize) // If using a custom tab size
			{
				myTarget.sizeMultiplier = EditorGUILayout.RectField(myTarget.sizeMultiplier);
				
				// Print input
				if(GUILayout.Button ("Print Input"))
				   {
					Debug.Log("Current Input: " + myTarget.sizeMultiplier);
				}
				
				// Reset the sizeMultiplier to default
				if(GUILayout.Button ("Reset Size Multiplier?"))
				{
					Debug.Log("Default Size: " + myTarget.defaultSize);
					myTarget.sizeMultiplier = myTarget.defaultSize;
				}
			}
		} // end else if tabular
		// Settings if this is the Settings window [main menu]
		else if(myTarget.thisWindow == GUIWindow.TypeWindow.Settings)
		{	
			EditorGUILayout.HelpBox("This is the main menu's setting GUI Window",
			                        MessageType.Info);
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