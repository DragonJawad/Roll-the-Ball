using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public GameObject ControlCenter;
	public GameObject panLeft;
	public GameObject panRight;
	public Transform[] targets = new Transform[1];
	public string destination;

	int currentPos = 0;		// Current position in Transform array
	int targetsLength;		// Number of targets
	bool saveCheck;	// If this is true, save the Prefs

	void Start(){
		if(ControlCenter == null)
			ControlCenter = GameObject.Find ("Control Center");

		if(panLeft == null)
			panLeft = GameObject.Find ("panLeft");
		if(panRight == null)
			panLeft = GameObject.Find ("panLeft");
		targetsLength = targets.Length;
	}

	void Update () {
	
	// If set to true, then DON'T activate anything
	if(Control.blockAction) return;
	
	if(Input.GetMouseButtonDown(0))
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	//	Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
		if (Physics.Raycast (ray, out hit)){
		//	print ("You clicked on: " + hit.transform.gameObject.name);
			TypeMenu typeMenuComponent = hit.transform.gameObject.GetComponent<TypeMenu> ();
			if (typeMenuComponent != null){
				switch(typeMenuComponent.thisMenu){
					case TypeMenu.MenuType.MainSelect:
						destination = hit.transform.GetComponent<TypeMenu>().worldDestination;
						if(destination != ""){
							ControlCenter.GetComponent<Control_MusicManager>().MenuClick();
							Application.LoadLevel (destination);
						}
						else print("No destination set!");
					break;
					
					case TypeMenu.MenuType.WorldSelect:
						if(hit.transform.GetComponent<TypeMenu>().unlockCheck){
							destination = hit.transform.GetComponent<TypeMenu>().worldDestination;
							if(destination != ""){
								ControlCenter.GetComponent<Control_MusicManager>().MenuClick();
								Application.LoadLevel (destination);
							}
							else print("No destination set!");
						}
					break;
					
					case TypeMenu.MenuType.LevelSelect:
						if(hit.transform.GetComponent<TypeMenu>().unlockCheck){
							destination = hit.transform.GetComponent<TypeMenu>().worldDestination;
							if(destination != ""){
								Application.LoadLevel (destination);
								ControlCenter.GetComponent<Control_MusicManager>().MenuClick();
							}
							else print("No destination set!");
						}
					break;

					case TypeMenu.MenuType.GUIWindow:
						ControlCenter.GetComponent<Control_MusicManager>().MenuClick();
						hit.transform.GetComponent<GUIWindow>().Activate();
					break;

					case TypeMenu.MenuType.CamTransition:
						ControlCenter.GetComponent<Control_MusicManager>().MenuClick();
						destination = hit.transform.GetComponent<TypeMenu>().worldDestination;
						CamTransition(destination);
					break;
				} // close check switch
			}
		} // close raycast hit check
	} // Close If Input.GetMouseButton(0)
	} // close Update()

	public void CamTransition(string targetText){
		int targetNum = int.Parse(targetText);
		StartCoroutine(MoveCam(this.transform, transform.position, targets[targetNum].position, 0.5f));
	//	AutoFade.CamFade(0.5f,Color.black,true);
	//	StartCoroutine(MoveCamFade(this.transform, transform.position, targets[targetNum].position, 0.5f));
	}

	public void NextPos(){
		if(currentPos != (targetsLength-1)){
			currentPos++;
			StartCoroutine(MoveCam(this.transform, transform.position, targets[currentPos].position, 0.5f));
			if(currentPos == (targetsLength-1))
				panRight.GetComponent<Control_Texture>().ToggleDestination(false);
		}
		panLeft.GetComponent<Control_Texture>().ToggleDestination(true);
	}
	public void BackPos(){
		if(currentPos != 0){
			currentPos--;
			StartCoroutine(MoveCam(this.transform, transform.position, targets[currentPos].position, 0.5f));
			if (currentPos == 0)
				panLeft.GetComponent<Control_Texture>().ToggleDestination(false);
		}
		panRight.GetComponent<Control_Texture>().ToggleDestination(true);
	}

	IEnumerator MoveCam(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time){
		float i = 0.0f;
		float rate = 1.0f/time;
		while (i < 1.0){
			yield return new WaitForEndOfFrame();
			i = i + (Time.deltaTime * rate);
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
		}
	}

	IEnumerator MoveCamFade(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time){
		yield return new WaitForSeconds(time);
		thisTransform.position = endPos;
	}

}
