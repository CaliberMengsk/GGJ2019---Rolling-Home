﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Globals : MonoBehaviour {
	public static float fxVolume = 1, musicVolume = 1;

	public Slider fxSlider, musicSlider;
    public AudioSource fxSource, musicSource;

	public static Globals instance;
	// Start is called before the first frame update
	void Start() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		fxVolume = PlayerPrefs.GetFloat("fxVolume", 1);
		fxSlider.value = fxVolume;
		musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
		musicSlider.value = musicVolume;
	}


	public void UpdateMusic(float inVol) {
		musicVolume = inVol;
		PlayerPrefs.SetFloat("musicVolume", inVol);
	}

	public void UpdateFX(float inVol) {
		fxVolume = inVol;
		PlayerPrefs.SetFloat("fxVolume", inVol);
	}

	public void LoadLevel(int levelID) {
		SceneManager.LoadScene(levelID, LoadSceneMode.Additive);
	}

	public void UnLoadLevel(int levelID) {
		SceneManager.UnloadSceneAsync(levelID);
	}

	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
		Application.OpenURL("http://google.com");
#else
		Application.Quit();
#endif
	}
}
