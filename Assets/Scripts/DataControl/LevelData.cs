using UnityEngine;
using System.Collections;

public class LevelData : ScriptableObject
{
	public string fileName;
	public string worldID;
	public string levelID;
	public string levelName;
	public string levelDesc;
	public string unlockID;
	public string other;
	public int totalPickups;
	public bool secretCheck;
}