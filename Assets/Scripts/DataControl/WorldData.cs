using UnityEngine;
using System.Collections;

public class WorldData : ScriptableObject
{
	public string fileName;
	public string worldID;
	public string worldName;
	public string description;
	public int unlockRequirement;
	public int totalLevels;
	public int totalSecrets;
	public bool unlockOverride;
}