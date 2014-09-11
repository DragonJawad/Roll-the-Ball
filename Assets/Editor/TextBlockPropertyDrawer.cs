//Goes in an Editor folder
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(TextBlockAttribute))]
public class TextBlockPropertyDrawer : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		//override height to adjust for word wrapping.
		GUIStyle style = new GUIStyle(EditorStyles.textField);
		style.wordWrap = true;

		return Mathf.Clamp(style.CalcHeight(new GUIContent(prop.stringValue), Screen.width - 34) + 16f, 32f, 128f);
	}

	public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) {
		//First we need to look at the property path to see if it is an array
		string path = prop.propertyPath;
		int arrayInd = path.LastIndexOf(".Array");
		bool bIsArray = false;
		if(arrayInd >= 0)
			bIsArray = true;
		
		
		Rect labelRect = position;
		labelRect.height = 16f;
		
		EditorGUI.LabelField(labelRect, label);
		
		if(bIsArray){
			//If we're dealing with an array then we need to get the object and find the array property so we can mess with it
			SerializedObject so = prop.serializedObject;
			string arrayPath = path.Substring(0, arrayInd);
			SerializedProperty arrayProp = so.FindProperty(arrayPath);

			//Next we need to grab the index from the path string
			int indStart = path.IndexOf("[") + 1;
			int indEnd = path.IndexOf("]");
			
			string indString = path.Substring(indStart, indEnd - indStart);
			
			int myIndex = int.Parse(indString);

			//And finaly we place our buttons
			labelRect.x += 80;
			labelRect.width = 75;
			
			if(GUI.Button(labelRect, "Move Up")){
				if(myIndex > 0){
					string temp = arrayProp.GetArrayElementAtIndex(myIndex-1).stringValue;
					arrayProp.GetArrayElementAtIndex(myIndex-1).stringValue = arrayProp.GetArrayElementAtIndex(myIndex).stringValue;
					arrayProp.GetArrayElementAtIndex(myIndex).stringValue = temp;
					
					so.ApplyModifiedProperties();
				}
			}
			
			labelRect.x += 90;
			
			if(GUI.Button(labelRect, "Move Down")){
				if(myIndex < arrayProp.arraySize-1){
					string temp = arrayProp.GetArrayElementAtIndex(myIndex+1).stringValue;
					arrayProp.GetArrayElementAtIndex(myIndex+1).stringValue = arrayProp.GetArrayElementAtIndex(myIndex).stringValue;
					arrayProp.GetArrayElementAtIndex(myIndex).stringValue = temp;
					
					so.ApplyModifiedProperties();
				}
			}
		}

		Rect blockRect = position;
		blockRect.height -= 16f;
		blockRect.y += 16f;
		
		GUIStyle style = new GUIStyle(EditorStyles.textField);
		style.wordWrap = true;
		
		EditorGUI.BeginChangeCheck ();
		string value = EditorGUI.TextArea (blockRect, prop.stringValue, style);
		if (EditorGUI.EndChangeCheck ()){
			prop.stringValue = value;
		}
	}
}
