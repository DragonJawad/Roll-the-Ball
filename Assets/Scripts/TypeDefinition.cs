using UnityEngine;
using System.Collections;

public class TypeDefinition : MonoBehaviour {

	public enum ObjectTypes{
		Pickup,
		Exit,
		Button,
		Accelerator,
		InstantBoost,
		JumpBoost,
		SecretItem,
		BouncePlatform,
		BounceWall
	}

	public ObjectTypes typeOfObject;
}
