using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	public bool typeOverride = false;
	public bool typeOverride2 = false;

	Vector3 startPos;

	void Start(){
		startPos = transform.up;
	}

	// Update is called once per frame
	void Update () {

		if(!typeOverride2){
			TypeDefinition typeDefinitionComponent = this.GetComponent<TypeDefinition> ();
			//	ObjectTypes ObjectType = TypeDefinition.ObjectTypes;
			if(typeOverride){
				transform.Rotate (Vector3.up * 180 * Time.deltaTime, Space.World);
				transform.Rotate (transform.right * 45 * Time.deltaTime, Space.World);
			}
			else if (typeDefinitionComponent != null){
				switch(typeDefinitionComponent.typeOfObject){
				case TypeDefinition.ObjectTypes.Pickup: // if it is a common pickup
					transform.Rotate (Vector3.up * 180 * Time.deltaTime, Space.World);
				break;
					
				case TypeDefinition.ObjectTypes.Exit:	// if it is the exit
				//	transform.Rotate (Vector3.up * -180 * Time.deltaTime, Space.World);
					transform.Rotate (Vector3.up * -180 * Time.deltaTime, Space.World);
					transform.Rotate (transform.right * 45 * Time.deltaTime, Space.World);
				break;

				case TypeDefinition.ObjectTypes.SecretItem:	// if it is a secret item
					transform.Rotate (Vector3.up * -180 * Time.deltaTime, Space.World);
					transform.Rotate (transform.right * 45 * Time.deltaTime, Space.World);
				break;
				
				case TypeDefinition.ObjectTypes.JumpBoost: // if it is a common pickup
				//	this.transform.position = startPos;
					transform.Rotate (startPos * 180 * Time.deltaTime, Space.World);
				//	transform.Rotate (Vector3.up * 180 * Time.deltaTime, Space.World);
					break;
				} // close switch
			} // close != null
		} // close if !typeOverride2
		else{
			transform.Rotate (Vector3.right * 90 * Time.deltaTime, Space.World);
		}
	} // close Update()
}
