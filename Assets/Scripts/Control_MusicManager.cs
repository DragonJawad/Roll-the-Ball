using UnityEngine;
using System.Collections;

public class Control_MusicManager : MonoBehaviour {

	// The actual Music Player object
	GameObject MPlayer;
	
	// The assigned BGM from Inspector
	public AudioClip newMusic;
	
	// Miscelleneous SFX for multiple uses
	public AudioClip boosterSound;
	public AudioClip buttonSound;
	public AudioClip clickSound;
	public AudioClip deathSound;
	public AudioClip jBoosterSound;
	public AudioClip menuSound;
	public AudioClip pickupSound;
	public AudioClip secretSound;
	public AudioClip winSound;

	// ...This could be more efficient
	void Start () {
		if( MPlayer == null)
			MPlayer = MusicPlayer.instance.gameObject;

		if(newMusic != null){
			if(MPlayer.GetComponent<AudioSource>().clip != newMusic){
				MPlayer.GetComponent<AudioSource>().clip = newMusic;
				MPlayer.GetComponent<AudioSource>().Play ();
			}
		}
	}

	public void BoosterSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(boosterSound,1.5f);
	}

	public void JBoosterSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(jBoosterSound, 1.5f);
	}

	public void ButtonSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(buttonSound);
	}

	public void ClickSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(clickSound,1.5f);
	}

	public void DeathBell(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(deathSound);
	}

	public void MenuClick(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(menuSound);
	}

	public void PickupSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(pickupSound);
	}

	public void SecretSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(secretSound,2);
	}


	public void WinSound(){
		MPlayer.GetComponent<AudioSource>().PlayOneShot(winSound);
	}
}
