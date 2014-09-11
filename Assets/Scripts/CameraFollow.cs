using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject player;				// the player sphere
	public bool getOffset = false;
	public bool projectorCheck;
	private Vector3 offset;					// the initial offset

	// Use this for initialization
	void Start () {
		if (!player) {
			player = GameObject.Find("Player");
		}

		if(!projectorCheck){
			if(getOffset)
				offset = transform.position;
			else
				offset = new Vector3(0,7,-8);
		}
		else{
			offset = new Vector3(0,1.5f,0);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
