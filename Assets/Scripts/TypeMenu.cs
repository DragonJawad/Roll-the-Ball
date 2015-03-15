using UnityEngine;
using System.Collections;

public class TypeMenu : MonoBehaviour {

	public enum MenuType{
		None,
		MainSelect,
		WorldSelect,
		LevelSelect,
		SpecialLevel,
		GUIWindow,
		CamTransition,
		Other
	}

	public MenuType thisMenu;	// Classify this object
	public int specialMode = -1;// Special changes depending on MenuType 
	public GameObject ControlCenter;
	public TextMesh textName;	// 3DText of world's name

	/*\ World Menu Variables \*/
	public string worldID;		// For world and level select
	public string worldDestination;	// May be a transform later
	public int aTotal;
	bool animateUnlock;

	/*\ World Select Variables |*/
	public bool nowUnlock;		// Check if it is NOW unlocked
	public TextMesh unlockIndicator;	// For showing unlock requirement
	WorldData thisWorld;
	int unlockNeeded;			// Total needed for unlock
	string worldName;			// Get world name
	bool unlockOverride = false;

	// Also use above for special levels

	/*\ Level Select Variables  \*/
	public string levelID;		// Insert ID of level, for getting info
	public TextMesh textID;		// 3DText to display levelID
	public TextMesh textPickups; // 3DText to display topCount/totalPickups
	public TextMesh textTopTime; // 3DText to display best time achieved in level
	public GameObject lockObject; // The lock object
	LevelData thisLevel;
	int totalPickups;			// Get total pickups in level
	float topTime;				// Get best time level done in
	int topCount;				// Get top collected 
	public bool unlockCheck = false;	// Check if unlocked

	// Special level variables
	int totalSecrets;			// Get total number of secrets
	int collectedSecrets;		// Calculate how many secrets collected

	// Special variables
	int totalLevels;

