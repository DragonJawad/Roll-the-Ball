using UnityEngine;
using System.Collections;

public class Control_Button : MonoBehaviour {

	public GameObject ControlCenter;
	public GameObject Interaction;		// What the button will affect
	public InteractionTypes typeOfInteraction;	// What will be done with Interaction
	bool activationCheck = true;		// If activated = true

	public enum InteractionTypes{
		NA,
		Destroy,
		ActivatePlatform,
		Instantiate
	}

	void Start(){
		if(ControlCenter == null){
			ControlCenter = GameObject.Find ("Control Center");
		}
		if (Interaction == null) {
			typeOfInteraction = InteractionTypes.NA;
		}
	}

	public void Activate(){
		if(activationCheck){	// If not already activated..
			ControlCenter.GetComponent<Control_MusicManager>().ButtonSound();
			this.animation.Play ();

			if (typeOfInteraction == InteractionTypes.NA) {
				print ("ERROR! Fix this button, ya fool!");
			}

			if (typeOfInteraction == InteractionTypes.Destroy) {
				Interaction.SetActive(false);
			}

			if (typeOfInteraction == InteractionTypes.ActivatePlatform){
				Interaction.collider.enabled = true;
				Interaction.renderer.material.color = Color.white;
			}

			activationCheck = false;	// Can't double press
		}
	}
}
