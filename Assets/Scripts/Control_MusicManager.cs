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
			if(MPlayer.audio.clip != newMusic){
				MPlayer.audio.clip = newMusic;
				MPlayer.audio.Play ();
			}
		}
	}

	public void BoosterSound(){
		MPlayer.audio.PlayOneShot(boosterSound,1.5f);
	}

	public void JBoosterSound(){
		MPlayer.audio.PlayOneShot(jBoosterSound, 1.5f);
	}

	public void ButtonSound(){
		MPlayer.audio.PlayOneShot(buttonSound);
	}

	public void ClickSound(){
		MPlayer.audio.PlayOneShot(clickSound,1.5f);
	}

	public void DeathBell(){
		MPlayer.audio.PlayOneShot(deathSound);
	}

	public void MenuClick(){
		MPlayer.audio.PlayOneShot(menuSound);
	}

	public void PickupSound(){
		MPlayer.audio.PlayOneShot(pickupSound);
	}

	public void SecretSound(){
		MPlayer.audio.PlayOneShot(secretSound,2);
	}


	public void WinSound(){
		MPlayer.audio.PlayOneShot(winSound);
	}
}
