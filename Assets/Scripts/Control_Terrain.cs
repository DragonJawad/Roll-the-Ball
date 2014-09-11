using UnityEngine;
using System.Collections;

public class Control_Terrain : MonoBehaviour {
	
	GameObject player;
	
	public enum TerrainType{
		None,
		PlatformMover,
		Etc
	}
	public enum AxesType{X=0,Y=1,Z=2}
	public TerrainType thisTerrain;
	
	// Currently for PlatformMover variables
	public Transform targetA;
	public Transform targetB;
	Vector3 playerOffset;	// Offset between player and platform
	bool riding;			// Is the player on the platform?
	
	public float speed = 0.1f;
	
	public AxesType firstAxis;
	public float firstAxisDistance;
	public AxesType secondAxis;
	public float secondAxisDistance;
	
	Vector3 startPos;
	
	void Start () 
	{
		if(player == null)
		{
			player = GameObject.Find("Player");
		}
	}
	
	void FixedUpdate()
	{
		if(thisTerrain == TerrainType.PlatformMover)
		{
			float weight = Mathf.Cos (Time.time*speed*2*Mathf.PI)*0.5f+0.5f;
			// Calculate new position of the platform
			Vector3 newPosition = targetA.position *weight + targetB.position*(1-weight);
			// Calculate change of position, to apply to riders
			Vector3 changeRate = -this.transform.position + newPosition;
			this.transform.position = newPosition;
			
			// Following code for moving the player
			if(riding){
				player.transform.position += new Vector3(changeRate.x, 0, changeRate.z);	
			}
		}
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if(hit.gameObject == player)
		{
			riding = true;
		}
	}
	
	void OnTriggerExit(Collider hit)
	{
		if(hit.gameObject == player)
		{
			riding = false;
		}
	}
}
