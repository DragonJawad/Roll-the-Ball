using UnityEngine;
using System.Collections;

public class PlayerBall : MonoBehaviour {
	
	public GUISkin guiSkin;
	GameObject ControlCenter;
	
	public GameObject GUIControl;		// Parent of the GUI Controls
		GameObject controlTilt;				// Control container for tilt
		GameObject controlJoystick;			// Control container for joystick
		GameObject startGUI;			// the starting GUI Textures/buttons, ie controlChanger and settings
		bool saveControlChange = false;				// Save that the controls have changed
	
	public GameObject DeathParticles;	// Particles to be instantiated on death
	public Texture secretItemEarned;	// For drawing texture when secret already earned
	public float speedMultiplier = 1;	// Speed multiplier
	public float jumpSpeed = 5;
	public bool startMsg = true;		// Show the opening message

	// Activate collision check to continuous
	public static bool collisionCheckChange = false;
				
	float collisionChangeTime = 1f;
	float collisionChangeTimer = 0f;
	bool activateCollisionTimer = true;

	int pickCount;						// Count the basic pickups
	float speed = 450f;					// Standard speed
	float distToGround = 0.5f;			// standard distance of ball from ground
	float startTime;					// Time since start of level
	float winTime = 0f;						// How long took to complete level
	float guiTime;
	string winTimeText;					// Win time in text format
	string textTime;					// Time in text format
	bool controlCheck = false;			// Controls are on? (If different from GUI)
	bool pauseCheck = false;			// Flag to pause game
	bool startCheck = false;			// Begin start of game routine?
	bool debugCheck = false;			// Flag for opening Debug menu
	bool winLevel = false;				// Flag to win game
	bool deathRoutineActivate = false;	// Flag for the deathRoutine to activate
	bool deathRoutineCheck = true;		// Flag for checking if already activated
	bool dead = false;					// Flag for death
	bool freezeClock = false;			// Flag for freeze clock effect
	bool groundCheck;					// Check if grounded	
	
	/* Settings Menu variables */
	bool settingsCheck = false;			// Flag for opening the Settings menu
	int joystickSizeSlider = 100;		// Stores the value of the joystickSize's setting slider
	int tiltSensitivitySlider = 100;	// Stores the value of the tiltSensitivity setting slider
	
	/* End of Settings Menu variables */
	
	Vector3 keyInput;	// Input from controls for movement from keyboard
	Vector3 moveInput;	// General input, for storing data from phone
	
	bool tempSecretEarned = false;		// Got the secret temporarily for this round

	/*\ Level Info \*/
	int pickTotal;						// Total of pickups in level
	string levelName;					// Name of the level
	string levelID;						// ID of level | Unused atm

	int pastTopPick;						// Past top pick total
	string bestTimeText;				// Best time in text format
	bool secretEarned = false;			// Check if secret already earned
	
	/*\ GUI Stuff \*/
	int controlType;					// Used for checking control type, tilt or joystick
	// For pause GUI... \\
	Rect pauseBox;						// the overall pause box itself
	float wBPercent = 0.3f;				// proportion of width of screen
	float hBTMargin = 50;				// Box's top margin/y position
	float hBox = 100f;					// height = 25 (for title) + 25 per button
	Rect levelRect						// rect for win or dead gui
	//	= new Rect((Screen.width/2) - Screen.width/4, 0, Screen.width/2, Screen.height);
	//	= new Rect(Screen.width/10, Screen.height/5,
		= new Rect(Screen.width/5,Screen.height/10,
			Screen.width*3/5, Screen.height*4/5);