	void Start(){
		if(specialMode == -1){
		if (thisMenu == MenuType.WorldSelect){
			/*\ WorldSelect notes (for selecting the world)
			 *  1) Get WorldData from WorldID
			 *  2) Get WorldName and unlockRequirement in local variables
			 *  3) Check if Control.aTotal is >= unlockRequirement
			 *  4) If above is true, world is unlocked. If false, locked
			 * 
			 * 	Unlocked:
			 *  1) Lock symbol is inactive
			 *  2) Can click on the World
			 * 
			 *  Locked:
			 *  1) Lock symbol is active
			 *  2) Can't click on the World
			 *  3) Will give message of needing to collect more items
			 * 
			\*/
			thisWorld = (WorldData)Resources.Load (worldID, typeof(WorldData));
			worldName = thisWorld.fileName;
			textName.text = worldName;
			// Check if this lock/unlock has an override | Deprecated?
			unlockOverride = thisWorld.unlockOverride;
			// Check how much is needed to unlock and output it to screen
			unlockNeeded = thisWorld.unlockRequirement;
			unlockIndicator.text = unlockNeeded.ToString();
			
			if( Control.aTotal >= unlockNeeded){
				unlockCheck = true;
			}
			else
				unlockCheck = false;

			/* Deprecated
			if(!unlockOverride){
				int unlockInt = PlayerPrefs.GetInt (worldID+"unlock");
				if (unlockInt == 1){
					unlockCheck = true;
				}
				else
					unlockCheck = false;
			}
			*/
			if(unlockOverride){
				unlockCheck = true;
			}

			if(unlockCheck){
				lockObject.SetActive(false);
			}
			else{
				lockObject.SetActive(true);
			}
			
		} // end if this is MenuSelect

		else if(thisMenu == MenuType.LevelSelect){ // If this is a level select object
			/*| Todo
			 *  1) Put together level id and world id
			 *  2) Get LevelData from asset(get entire level asset)
			 * 		- Unload the asset after getting data
			 * 	 Check if unlocked or locked, if unlocked do 3-6
			 *  3) Get totalPickups from LevelData
			 *  4) Get topTime from playerPrefs
			 *  5) Get topCount from playerPrefs
			 *  6) Change 3D text
			 * 
			 * 	 - If locked:
			 *  1) Instantiate lock at the top right
			 *  2) Check if nowUnlock = true
			 * 		if true, do unlock animation then do the above steps
			 *  3) "Deactivate" clicking, maybe shake it and do a sound?
			\*/

			thisLevel = (LevelData)Resources.Load (worldID+levelID, typeof(LevelData));

			if (levelID != "LS"){
				textID.text = thisLevel.levelID;
				int unlockInt = PlayerPrefs.GetInt(worldID+levelID+"unlock");
				if(unlockInt == 1) unlockCheck = true;
				else if (levelID == "L1") unlockCheck = true;
				else unlockCheck = false;

				if(unlockCheck){
					totalPickups = thisLevel.totalPickups;
					topCount = PlayerPrefs.GetInt(worldID+levelID+"topCount");
					textPickups.text = topCount + "/" + totalPickups;

					topTime = PlayerPrefs.GetFloat(worldID+levelID+"topTime");
					if(topTime != 0){
						textTopTime.text = timeToText(topTime);
					}
					else
						textTopTime.text = "N/A";

					if(thisLevel.secretCheck){
						int secretStatus = PlayerPrefs.GetInt (worldID+levelID+"secretEarned");
						if(secretStatus == 1)
								Control.secretsCounter++;
						}
				}

				else{
					lockObject.SetActive(true);
					textPickups.text = "";
					textTopTime.text = "";
				}
			} // End if != LS
			else if (levelID == "LS"){
				textID.text = "Special";
				if(Control.secretsCounter == Control.secretsTotal){
					unlockCheck = true;
				}
				else{
					unlockCheck = false;
				}
				
				if(unlockCheck){
					totalPickups = thisLevel.totalPickups;
					topCount = PlayerPrefs.GetInt(worldID+levelID+"topCount");
					textPickups.text = topCount + "/" + totalPickups;
					
					topTime = PlayerPrefs.GetFloat(worldID+levelID+"topTime");
					if(topTime != 0){
						textTopTime.text = timeToText(topTime);
					}
					else
						textTopTime.text = "N/A";
				}
				else{
					lockObject.SetActive(true);
					textPickups.text = "";
					textTopTime.text = "";
				}

				
			} // end if == LS
		} // end if LevelSelect
		} // end if specialMode == -1

		else if(specialMode == 0){
			if (thisMenu == MenuType.GUIWindow){
				thisWorld = (WorldData)Resources.Load (worldID, typeof(WorldData));
				worldName = thisWorld.worldName;
				textName.text = worldName;

				this.GetComponent<GUIWindow>().windowName = worldName;
				this.GetComponent<GUIWindow>().message = thisWorld.description;
			}
		}
		else if(specialMode == 1){
			if (thisMenu == MenuType.GUIWindow){
				/*\ Plans for collecting pickups data
				 *  1) Get world ID and world data
				 *  2) Get total number of levels
				 *  3) Loop through all the levels
				 *  4) Get the number of pickups collected
				 *  5) Add to a total
				 *  6) Display collected/total
				\*/
				thisWorld = (WorldData)Resources.Load (worldID, typeof(WorldData));
				totalLevels = thisWorld.totalLevels;

				for(int i = 1; i <= totalLevels; i++){
					thisLevel = (LevelData)Resources.Load (worldID+"L"+i,
					                                       typeof(LevelData));
					topCount += PlayerPrefs.GetInt(worldID+"L"+i+ "topCount");
					totalPickups += thisLevel.totalPickups;
				}
				
					// Then get data from special level
				thisLevel = (LevelData)Resources.Load (worldID+"LS",
				                                       typeof(LevelData));
				topCount += PlayerPrefs.GetInt(worldID+"LS"+ "topCount");
				totalPickups += thisLevel.totalPickups;
				
				textName.text = topCount + "/" + totalPickups;
			}
		}
		else if(specialMode == 2){
			if (thisMenu == MenuType.GUIWindow){
				/*\ Plans for collecting ALL pickups data
				 *  1) Get world ID and world data for CURRENT world ID
				 *  2) Get total number of levels
				 *  3) Loop through all the levels
				 *  4) Get the number of pickups collected
				 *  5) Add to a total
				 *  6) Repeat steps for all worlds
				\*/

				for(int j = 1; j <= aTotal; j++){
					thisWorld = (WorldData)Resources.Load ("W"+j, typeof(WorldData));
					totalLevels = thisWorld.totalLevels;
					
					//Special level's data
					thisLevel = (LevelData)Resources.Load ("W"+j+"LS",
					                                       typeof(LevelData));
					topCount += PlayerPrefs.GetInt("W"+j+"LS"+ "topCount");
					totalPickups += thisLevel.totalPickups;
					
					for(int i = 1; i <= totalLevels; i++){
						thisLevel = (LevelData)Resources.Load ("W"+j+"L"+i,
						                                       typeof(LevelData));
						topCount += PlayerPrefs.GetInt("W"+j+"L"+i+ "topCount");
						totalPickups += thisLevel.totalPickups;
					} // close i/level for loop
				} // close j/world for loop
				textName.text = topCount + "/" + totalPickups;
				Control.aTotal = topCount;
			} // close if menuType GUIWindow
		} // close if SpecialMode == 2
	}  // end Update()

	void OnMouseEnter(){
		if(thisMenu == MenuType.LevelSelect)
			GetComponent<Renderer>().material.color = Color.black;
		else if(thisMenu == MenuType.WorldSelect){
			Color color;
			color = textName.color;
			color.a = 0.2f;
			textName.color = color;
		}
		else if(thisMenu == MenuType.GUIWindow){
			if(specialMode == 0){
				Color color;
				color = textName.color;
				color.a = 0.2f;
				textName.color = color;
			}
		}
		else{
			Color color;
			color = GetComponent<Renderer>().material.color;
			color.a = 0.2f;
			GetComponent<Renderer>().material.color = color;
		}
	}
	
	void OnMouseExit(){
		if(thisMenu == MenuType.LevelSelect)
			GetComponent<Renderer>().material.color = Color.blue;
		else if(thisMenu == MenuType.WorldSelect){
			Color color;
			color = textName.color;
			color.a = 1f;
			textName.color = color;
		}
		else if(thisMenu == MenuType.GUIWindow){
			if(specialMode == -1){
				Color color;
				color = GetComponent<Renderer>().material.color;
				color = Color.white;
				GetComponent<Renderer>().material.color = color;
			}
		}
		else{
		//	renderer.material.color = Color.white;
			Color color;
			color = GetComponent<Renderer>().material.color;
			color.a = 1f;
			GetComponent<Renderer>().material.color = color;
		}
	}

	string timeToText(float time){
		int minutes = (int)time / 60;
		int seconds = (int)time % 60;
		int fraction = (int)(time * 100f) % 100; 
		string timeText = System.String.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);

		return timeText;
	}
}
