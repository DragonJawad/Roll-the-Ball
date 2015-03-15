using UnityEngine;
using System.Collections;

public class Control_Boost : MonoBehaviour {

	public GameObject player;
	public float boostMultiplier = 1;
	public bool upgradeCollision = false;	// Check true for when going through objects
	public bool centerBoost = false;		// Boost from the center?
	
	float boost = 40;	// The flat boost rate

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.Find("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Boost(){

		if(upgradeCollision)
			PlayerBall.collisionCheckChange = true;

	//	Vector3 boostPower = -transform.right * boostMultiplier * boost;
	//	player.rigidbody.velocity = boostPower;
		
		// If not boosting from the center of the booster...
		if(!centerBoost){
			Vector3 boostPower = -transform.right * boostMultiplier * boost;
			player.GetComponent<Rigidbody>().velocity = boostPower;
		}
		else{
			// Move the ball to the center of the booster
			player.transform.position = this.transform.position;
			// Calculate the boost itself
			Vector3 boostPower = -transform.right * boostMultiplier * boost;
			// Apply boost
			player.GetComponent<Rigidbody>().velocity = boostPower;
		}
	}
}
