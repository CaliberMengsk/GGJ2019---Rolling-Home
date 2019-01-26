using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
	public UnityEvent OnAllLevelsComplete = new UnityEvent();

	void passthru(LevelController1 lc1){
		Debug.Log("[Main Menu] all levels complete");
		OnAllLevelsComplete.Invoke();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log("[Main Menu] Loading Scene " + scene.name);

		LevelController1 level = null;
		foreach(GameObject g in scene.GetRootGameObjects()) {
			level = g.GetComponent<LevelController1>();
			if(level != null) {
				level.OnAllLevelsComplete.AddListener(passthru);
				return;
			}
		}
		Debug.Log("[Main Menu] Scene " + scene.name + " has no Level manager script, ignoring...");
	}
	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
