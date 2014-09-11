using UnityEngine;
using System.Collections;

public class Control_Tilt : MonoBehaviour {
	
	public enum ControlType{
		None,
		Tilt,
		Indicator_BG,
		Indicator_Dot,
		Jump
	}
	
	// What type of object is this?
	public ControlType thisControl;
	
	// Flag for if controls have already been changed
	bool controlsChanged = false;
	
	// For accessing MoveBall()
	public GameObject player;			// Get the player itself
	private PlayerBall playerScript;	// Get the playerScript on the player
	
	/*\ Jump-only Variables \*/
	public bool jumpTexture = false;	// Use the jump texture?
	GUITexture gui;
	float scaledHeight, scaledWidth, xPos, yPos;
	
	// Touching variables
	Rect defaultRect;					// defaultRect of the touchPad
	Rect touchZone;						// Rect of touchZone of touchPad
	int tapCount;
	private int lastFingerId = -1;		// Finger last used for this joystick
	private float tapTimeWindow;		// How much time there is left for a tap to occur
//	private Vector2 fingerDownPos;
	
	/*\ Tilt-only Variables \*/
	Vector2 tilt;						// Tilt input
	Vector2 normalTilt;					// Normalized tilt
	float normalPercent = 0.5f;			// In percent, the limit of the tilt
	
	/*\ Indicator_Dot only variables \*/
	Rect offsetRect;					// For calculating and applying the offset of the dot
	float radius;						// The radius of the entire indicator
	
