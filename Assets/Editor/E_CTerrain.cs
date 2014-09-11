using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Control_Terrain))]
public class E_CTerrain : Editor 
{
	public override void OnInspectorGUI()
	{		
		// Get the current script and its values
		Control_Terrain myTarget = (Control_Terrain) target;
		// Get the current type of Terrain
		myTarget.thisTerrain = (Control_Terrain.TerrainType)EditorGUILayout.EnumPopup(
			"Terrain Type: ", myTarget.thisTerrain);
		
		
		
		// Settings if this is a PlatformMover
		if(myTarget.thisTerrain == Control_Terrain.TerrainType.PlatformMover)
		{
			// Change the target's speed
			myTarget.speed = EditorGUILayout.Slider("Move Speed", myTarget.speed, 0, 1);
			
			// Get the two targets for this object
			myTarget.targetA = EditorGUILayout.ObjectField("TargetA (Start)", myTarget.targetA,
				 typeof(Transform), true) as Transform;
			myTarget.targetB = EditorGUILayout.ObjectField("TargetB (End)", myTarget.targetB,
				 typeof(Transform), true) as Transform;
			
		}
	}
}