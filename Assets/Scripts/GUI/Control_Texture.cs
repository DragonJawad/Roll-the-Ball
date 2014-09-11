using UnityEngine;
using System.Collections;

public class Control_Texture : MonoBehaviour {

	public enum TextureType{
		None,
		Pause,
		Retry,
		SecretIndicator,
		WorldPanL,
		WorldPanR,
		GameLogo,
		Background,
		JoystickBG,
		Settings,
		Return
	}

	public GameObject ControlCenter;
	public TextureType thisTexture;
	public GameObject player;
	public bool showGUI = true;

	private GUITexture myGUITexture;
	private float scaledHeight, scaledWidth, xPosition, yPosition;

	// Special pan variables
	public GameObject Cam;
	public bool destinationCheck = true;
	
	// Special JoystickBG variables
	float sizeMultiplier; // Used for JoystickBG, setting for changing size
	bool controlsChanged; // Flag for if controls have already been changed

	// Special return variables
	string destination;

	void Awake()
	{
		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
	}

	// Use this for initialization
	void Start () {
		if(ControlCenter == null)
			ControlCenter = GameObject.Find ("Control Center");

		if(player == null){
			player = GameObject.Find ("Player");
		}
		if(Cam == null && (thisTexture == TextureType.WorldPanL || 
		                   thisTexture == TextureType.WorldPanR)){
			Cam = GameObject.Find ("Main Camera");;
		}

		if(thisTexture == TextureType.Pause){
			scaledHeight = Screen.height/8;
			xPosition = 0;
			yPosition = -scaledHeight;
			
			Color color2 = myGUITexture.color;
			color2.a = 1f;
			myGUITexture.color = color2;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
		}
		else if(thisTexture == TextureType.Retry){
			scaledHeight = Screen.height/8;
			xPosition = -scaledHeight;
			yPosition = -scaledHeight;
			
			Color color2 = myGUITexture.color;
			color2.a = 1f;
			myGUITexture.color = color2;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
		}
		else if(thisTexture == TextureType.SecretIndicator){
			scaledHeight = Screen.height/6;
			xPosition = Screen.width-Screen.width*3/10;
			yPosition = -scaledHeight;
			
			Color color2 = myGUITexture.color;
			color2.a = 1f;
			myGUITexture.color = color2;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
		}
		else if(thisTexture == TextureType.WorldPanL || thisTexture == TextureType.WorldPanR){
			if(destinationCheck){	// If there is a destination
				Color color;
				color = myGUITexture.color;
				color.a = 1f;
				myGUITexture.color = color;
			}
			else{
				Color color;
				color = myGUITexture.color;
				color.a = 0.2f;
				myGUITexture.color = color;
			}
			
			// Get size of arrows
			scaledHeight = Screen.height/4;
			scaledWidth = Screen.width*1/10;
			yPosition = -scaledHeight/2;
			
			if(thisTexture == TextureType.WorldPanL){
				xPosition = 0;
				myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledWidth, scaledHeight);
			}
			else if(thisTexture == TextureType.WorldPanR){
				xPosition = -scaledWidth;
				myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledWidth, scaledHeight);
			}
		} // End pan specific Start() stuff
		else if(thisTexture == TextureType.GameLogo){
			scaledHeight = Screen.height/2;
			scaledWidth = Screen.width*2/3;
			xPosition = -scaledWidth/2;
			yPosition = 0;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledWidth, scaledHeight);
		}
		else if(thisTexture == TextureType.Background){
			scaledHeight = Screen.height*1.005f;
			scaledWidth = Screen.width*1.005f;
			xPosition = 0;
			yPosition = 0;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledWidth, scaledHeight);
		}
		else if(thisTexture == TextureType.None){
			scaledHeight = 100;
			xPosition = 0;
			yPosition = 0;
			Color color2 = myGUITexture.color;
			color2.a = 1f;
			myGUITexture.color = color2;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
			Debug.LogError("It's... not a texture?!");
		}
		else if(thisTexture == TextureType.JoystickBG)
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

			myGUITexture.pixelInset = new Rect(0f, 0f, scaledHeight, scaledHeight);
			
			float touchOffset;	
			touchOffset = myGUITexture.pixelInset.width * 0.5f;
			
			myGUITexture.pixelInset = new Rect(touchOffset, 0, 4*touchOffset, 4*touchOffset);	
		}
		else if(thisTexture == TextureType.Settings){
			scaledHeight = Screen.height/8;
			xPosition = -scaledHeight;
			yPosition = -scaledHeight;
			
			Color color2 = myGUITexture.color;
			color2.a = 1f;
			myGUITexture.color = color2;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
		}
		else if(thisTexture == TextureType.Return){
			scaledHeight = Screen.height/8;
			xPosition = 0;
			yPosition = -scaledHeight;
			
			Color color2 = myGUITexture.color;
			color2.a = 1f;
			myGUITexture.color = color2;
			myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
		}
	} // End start
	
	// Update is called once per frame
	void Update () {
	
		// If we're showing this GUI (which is defaultly true)
		if(showGUI){
			// Make sure the guiTexture is enabled
			guiTexture.enabled = true;

			// if this Texture is a JoystickBG
			if(thisTexture == TextureType.JoystickBG)
			{
				// If controls haven't been already changed and settings have been changed...
				if(!controlsChanged && Control.controlsChangeCheck)
				{
					// Reset this texture's size and such
					ResetJoystickBG();
					
					// Indicate controls have already been changed
					controlsChanged = true;
				}
			}

			if(Input.touchCount > 0){
				for(int i = 0; i < Input.touchCount; i++){

					Touch touch = Input.GetTouch(i);
					if(touch.phase == TouchPhase.Began &&
					   	guiTexture.HitTest(touch.position)){

						if (thisTexture == TextureType.Pause){
							ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
							player.GetComponent<PlayerBall>().PauseToggle();
						}
						else if(thisTexture == TextureType.Retry){
							ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
							player.GetComponent<PlayerBall>().Restart();
						}
						else if(thisTexture == TextureType.Settings){
							ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
							player.GetComponent<PlayerBall>().OpenSettings();
						}
						else if(thisTexture == TextureType.WorldPanL){
							if(destinationCheck){
								ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
								Cam.GetComponent<MenuControl>().BackPos();
							}
						}
						else if(thisTexture == TextureType.WorldPanR){
							if(destinationCheck){
								ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
								Cam.GetComponent<MenuControl>().NextPos();
							}
						}
						else if(thisTexture == TextureType.Return){
							ControlCenter.GetComponent<Control_MusicManager>().ClickSound();
							Application.LoadLevel(Control.worldID);
						}
						else if(thisTexture == TextureType.None){
							print("How are you touching this?!");
						}
					
					} // close if touch.phase
				} // close for in loop
			} // close if touch count
		}
//		else
//			guiTexture.enabled = false;
	
	} // close Update()

	void OnMouseEnter(){
		if(thisTexture == TextureType.Pause || thisTexture == TextureType.Retry ||
			thisTexture == TextureType.Settings || thisTexture == TextureType.Return)
		{
			// Set alpha to 0.1f
			Color color = myGUITexture.color;
			color.a = 0.1f;
			myGUITexture.color = color;
			
			// Retoggle the color in 0.5 seconds
			StartCoroutine(ReToggle(0.2f));
		}
		else if(thisTexture == TextureType.WorldPanL || thisTexture == TextureType.WorldPanR){
			if(destinationCheck){	// Check if there's a possible destination
				Color color;
				color = myGUITexture.color;
				color.a = 0.2f;
				myGUITexture.color = color;
			}
		}
	}

	void OnMouseExit(){
		/*
		if(thisTexture == TextureType.Pause || thisTexture == TextureType.Retry){
			Color color = myGUITexture.color;
			color.a = 1f;
			myGUITexture.color = color;
		}
		*/
		if(thisTexture == TextureType.WorldPanL || thisTexture == TextureType.WorldPanR){
			if(destinationCheck){	// Check if there's a possible destination
				Color color;
				color = myGUITexture.color;
				color.a = 1f;
				myGUITexture.color = color;
			}
		}
	}

	public void ToggleDestination(bool setTrue){	// For remotely changing destinationCheck
		if(setTrue){	// If setting this to true...
			destinationCheck = true;
			Color color;
			color = myGUITexture.color;
			color.a = 1f;
			myGUITexture.color = color;
		}
		else{
			destinationCheck = false;
			Color color;
			color = myGUITexture.color;
			color.a = 0.2f;
			myGUITexture.color = color;
		}
	}

	IEnumerator ReToggle(float time){
		yield return new WaitForSeconds(time);

		Color color = myGUITexture.color;
		color.a = 1f;
		myGUITexture.color = color;
	}
	
	public void ResetJoystickBG()
	{ // Reset the joystick BG if controls have been changed
		
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
		
		myGUITexture.pixelInset = new Rect(0f, 0f, scaledHeight, scaledHeight);
		
		float touchOffset;	
		touchOffset = myGUITexture.pixelInset.width * 0.5f;
		
		myGUITexture.pixelInset = new Rect(touchOffset, 0, 4*touchOffset, 4*touchOffset);	
	}
}