	// Use this for initialization
	void Start () {
		if(player == null){
			player = GameObject.Find ("Player");
		}
		playerScript = player.GetComponent<PlayerBall>();
		
		// Get the NormalPercent tilt multiplier
		GetNormalPercent();
		
		// If this is the background for the tilt indicator...
		if(thisControl == ControlType.Indicator_BG)
		{
			// get the GUITexture
			gui = this.GetComponent<GUITexture>();
			
			// Set dimensions for the texture
			scaledHeight = Screen.height*3/10;
			xPos = 0;
			yPos = 0;
			
			// Apply dimensions
			gui.pixelInset = new Rect(xPos, yPos, scaledHeight, scaledHeight);
		}
		else if(thisControl == ControlType.Indicator_Dot)
		{
			// get the GUITexture
			gui = this.GetComponent<GUITexture>();
			
			// Set dimensions for the texture
			scaledHeight = (Screen.height*3/10)/4;
			xPos = (Screen.height*3/10)/2 - scaledHeight/2;
			yPos = (Screen.height*3/10)/2 - scaledHeight/2;
			
			// Apply dimensions
			gui.pixelInset = new Rect(xPos, yPos, scaledHeight, scaledHeight);
			// Cache the default position and size
			defaultRect = gui.pixelInset;
			offsetRect = defaultRect;
			radius = offsetRect.x;
		}
		// This object controls jumping...
		else if(thisControl == ControlType.Jump){
			
			scaledWidth = Screen.width;
			scaledHeight = Screen.height*7/8;
			xPos = 0;
			yPos = 0;
			
			// If using a texture...
			if(jumpTexture)
			{
				// get the GUITexture
				gui = this.GetComponent<GUITexture>();
				
				gui.pixelInset = new Rect(xPos, yPos, scaledWidth, scaledHeight);
				defaultRect = gui.pixelInset;
				touchZone = defaultRect;
				
				Color color = gui.color;
				color.a = 0.2f;
				gui.color = color;
			}
			else if (!jumpTexture) // If not displaying jumpTexture...
			{
				this.GetComponent<GUITexture>().enabled = false;
				defaultRect = new Rect(xPos, yPos, scaledWidth, scaledHeight);
				touchZone = defaultRect;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	/* Moved and updated in FixedUpdate
		tilt = Input.acceleration;
		playerScript.MoveBall(new Vector3(tilt.x, 0, tilt.y));
	*/			
		
		// If controls haven't been changed and settings have changed...
		if(!controlsChanged && Control.controlsChangeCheck)
		{
			// Indicate controls have been changed
			controlsChanged = true;
			
			// Change the normalPercent multiplier
			GetNormalPercent();
		}
		
		/*\ Begin tilt reading and sending code \*/
		if(thisControl == ControlType.Tilt)
		{
			GetNormalTilt(); // Get the normalized tilt
			SendTilt(normalTilt); // Send the normalized tilt
		}
		/*\ Begin touchPad touching code \*/
		else if(thisControl == ControlType.Indicator_Dot)
		{
			GetNormalTilt(); // Get the normalized tilt
		//	SimplifyNormalTilt(); No point, too jerky, keeping here just in case. Meant to round off the normal tilt
			
			// Calculate offset, as normalTilt is a percent
			offsetRect.x = (int)System.Math.Round(defaultRect.x + normalTilt.x*radius);
			offsetRect.y = (int)System.Math.Round(defaultRect.y + normalTilt.y*radius);
		//	print(offsetRect);
		//	print(normalTilt);
		
			
			gui.pixelInset = offsetRect; // Apply offset
		}
		else if(thisControl == ControlType.Jump)
		{
			int count = Input.touchCount;
			
			if ( tapTimeWindow > 0 )
				tapTimeWindow -= Time.deltaTime;
			else
			{	
				tapCount = 0;
			}
				
			if(count != 0){
				for(int i= 0;i < count; i++)
				{
					Touch touch = Input.GetTouch(i);			
				//	Vector2 guiTouchPos = touch.position;
					
					bool shouldLatchFinger = false;			
					if ( touchZone.Contains( touch.position ) )
					{
						shouldLatchFinger = true;
					}
					
					if ( shouldLatchFinger && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
					{
						if(jumpTexture){
							// Change the alpha for tap effect
							Color color = gui.color;
							color.a = 0.05f;
							gui.color = color;
						}
						
						// Activate jump
						player.GetComponent<PlayerBall>().Jump ();
						
						lastFingerId = touch.fingerId;
					//	fingerDownPos = touch.position;
						//						fingerDownTime = Time.time;
						
						lastFingerId = touch.fingerId;
						
						/* Deprecated code? No need for this, and there's no tapTimeDelta ironically...
						// Accumulate taps if it is within the time window
						if ( tapTimeWindow > 0 )
							tapCount++;
						else
						{
							tapCount = 1;
							tapTimeWindow = tapTimeDelta;
						}
						*/
					} // end if shouldLatchFinger....
					
					if ( lastFingerId == touch.fingerId )
					{	
						// Override the tap count with what the iPhone SDK reports if it is greater
						// This is a workaround, since the iPhone SDK does not currently track taps
						// for multiple touches
						if ( touch.tapCount > tapCount )
						{
							tapCount = touch.tapCount;
						}
						/* Deprecated?
							// For a touchpad, let's just set the position directly based on distance from initial touchdown
							position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
							position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
						*/		
						
						if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
						{
							lastFingerId = -1;
						//	fingerDownPos = Vector2.zero;
							
							if(jumpTexture){
								// Change the alpha for tap effect
								Color color = gui.color;
								color.a = 0.2f;
								gui.color = color;
							}
						}				
					} // end if lastFingerId == touch.finderID
					
				} // end for how many counts
			} // end if count!= 0
		}
		/*\ End touchPad touching code \*/
	}
	
	public void GetNormalPercent(){
		normalPercent = Control.tiltSensitivity/200;
		if(normalPercent == 0)
		{ // If sensitivity hasn't been assigned, then use default value
			normalPercent = 0.5f;
		}
	}
	
	public void GetNormalTilt(){		
		tilt = Input.acceleration;
		normalTilt = tilt;
		
		if(normalTilt.x > 0)
		{
			if(normalTilt.x > normalPercent)
			{
				normalTilt.x = normalPercent;
			}
		}
		else if(normalTilt.x < 0)
		{
			if(normalTilt.x < -normalPercent)
			{
				normalTilt.x = -normalPercent;
			}
		}
		
		if(normalTilt.y > 0)
		{
			if(normalTilt.y > normalPercent)
			{
				normalTilt.y = normalPercent;
			}
		}
		else if(normalTilt.y < 0)
		{
			if(normalTilt.y < -normalPercent)
			{
				normalTilt.y = -normalPercent;
			}
		}
		
		normalTilt = normalTilt/normalPercent;
	} // end GetNormalTilt()
	
	public void SimplifyNormalTilt()
	{ // Rounds normalTilt for ease of screen calculations
		normalTilt.x = (float)System.Math.Round (normalTilt.x, 1);
		normalTilt.y = (float)System.Math.Round (normalTilt.y, 1);
	} // end SimplifyNormalTilt()
	
	public void SendTilt(Vector2 tiltSend)
	{
		playerScript.GetMoveInput(new Vector3(tiltSend.x, 0, tiltSend.y));
	} // end SendTilt
}
