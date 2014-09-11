using UnityEngine;
using System.Collections;

public class Control_JBoost : MonoBehaviour {

	public GameObject player;
	public float boostMultiplier = 1;
	public bool upgradeCollision = false;
	public bool centerBoost = true;

	float boost = 10f;
	Vector3 boostDirection;

	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GameObject.Find("Player");
		}
		boostDirection = transform.up;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Fly(){
		if(upgradeCollision)
			PlayerBall.collisionCheckChange = true;

		if(!centerBoost){
			Vector3 boostPower = boostDirection * boostMultiplier * boost;
			player.rigidbody.velocity = boostPower;
		}
		else{
			player.transform.position = this.transform.position;
			Vector3 boostPower = boostDirection * boostMultiplier * boost;
			player.rigidbody.velocity = boostPower;
		}
	}
}
