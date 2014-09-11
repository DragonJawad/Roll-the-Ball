using UnityEngine;
using System.Collections;

public class SecretIndicator : MonoBehaviour {

	public TextMesh secretsText;
	
	int secretsTotal;
	int secretsCollected;

	// Use this for initialization
	void Start () {
		secretsTotal = Control.secretsTotal;
		secretsCollected = Control.secretsCounter;
		secretsText.text = secretsCollected + "/" + secretsTotal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
