using UnityEngine;
using System.Collections;

public class ControlChange : MonoBehaviour {

	public GameObject controlCenter;
	public GameObject player;
	private PlayerBall playerScript;
	
	public Texture[] Textures;	// 0 is joystick, 1 is tilt
	private GUITexture gui;
	// For setting the position and size of the texture(s)
	float scaledHeight, scaledWidth, xPosition, yPosition;
	
	int currentMode;
	
	// Use this for initialization
	void Start () {
		if(controlCenter == null){
			controlCenter = GameObject.Find ("Control Center");
		}
		if(player == null){
			player = GameObject.Find ("Player");
		}
		playerScript = player.GetComponent<PlayerBall>();
		
		gui = this.GetComponent<GUITexture>();
		currentMode = Control.currentControl;
		
		scaledHeight = Screen.height/3;
		xPosition = -scaledHeight;
		yPosition = 0;
		gui.pixelInset = new Rect(xPosition, yPosition, scaledHeight, scaledHeight);
		
		gui.texture = Textures[currentMode];
	}
	
	// Update is called once per frame
	void Update () {
		/* Start getting touch */
		if(Input.touchCount > 0){
			for(int i = 0; i < Input.touchCount; i++){
				Touch touch = Input.GetTouch(i);
				if(touch.phase == TouchPhase.Began &&
				   guiTexture.HitTest(touch.position)){
					AlternateMode();
				} // end if hit the texture
		} // end for however many touches
		}
	/* End getting touch */
	}
	
	void AlternateMode(){
		if (currentMode == 0){
			currentMode = 1;
			gui.texture = Textures[currentMode];
		}
		else if(currentMode == 1){
			currentMode = 0;
			gui.texture = Textures[currentMode];
		}
		playerScript.ToggleControlType(currentMode);
	}
}
