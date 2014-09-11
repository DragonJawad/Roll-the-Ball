using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUITexture))]

public class Joystick : MonoBehaviour {

	public GameObject player;
	private PlayerBall playerScript;

	// A simple class for bounding how far the GUITexture will move
	public class Boundary 
	{
		public Vector2 min = Vector2.zero;
		public Vector2 max = Vector2.zero;
	}

	private Joystick[] joysticks;
	private bool enumeratedJoysticks = false;
	private float tapTimeDelta = 0.3f;

	public bool touchPad;
	public Rect touchZone;
	public Vector2 deadZone = Vector2.zero;
	public bool normalize = false;
	public Vector2 position;
	public int tapCount;

	private int lastFingerId = -1;								// Finger last used for this joystick
	private float tapTimeWindow;							// How much time there is left for a tap to occur
	private Vector2 fingerDownPos;
//	private float fingerDownTime;
//	private float firstDeltaTime = 0.5f;

	private GUITexture gui;								// Joystick graphic
	private Rect defaultRect;								// Default position / extents of the joystick graphic
	public Boundary guiBoundary = new Boundary();			// Boundary for joystick graphic
	private Vector2 guiTouchOffset;						// Offset to apply to touch input
	private Vector2 guiCenter;							// Center of joystick

	// Custom variables
	private float scaledHeight;
	private float scaledWidth;
	private float yPos;
	private float xPos;
	
	float sizeMultiplier = 0f;	// Size multiplier
	
	// Flag for if controls have already been changed
	bool controlsChanged = false;
	
	// Save the Control Center
	private GameObject ControlCenter;
	// Cache the jumpButton
	private GameObject jumpButton;

	void Start()
	{
		if(player == null){
			player = GameObject.Find("Player");
		}
		if(ControlCenter == null)
		{
			ControlCenter = GameObject.Find ("Control Center");
		}
		if(jumpButton == null)
		{ // Get the separate jump button
			jumpButton = GameObject.Find ("JoyJump");
		}
		
		playerScript = player.GetComponent<PlayerBall>();
		
		// Cache this component at startup instead of looking up every frame	
		gui = GetComponent< GUITexture>();
		
		if(!touchPad) // If this is a joystick...
		{
			// Get joystickSize setting and multiplier
			sizeMultiplier = (Control.joystickSize);
			sizeMultiplier = sizeMultiplier/100;
			
			if(sizeMultiplier != 0)
			{ // If sizeMultipler is actually set, use it
				scaledHeight = Screen.height/4 * sizeMultiplier;
			}
			else // If joystickSize is not set, use default size
			{
				scaledHeight = Screen.height/4;
			}	
			
			xPos = scaledHeight;
			yPos = scaledHeight/2;
			gui.pixelInset = new Rect(xPos, yPos, scaledHeight, scaledHeight);
		
		}
		else{
		/* Old	
			scaledWidth = Screen.width/2;
			scaledHeight = Screen.height*1/3;
			xPos = -scaledWidth;
			yPos = 0;
		*/	
		
			scaledWidth = Screen.width/2;
			scaledHeight = Screen.height*7/8;
			xPos = -scaledWidth;
			yPos = 0;
			
			gui.pixelInset = new Rect(xPos, yPos, scaledWidth, scaledHeight);
		}
		
		// Store the default rect for the gui, so we can snap back to it
		defaultRect = gui.pixelInset;	
		
		defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5;
		defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
		
		transform.position = Vector2.zero;
		
		if ( touchPad )
		{
			// If a texture has been assigned, then use the rect ferom the gui as our touchZone
			if ( gui.texture )
				//	touchZone = new Rect(0,0,scaledWidth, scaledHeight);
				touchZone = defaultRect;
		}
		else
		{				
			// This is an offset for touch input to match with the top left
			// corner of the GUI
			guiTouchOffset.x = defaultRect.width * 0.5f;
			guiTouchOffset.y = defaultRect.height * 0.5f;
			
			// Cache the center of the GUI, since it doesn't change
			guiCenter.x = defaultRect.x + guiTouchOffset.x;
			guiCenter.y = defaultRect.y + guiTouchOffset.y;
			
			// Let's build the GUI boundary, so we can clamp joystick movement
			guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
			guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
			guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
			guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
			
			/*
			1) Get guiCenter
			2) Get guiDiameter (guiBoudnary.max.x - min.x)
			3) Set background texture positon as guiCenter
			4) Set size of background texture using guiDiameter
			5) Set pixeloffset using that
			
			Above won't work, use some relative math to NOT make it dependent
			on this script. Need following data (how calculated):
			1) Center of joystick
			2) The min/max of the joystick
			3) Calculating the surrounding box of the joystick
			*/
	//		guiBoundary.min = new Vector2(defaultRect.x - guiTouchOffset.x,
	//		                              defaultRect.x + guiTouchOffset.x);

		}
	}
	
	void Disable()
	{
		gameObject.SetActive(false);
		enumeratedJoysticks = false;
	}
	
	void ResetJoystick()
	{
		// Release the finger control and set the joystick back to the default position
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
		
		if ( touchPad ){
		/*  Deprecated: separate jump button
		//	gui.color.a = 0.10;	
			Color color = gui.color;
			color.a = 0.1f;
			gui.color = color;
		*/
		}	
	}
	
	bool IsFingerDown()
	{
		return (lastFingerId != -1);
	}
	
	void LatchedFinger(int fingerId)
	{
		// If another joystick has latched this finger, then we must release it
		if ( lastFingerId == fingerId )
			ResetJoystick();
	}
	
