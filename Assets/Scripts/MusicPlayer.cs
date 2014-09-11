using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public static MusicPlayer instance = null;
	public static MusicPlayer Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