	void Start () {

		// Get the default custom guiSkin if not attached due to laziness
		if(guiSkin == null)
		{
			guiSkin = (GUISkin)Resources.Load ("GUI/CustomSkin");
		}
		
		// Get the central gameObject if not attached
		if(ControlCenter == null){
			ControlCenter = GameObject.Find("Control Center");
		}

		// Get the GUIControl overall container
		if(GUIControl == null){
			GUIControl = GameObject.Find ("GUIControl");
		}
		
		// Get the ControlChanger to turn it off
		if(startGUI == null){
			startGUI = GameObject.Find ("OpeningGUI");
		}
		
		// These objects are easier found than otherwise...
		controlTilt = GameObject.Find ("TiltControl");
		controlJoystick = GameObject.Find ("JoystickControl");
		
		GetControlType(); // Get the current control type, tilt or joystick
		// Do above FIRST before using GUIToggle

		// If there is a beginning message, deactivate GUIControl
		if(startMsg){
			GUIToggle(false);
		}
		else{
			GUIToggle(true);
		}

		GetLevelData ();	// Get basic level data

		pickCount = 0;		// Make sure pickCount is 0 at start
		startCheck = false;

		// Get the pauseBox GUI dimensions now
		pauseBox.x = (Screen.width*(1-wBPercent))/2;	// Half of percent of screen width
		pauseBox.y = hBTMargin;
		pauseBox.width = Screen.width * wBPercent;
		pauseBox.height = hBox;

	} // end of Start ()

