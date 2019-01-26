using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelController1 : MonoBehaviour {
	[SerializeField]
	string[] levelNames;
	public int levelNamesIndex = 0;
	Scene currentScene;
	Level currentLevel;
	Scene loadedScene;
	Level loadedLevel;
	[SerializeField]
	float levelSeparationDistance = 100f;
	[SerializeField]
	float levelReplacementSpeed = 100f;
	public UnityEvent OnAllLevelsComplete = new UnityEvent();
	public UnityEvent OnLevelTransitionComplete = new UnityEvent();
	public UnityEvent OnLevelLoaded = new UnityEvent();

	public bool nextLevelReady { get { return loadedScene != default(Scene) && loadedScene.isLoaded && loadedScene.IsValid(); }}
	bool levelTransition = false;
	float levelProgess = 0;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	// Start is called before the first frame update
	void Start() {
		transform.position += Vector3.up * levelSeparationDistance; // move to active scne area
		// load initial scene
		SceneManager.LoadSceneAsync(levelNames[levelNamesIndex], LoadSceneMode.Additive);
	}

	// Update is called once per frame
	void Update() {
		if(levelTransition){
			//Debug.Log("Transitioning from " + currentScene.name + " to " + loadedScene.name);

			// track replacement progress of the levels
			float moveAmount = levelReplacementSpeed * Time.deltaTime;
			if(levelProgess + moveAmount > levelSeparationDistance){ // if at our destination, do cleanup of transition
				// finish amy movement
				moveAmount = levelSeparationDistance - levelProgess;
				// cleanup
				levelProgess = levelSeparationDistance;
				levelTransition = false;
				SceneManager.UnloadSceneAsync(currentScene);
				currentScene = loadedScene;
				currentLevel = loadedLevel;
				levelNamesIndex++;
				if(StartLoadnext()){
					OnLevelTransitionComplete.Invoke();
				}else{
					// no more levels to load, end game
					OnAllLevelsComplete.Invoke();
				}
			}else{
				levelProgess += moveAmount;
			}
			Vector3 movement = Vector3.up * moveAmount;

			// move both levels up by the amount required each update step
			foreach(GameObject g in currentScene.GetRootGameObjects()){
				g.transform.position += movement;
			}
			foreach(GameObject g in loadedScene.GetRootGameObjects()) {
				g.transform.position += movement;
			}
		}
	}

	bool StartLoadnext(){
		if(levelNames.Length - 1 > levelNamesIndex) {
			// load next level
			//levelNamesIndex++;
			SceneManager.LoadSceneAsync(levelNames[levelNamesIndex+1], LoadSceneMode.Additive);
			return true;
		}
		return false;

	}

	// called to begin transition to new level, when current level is complete
	public void GoToNextLevel(){
		if(levelNamesIndex >= levelNames.Length-1){ // if I'm on the last level
			Debug.Log("All levels complete.");
			OnAllLevelsComplete.Invoke();
			return;
		}
		Debug.Log("Going to level " + loadedScene.name + " from " + currentScene.name);
		if(!nextLevelReady){
			// wait for level load
			Debug.Log("Waiting for next level to load...");
			OnLevelLoaded.AddListener(WaitForLoad);
			return;
		}
		levelProgess = 0f;
		levelTransition = true;
	}

	void WaitForLoad(){
		GoToNextLevel();
		OnLevelLoaded.RemoveListener(WaitForLoad);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log("Loading Scene " + scene.name);

		Level level = null;
		foreach(GameObject g in scene.GetRootGameObjects()) {
			level = g.GetComponent<Level>();
			if(level != null) {
				// scene has a Level object, is a loaded level. init here
				if(currentLevel == null) { // initial level needs to start up high so levels can load in below it.
					Debug.Log("Scene " + scene.name + " is first level");
					currentScene = scene;
					currentLevel = level;
					level.Move(Vector3.up * levelSeparationDistance);
					StartLoadnext();
				} else {
					Debug.Log("Scene " + scene.name + " is a level");

					loadedScene = scene;
					loadedLevel = level;
					level.AlignTo(currentLevel.GetGoalPoint());
				}
				OnLevelLoaded.Invoke();
				return;
			}
		}
		Debug.Log("Scene " + scene.name + " has no Level script, ignoring...");
	}


	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
