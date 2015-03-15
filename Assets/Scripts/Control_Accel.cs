using UnityEngine;
using System.Collections;

public class Control_Accel : MonoBehaviour {

	public GameObject player;
	public float boostMultiplier = 1;
	public bool upgradeCollision = false;

	float boost = 5;

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.Find("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Accelerate(){
		if(upgradeCollision)
			PlayerBall.collisionCheckChange = true;
		
		float boostPower = boostMultiplier * boost;
	//	print (boostPower);
		player.GetComponent<Rigidbody>().velocity = boostPower * player.GetComponent<Rigidbody>().velocity;
	}
}