	void Update () {
		/* TEST ZONE*/
		#if UNITY_EDITOR
		
	//	tilt = Input.acceleration;
	//	moveInput = new Vector3(tilt.x, 0, tilt.y);
				
		//Cheat codes
		if (Input.GetKey ("y")) {
			WinLevel();
		}
		
		if (Input.GetKey ("i")) {
			Die();
		}
		
		if(Input.GetKeyDown("f")){
			if(!debugCheck)
				debugCheck = true;
			else
				debugCheck = false;
		}
		
		// Toggle the control mode
		if(Input.GetKeyDown ("r"))
		{
			if(controlType == 0){
				print("Tilt control type!");
				controlTilt.SetActive(true);
				controlJoystick.SetActive(false);
				controlType = 1;
			}
			else{
				print("Control type: Joystick!");
				controlTilt.SetActive(false);
				controlJoystick.SetActive(true);
				controlType = 0;
			}
		}
		#endif
		/* END TZONE*/
		
		//Check for death
		if (transform.position.y <= -1 || Input.GetKey("t")) {
			renderer.enabled = false;
			rigidbody.useGravity = false;
			rigidbody.velocity = Vector3.zero;
			StartCoroutine(DieDelay(1));
			deathRoutineActivate = true;
		}

		if(deathRoutineCheck && deathRoutineActivate){
			deathRoutineCheck = false;
			Instantiate(DeathParticles,this.transform.position, Quaternion.identity);
		}

		//Check if setting CollisionCheck to Conitnuous
		if (collisionCheckChange) {
			if(activateCollisionTimer){
				activateCollisionTimer = false;
				collisionChangeTimer = Time.time + collisionChangeTime;
			}
			if(Time.time < collisionChangeTimer)
			{
				rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
			else{
				// Reset settings for future use
				collisionCheckChange = false;	// Reset so it doesn't run again
				activateCollisionTimer = true;  // Reset for future use
				rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
			}
		}

		if (Input.GetButtonDown("Pause")){
			ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
			PauseToggle();
		}

		if(controlCheck) // If you can control the ball still
		{	
			rigidbody.drag = 0.45f;
				
			#if UNITY_EDITOR
				keyInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			#endif


			if (Input.GetButton ("Jump")) {
				Jump();
			}
		}
		else {	// Make sure the ball can't move
	//		moveInput = Vector3.zero;
			rigidbody.velocity = Vector3.zero;
			rigidbody.useGravity = false;
		}
	} // end of Update()

	
	void FixedUpdate(){
		
		// If there's actually any input...
		if(moveInput != Vector3.zero)
		{
			// Move the ball with that input
			MoveBall(moveInput);
		}
		#if UNITY_EDITOR
		else
		{
			MoveBall(keyInput);
		}
		#endif
		
	} // end of FixedUpdate()
	
	
	void OnTriggerEnter(Collider hit){	// when entering an object with a Trigger...
		
		TypeDefinition typeDefinitionComponent = hit.gameObject.GetComponent<TypeDefinition> ();
	//	ObjectTypes ObjectType = TypeDefinition.ObjectTypes;
		if (typeDefinitionComponent != null){
		switch(typeDefinitionComponent.typeOfObject){
			case TypeDefinition.ObjectTypes.Pickup: // if it is a common pickup
				ControlCenter.GetComponent<Control_MusicManager>().PickupSound();
				pickCount++;						// Increase total pickups counted by one
				hit.gameObject.SetActive(false);	// Deactivate pickup to "pick up"
			break;
			
			case TypeDefinition.ObjectTypes.Exit:	// if it is the exit
				pickCount++;						// still counts as a pickup
				freezeClock = true;					// Freeze clock for effect
				winTime = guiTime;
				winTimeText = textTime;				// Set winTimeText to now, since delay is here
				StartCoroutine(WinLevelDelay(1));	// WinLevel stuff in one second for effect
				hit.gameObject.SetActive(false);	// Deactivate pickup to "pick up"
			break;

			case TypeDefinition.ObjectTypes.SecretItem:	// if it is a secret item
				ControlCenter.GetComponent<Control_MusicManager>().SecretSound();
				pickCount++;
				tempSecretEarned = true;
				hit.gameObject.SetActive(false);	// Deactivate pickup to "pick up"
			break;
				
			case TypeDefinition.ObjectTypes.Button:
			//	ControlCenter.GetComponent<Control_MusicManager>().ButtonSound();
				hit.transform.parent.parent.GetComponent<Control_Button>().Activate();
			break;

			case TypeDefinition.ObjectTypes.Accelerator:
				hit.transform.GetComponent<Control_Accel>().Accelerate();
			break;
			
			case TypeDefinition.ObjectTypes.InstantBoost:
				ControlCenter.GetComponent<Control_MusicManager>().BoosterSound();
				hit.transform.GetComponent<Control_Boost>().Boost();
			break;
			
			case TypeDefinition.ObjectTypes.JumpBoost:
				ControlCenter.GetComponent<Control_MusicManager>().JBoosterSound();
				hit.transform.GetComponent<Control_JBoost>().Fly();
			break;

			case TypeDefinition.ObjectTypes.BouncePlatform:
				PerfectBounce();
			break;

			case TypeDefinition.ObjectTypes.BounceWall:
				PerfectBounceWall();
			break;
		} // close switch
		} // close if != null
	} // close OnTriggerEnter

	void OnGUI(){
		GUI.skin = guiSkin; // Assign the customSkin for the game
		GUI.skin.box.fontSize = Screen.width/30; // Dynamically change the szie of the font for boxes
		GUI.skin.button.fontSize = Screen.width/30; // Dynamicaly change the size of the font for buttons
		// Change the font size of the TextBox custom style
		GUI.skin.customStyles[1].fontSize = Screen.width/34;
		
		
		if (debugCheck){
			GUI.BeginGroup(levelRect);
			
			GUI.Box (new Rect(0,0,250,250), "The magical Debug menu!");

			if(GUI.Button (new Rect(50,125,150,25), "Unlock World 2")){
				print ("World2 Unlocked");
				PlayerPrefs.SetInt("world2State", 1);
			}
			
			if(GUI.Button (new Rect(50,150,150,25), "Toggle freeze")){
				if(Time.timeScale == 1)
					Time.timeScale = 0;
				else
					Time.timeScale = 1;
			}
			if(GUI.Button (new Rect(50,175,150,25), "Delete PPrefs")){
				print ("Deleted! =O");
				PlayerPrefs.DeleteAll();
			}
			if(GUI.Button (new Rect(50,200,150,25), "Replay NAOW!")){
				Restart ();
			}
			
			GUI.EndGroup();
		}
		
		else if (settingsCheck){
			Rect tempRect						// rect for win or dead gui
				//	= new Rect((Screen.width/2) - Screen.width/4, 0, Screen.width/2, Screen.height);
				//	= new Rect(Screen.width/10, Screen.height/5,
				= new Rect(Screen.width*1/10,Screen.height*1/20,
				           Screen.width*4/5, Screen.height*9/10);
			GUI.BeginGroup(tempRect);
			
			GUI.Box (new Rect(0,0,Screen.width*4/5,Screen.height*9/10), "", "BoxTitle");
			GUI.Box(new Rect(Screen.width*1/10,0,
			                 Screen.width*6/10,Screen.height*1/10),"Settings");
			
			// JoysticksSize label box
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*1/10,
			                 Screen.width*6/20,Screen.height*1/10),"Joystick Size");
			// Box for showing the percent on the slider
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*2/10,
			                 Screen.width*1/10,Screen.height*1/10),joystickSizeSlider+"%");
			// Actual slider for the joystickSize setting
			Rect slideBox1 = new Rect(Screen.width*5/20,Screen.height*9/40,Screen.width*2/5,Screen.height/10);
			joystickSizeSlider = Mathf.RoundToInt(GUI.HorizontalSlider (slideBox1, joystickSizeSlider, 50, 150));
			
			// Tilt Sensitivity label box
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*3/10,
			                 Screen.width*6/20,Screen.height*1/10),"Tilt Sensitivity");
			// Box for showing the percent on the slider
			GUI.Box(new Rect(Screen.width*1/10,Screen.height*4/10,
			                 Screen.width*1/10,Screen.height*1/10),tiltSensitivitySlider+"%");
			// Actual slider for the tiltSensitivity setting
			Rect slideBox2 = new Rect(Screen.width*5/20,Screen.height*17/40,Screen.width*2/5,Screen.height/10);
			tiltSensitivitySlider = Mathf.RoundToInt(GUI.HorizontalSlider (slideBox2, tiltSensitivitySlider, 20, 200));
			
			// Default button (make all the settings the default values)
			if(GUI.Button (new Rect(Screen.width*1/10,Screen.height*11/20,
			                        Screen.width*2/10, Screen.height*1/10), "Defaults")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				joystickSizeSlider = 100;
				tiltSensitivitySlider = 100;
			}
			
			// Cancel Button (don't save changes)
			if(GUI.Button (new Rect(Screen.width*3/20,Screen.height*7/10,
			                        Screen.width*1/5, Screen.height*2/10), "Cancel")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				
				// Close the Settings menu
				settingsCheck = false;
				// Go back to the opening message and GUIs
				OpeningToggle(true);
			}
			// Save changes and close the Settings menu
			if(GUI.Button (new Rect(Screen.width*9/20,Screen.height*7/10,
			                        Screen.width*1/5, Screen.height*2/10), "Save")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				
				// Close the Settings menu
				settingsCheck = false;
				// Go back to the opening message and GUIs
				OpeningToggle(true);
				
				// Apply and save changes
				Control.ChangeControlSettings(joystickSizeSlider, tiltSensitivitySlider);
			}
			
			GUI.EndGroup();
		}

		else if (pauseCheck) { // Check if game is paused;

			GUI.BeginGroup(levelRect);

	//		GUI.Box(pauseBox, "This is a paused box!");
			GUI.Box(new Rect(Screen.width/10,Screen.height*1/10,Screen.width*4/10,Screen.height*3/5),"", "BoxTitle");
			GUI.Box(new Rect(Screen.width*3/20,Screen.height*1/10,
			                 Screen.width*3/10,Screen.height*1/10),"Paused!");

	//		Rect nameBox = new Rect(Screen.width/10,0, Screen.width*2/5, Screen.height/10);		
			
			//Unpause button
			if(GUI.Button (new Rect(Screen.width/10, Screen.height*5/20,
			                        Screen.width*4/10,Screen.height*1/5),"Continue!")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				PauseToggle();
			}

			if(GUI.Button (new Rect(Screen.width/10, Screen.height*5/10,
			                        Screen.width*4/10,Screen.height*1/5),"World Menu")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				Application.LoadLevel(Control.worldID);
			//	print ("Commented it all out for now!");
			}

			/*
			//Quit button, to become Return to Menu button later
			if(GUI.Button(new Rect(pauseBox.x, pauseBox.y+75, pauseBox.width, 25), "Main Menu" )){
				Application.LoadLevel ("MainMenu");
			}
			*/
			GUI.EndGroup();
		}

		else if (startMsg && !startCheck) {
		
		Rect msgRect
			= new Rect(Screen.width/5,Screen.height*1/20,
			           Screen.width*3/5, Screen.height*9/10);
			
			if(secretEarned){
				GUI.DrawTexture(new Rect(Screen.width/5-Screen.height/10,
				                         Screen.height*2/10-Screen.height/10,
				                         Screen.height/5,Screen.height/5),
				                secretItemEarned, ScaleMode.ScaleToFit);
			}

			GUI.BeginGroup(msgRect);
			
			GUI.Box (new Rect(0,0,Screen.width*3/5,Screen.height*9/10), "", "BoxTitle");

			Rect nameBox = new Rect(Screen.width/10,0, Screen.width*2/5, Screen.height/10);
			GUI.Box(nameBox, levelName);

			GUI.skin.box.fontSize = Screen.width/35;
			GUI.skin.button.fontSize = Screen.width/30;
			
			GUI.Box (new Rect(Screen.width*3/40,Screen.height/10,Screen.width*2/10,Screen.height/10),
			         "High Score: ");

			Rect topPickBox = new Rect(Screen.width*4/35+Screen.width*2/10,
			                           Screen.height/10, Screen.width*2/10,
			                           Screen.height/10);
			if (pastTopPick == 0)
				GUI.Box (topPickBox, "N/A");
			else
				GUI.Box (topPickBox, pastTopPick + "/" + pickTotal);

			GUI.Box (new Rect(Screen.width*3/40,Screen.height*2/10,
			                  Screen.width*2/10,Screen.height/10),
			         "Top Time: ");
			GUI.Box (new Rect(Screen.width*4/35+Screen.width*2/10,Screen.height*2/10,
			                  Screen.width*2/10,Screen.height/10), bestTimeText);

			GUI.Box(new Rect(Screen.width*1/40,Screen.height*7/20,
			                 Screen.width*11/20, Screen.height*3/10), Control.levelDesc, "TextBox");

			if(GUI.Button (new Rect(Screen.width/10,Screen.height*7/10,
			                        Screen.width*2/5, Screen.height*2/10), "Start!")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				StartRoutine();
			}

			GUI.EndGroup();
		}

		else if(winLevel){
		GUI.BeginGroup(levelRect);
			GUI.Box (new Rect(0,0,Screen.width*3/5,Screen.height*4/5), "", "BoxTitle");
			GUI.Box (new Rect(Screen.width*1/5,0,Screen.width*1/5,Screen.height*1/10), "You Won!");

			GUI.Box (new Rect(Screen.width*3/40,Screen.height/10,Screen.width*2/10,Screen.height/10),
			         "Collected: ");
			GUI.Box (new Rect(Screen.width*4/35+Screen.width*2/10,
			                  Screen.height/10, Screen.width*2/10,
			                  Screen.height/10), pickCount + "/" + pickTotal);

			GUI.Box (new Rect(Screen.width*3/40,Screen.height*2/10,
			                  Screen.width*2/10,Screen.height/10), "Time: ");
			GUI.Box (new Rect(Screen.width*4/35+Screen.width*2/10,Screen.height*2/10,
			                  Screen.width*2/10,Screen.height/10), winTimeText);

			if(GUI.Button (new Rect(Screen.width*1/20,Screen.height*3/5,Screen.width*2/10,Screen.height*1/10), "Replay")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				Restart ();
			}

			if(GUI.Button (new Rect(Screen.width*4/10-Screen.width*1/20,
			                        Screen.height*3/5,Screen.width*2/10,Screen.height*1/10), "Next")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				NextLevel();
			}

			if(GUI.Button (new Rect(Screen.width*2/10,
			                        Screen.height*7/10,Screen.width*2/10,Screen.height*1/10), "Menu")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				Application.LoadLevel(Control.worldID);
			}
			
			GUI.EndGroup();
		}

		else if(dead){
			// Insert GUI stuff for when player dies
		GUI.BeginGroup (levelRect);

			GUI.Box (new Rect(0,0,Screen.width*3/5,Screen.height*4/5), "", "BoxTitle");
			GUI.Box (new Rect(Screen.width*1/5,0,Screen.width*1/5,Screen.height*1/10), "You Died");

			GUI.Box (new Rect(Screen.width*3/40,Screen.height/10,Screen.width*2/10,Screen.height/10), "Collected: ");
			GUI.Box (new Rect(Screen.width*4/35+Screen.width*2/10,
			                  Screen.height/10, Screen.width*2/10,
			                  Screen.height/10), pickCount + "/" + pickTotal);
			
			GUI.Box (new Rect(Screen.width*3/40,Screen.height*2/10,
			                  Screen.width*2/10,Screen.height/10), "Time: ");
			GUI.Box (new Rect(Screen.width*4/35+Screen.width*2/10,Screen.height*2/10,
			                  Screen.width*2/10,Screen.height/10), winTimeText);

			if(GUI.Button (new Rect(Screen.width*1/10,Screen.height*3/5,
			                        Screen.width*4/10,Screen.height*1/10), "Replay")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				Restart ();
			}
			if(GUI.Button (new Rect(Screen.width*2/10,
			                        Screen.height*7/10,Screen.width*2/10,Screen.height*1/10), "Menu")){
				ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
				Application.LoadLevel(Control.worldID);
			}
			
			GUI.EndGroup();

		}

		else{		// Normal GUI stuff
			GUI.Box (new Rect (Screen.height/8, 0, Screen.width/5, Screen.height*1/10),
			         "Pickups: " + pickCount + "/" + pickTotal);

			if(freezeClock) textTime = winTimeText;
			else{
				guiTime = Time.time - startTime;
				int minutes = (int)guiTime / 60;
				int seconds = (int)guiTime % 60;
				int fraction = (int)((guiTime * 100f) % 100);  
				textTime = System.String.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
			}
			
			GUI.Box (new Rect (Screen.width-Screen.height/8-Screen.width/5, 
			                   0, Screen.width/5, Screen.height*1/10), textTime);
		}

	}	// end of OnGUI() 

	void GetLevelData(){		// Get all necessary level data
		// Level meta
		pickTotal = ControlCenter.GetComponent<Control>().thisLevel.totalPickups;
		levelName = ControlCenter.GetComponent<Control>().thisLevel.levelName;

		// Level saved/loaded data
		bestTimeText = Control.pastTopTimeText;
		pastTopPick = Control.pastTopCount;
		secretEarned = Control.secretEarned;
	}
	
	// For determining type of control of game (tilt or joystick)
	void GetControlType(){
		controlType = Control.currentControl;
		
		// If the control type is 0, then it's tilt
		if(controlType == 0){
			controlTilt.SetActive(true);
			controlJoystick.SetActive(false);
		}
		else{
			controlTilt.SetActive(false);
			controlJoystick.SetActive(true);
		}
	}
	
	void SaveControlChange(){
		saveControlChange = false;	// Just in case of issues from replaying
		PlayerPrefs.SetInt("ControlType", controlType);
	}
	
	public void ToggleControlType(int changeToType){
		controlType = changeToType;
		saveControlChange = true;
	}

	public bool isGrounded(){	// Check if in contact with ground
		groundCheck = Physics.Raycast (transform.position, -Vector3.up, distToGround);
		return groundCheck;
	}
	
	// Get input from other sources
	public void GetMoveInput(Vector3 moveAmount)
	{
		// Save the amount for moving the ball
		moveInput = moveAmount;
	}
	
	// Actually move the ball
	public void MoveBall(Vector3 moveAmount){
		if(moveAmount == Vector3.zero) return;
		rigidbody.AddForce(moveAmount*speed*speedMultiplier*Time.deltaTime);
	}
	
	// Do jumping action
	public void Jump(){
		if(isGrounded()){
			rigidbody.velocity = new Vector3(rigidbody.velocity.x,jumpSpeed,
		                                 rigidbody.velocity.z);
		}
	}
	
	public void OpeningToggle(bool activate)
	{ // Turns off the opening message and GUIs off
		
		if(activate)
		{ // If activating the opening message and GUIs...
			
			// Turn on the opening message
			startMsg = true;
			// Turn on the startGUIs
			startGUI.SetActive(true);
		}
		else
		{ // Turn off the opening message and GUIs
			
			// Turn off the opening message
			startMsg = false;
			// Turn off the startGUIs
			startGUI.SetActive(false);
		}
	}
	
	public void GUIToggle(bool activate){
		if(activate)
		{
			GUIControl.SetActive(true);
			if(controlType == 0) // Default: tilt
			{
				controlTilt.SetActive (true);
			}
			else if(controlType == 1){ // Joystick
				controlJoystick.SetActive(true);
			}
		}
		else
		{
			GUIControl.SetActive(false);
			if(controlType == 0) // Default: tilt
			{
				controlTilt.SetActive (false);
			}
			else if(controlType == 1){ // Joystick
				controlJoystick.SetActive(false);
			}
		}
	}

	public void PerfectBounce(){
		rigidbody.velocity = new Vector3(rigidbody.velocity.x,-rigidbody.velocity.y*1.6f,
		                                 rigidbody.velocity.z);
	}

	public void PerfectBounceWall(){
		rigidbody.velocity = new Vector3(-rigidbody.velocity.x,rigidbody.velocity.y,
		                                 -rigidbody.velocity.z);
	}

	public void StartRoutine(){ // Set the starting bools and such to the correct values to start the game
		startCheck = false;
		startTime = Time.time;				// Set the starting time of the timer to NOW
		OpeningToggle(false);				// Turn off the opening message screen
		controlCheck = true;				// Allow the player and such to be moved
		GUIToggle(true);
		rigidbody.useGravity = true;
		
		// Save that the default controls have changed
		if(saveControlChange){
			SaveControlChange();
		}
	}

	public void PauseToggle(){
		if(!pauseCheck){
			pauseCheck = true;			// Game is now paused
			Time.timeScale = 0;			// Officially pause the gam
				GUIToggle(false);
		}
		else{
			pauseCheck = false;			// Game is now unpaused
			Time.timeScale = 1;			// Officially unpause the game
			if(!startMsg)
				GUIToggle(true);
		}
	}

	IEnumerator DieDelay(float deathDelay){		// The delayed version of Die()
		ControlCenter.GetComponent<Control_MusicManager>().DeathBell();
		yield return new WaitForSeconds(deathDelay);
		Die ();
	}

	public void Die(){	// For when death comes upon thee
		dead = true;
		GUIToggle(false);
		if(winTimeText == null) winTimeText = textTime;
	//	Time.timeScale = 0;
	//	Application.LoadLevel (Application.loadedLevel);
	}

	public void Restart(){	// For restarting the game
		AutoFade.LoadLevel(Application.loadedLevelName,0.25f,Color.black,true);
	//	Application.LoadLevel (Application.loadedLevel);	
	}

	public void NextLevel(){
		if(Control.unlockID == "")
			AutoFade.LoadLevel(Control.worldID,0.25f,Color.black,true);
		else
			AutoFade.LoadLevel(Control.unlockID,0.25f,Color.black,true);
	}

	public IEnumerator WinLevelDelay(float winDelay){ // Delay winning the game for effectual reasons
		ControlCenter.GetComponent<Control_MusicManager>().WinSound();
		yield return new WaitForSeconds(winDelay);
		WinLevel ();
	}

	public void WinLevel(){ // Placeholder for winning the game stuff
		controlCheck = false;
		winLevel = true;
		GUIToggle(false);
		if(winTime == 0)
			winTime = guiTime;
		if(winTimeText == null)
			winTimeText = textTime;

		if(tempSecretEarned)
			Control.GotSecret();

		Control.WinLevel(pickCount, winTime); // Send current win stats to Control for processing
	}
	
	public void OpenSettings()
	{
		/*\ Open the settings menu mid-game
		 * 1) Effectually pause the game, if not paused (MOST LIKELY unneccessary)
		 * 2) Set the bool for the Settings GUI to open (it trumps all the rest)
		 *
		 * For GUI itself: (doesn't go here, but good place for planning)
		 * -> First get/check the current settings (cache them)
		 *
		 * 1) Tilt Sensitivity slider (indicate in percent)
		 * 2) Joystick Size slider (indicate in percent)
		 * 3) Save and Cancel buttons (Save applies the changes and uses PlayerPrefs.Save() maybe)
		\*/
		
		// Activate the Settings Menu
		settingsCheck = true;
		
		// (Re)set the sliders to the current settings
		joystickSizeSlider = Control.joystickSize;
		if(joystickSizeSlider == 0)
		{ // If joystickSize is unassigned, use default value
			joystickSizeSlider = 100;
		}
		tiltSensitivitySlider = Control.tiltSensitivity;
		if(tiltSensitivitySlider == 0)
		{ // If tiltSensitivitySlider is unassigned, use default value
			tiltSensitivitySlider = 100;
		}
		
		// Turn off the opening screen and buttons
		OpeningToggle(false);
	}

} // end of program
