using UnityEngine;
using System.Collections;

public class JumpButton : MonoBehaviour {

	// This button's texture component
	GUITexture gui;
	
	// Used for positioning the guiTexture
	float scaledHeight, xPos, yPos;

	// Use this for initialization
	void Start () {
		if (gui == null)
		{ // Get the guiTexture of the jump button
			gui = this.GetComponent<GUITexture>();
		}
		
		// Get the size and positioning of the texture
		scaledHeight = Screen.height*5/20;
		xPos = -scaledHeight;
		yPos = 0;
		
		// Apply the size and positioning
		gui.pixelInset = new Rect(xPos, yPos, scaledHeight, scaledHeight);
		
		//	Set the transparency of the texture
		Color color = gui.color;
		color.a = 1f;
		gui.color = color;
	}
	
	public void ChangeAlpha(float percent)
	{ // Change the alpha of the button
		
		// First change the alpha to more transparent
		Color color = gui.color;
		color.a = percent;
		gui.color = color;
		
		// Tell the button to wait a second before flickering back
		StartCoroutine(ToggleAlpha(1f, 0.2f));
	}
	
	public IEnumerator ToggleAlpha(float alphaChange, float timeToWait)
	{ // Retoggle the color of the jump button
		
		// Wait timeToWait seconds before changing the alpha
		yield return new WaitForSeconds(timeToWait);
		
		// Change the alpha finally
		Color color = gui.color;
		color.a = alphaChange;
		gui.color = color;
	}
}