	void Update()
	{	
		// Check if the control settings have been changed
		if(!touchPad && !controlsChanged && Control.controlsChangeCheck)
		{ // If controls have been recently changed, apply changes
			
			// Indicate that the controls change has been noticed		
			controlsChanged = true;
			
			// Reset the size of the joystick
			ResetJoystickSize();
		}
	
		if ( !enumeratedJoysticks )
		{
			// Collect all joysticks in the game, so we can relay finger latching messages
			//Joystick[] joysticks = Object.FindObjectsOfType( Joystick ) as Joystick[];
	//		Joystick[] joysticks = UnityEngine.Object.FindObjectsOfType( typeof(Joystick) ) as Joystick[];
			enumeratedJoysticks = true;
		}	
		
		int count = Input.touchCount;
		
		// Adjust the tap time window while it still available
		if ( tapTimeWindow > 0 )
			tapTimeWindow -= Time.deltaTime;
		else
			tapCount = 0;
		
		if ( count == 0 )
			ResetJoystick();
		else
		{
			for(int i= 0;i < count; i++)
			{
				Touch touch = Input.GetTouch(i);			
				Vector2 guiTouchPos = touch.position - guiTouchOffset;
				
				bool shouldLatchFinger = false;
				if ( touchPad )
				{				
					if ( touchZone.Contains( touch.position ) )
						shouldLatchFinger = true;
					
				}
				else if ( gui.HitTest( touch.position ) )
				{
					shouldLatchFinger = true;
				}		
				
				// Latch the finger if this is a new touch
				if ( shouldLatchFinger && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
				{
					
					if ( touchPad )
					{	
					/*  Deprecated: Separate jump button
						// Change the alpha for tap effect
						Color color = gui.color;
						color.a = 0.25f;
						gui.color = color;
					*/
					
						// Activate jump
						player.GetComponent<PlayerBall>().Jump ();
						// Change the jump button's alpha
						jumpButton.GetComponent<JumpButton>().ChangeAlpha(0.1f);
												
						lastFingerId = touch.fingerId;
						fingerDownPos = touch.position;
//						fingerDownTime = Time.time;
					}
					
					lastFingerId = touch.fingerId;
					
					// Accumulate taps if it is within the time window
					if ( tapTimeWindow > 0 )
						tapCount++;
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}

				/*	if(!touchPad){
					// Tell other joysticks we've latched this finger
					foreach ( Joystick j in joysticks )
					{
						if ( j != this )
							j.LatchedFinger( touch.fingerId );
					}		
					} */
				}				
				
				if ( lastFingerId == touch.fingerId )
				{	
					// Override the tap count with what the iPhone SDK reports if it is greater
					// This is a workaround, since the iPhone SDK does not currently track taps
					// for multiple touches
					if ( touch.tapCount > tapCount )
						tapCount = touch.tapCount;
					
					if ( touchPad )
					{	
						// For a touchpad, let's just set the position directly based on distance from initial touchdown
						position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
						position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
					}
					else
					{					
						// Change the location of the joystick graphic to match where the touch is
						Rect clamp = new Rect(Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x ),
						                            Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y ),
						                      gui.pixelInset.width, gui.pixelInset.height);
						gui.pixelInset = clamp;
				//		gui.pixelInset.x =  Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
				//		gui.pixelInset.y =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
					}
					
					if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
						ResetJoystick();					
				}			
			}
		}
		
		if ( !touchPad )
		{
			// Get a value between -1 and 1 based on the joystick graphic location
			position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
			position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
			playerScript.GetMoveInput(new Vector3(position.x, 0, position.y));
		}
		
		// Adjust for dead zone	
		var absoluteX = Mathf.Abs( position.x );
		var absoluteY = Mathf.Abs( position.y );
		
		if ( absoluteX < deadZone.x )
		{
			// Report the joystick as being at the center if it is within the dead zone
			position.x = 0;
		}
		else if ( normalize )
		{
			// Rescale the output after taking the dead zone into account
			position.x = Mathf.Sign( position.x ) * ( absoluteX - deadZone.x ) / ( 1 - deadZone.x );
		}
		
		if ( absoluteY < deadZone.y )
		{
			// Report the joystick as being at the center if it is within the dead zone
			position.y = 0;
		}
		else if ( normalize )
		{
			// Rescale the output after taking the dead zone into account
			position.y = Mathf.Sign( position.y ) * ( absoluteY - deadZone.y ) / ( 1 - deadZone.y );
		}
		
		// Make the ball move
		//playerScript.GetMoveInput(new Vector3(position.x, 0, position.y));
		//player.GetComponent<PlayerBall>().GetMoveInput(new Vector3(position.x, 0, position.y));
	}
	public void ResetJoystickSize()
	{ // Function for resetting the joystick size to apply changes
	
		// Get joystickSize setting and multiplier
		sizeMultiplier = (Control.joystickSize);
		sizeMultiplier = sizeMultiplier/100;
		
		if(sizeMultiplier != 0)
		{ // If sizeMultipler is actually set, use it
			scaledHeight = Screen.height/4 * sizeMultiplier;
		}
		else // If joystickSize is not set, use default size
		{
			scaledHeight = Screen.height/4;
		}	
		
		xPos = scaledHeight;
		yPos = scaledHeight/2;
		gui.pixelInset = new Rect(xPos, yPos, scaledHeight, scaledHeight);
	
		defaultRect = gui.pixelInset;	
		
		defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5;
		defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
		
		transform.position = Vector2.zero;
	
		guiTouchOffset.x = defaultRect.width * 0.5f;
		guiTouchOffset.y = defaultRect.height * 0.5f;
		
		// Cache the center of the GUI, since it doesn't change
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
		
		// Let's build the GUI boundary, so we can clamp joystick movement
		guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
		guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
		guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
		guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
	}
}
