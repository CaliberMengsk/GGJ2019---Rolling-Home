using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
public class LevelEvent : UnityEvent<Level>{}

[System.Serializable]
public class LevelCompleteEvent : UnityEvent<LevelController1> { }

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
	[SerializeField]
	Player player;
	public LevelCompleteEvent OnAllLevelsComplete = new LevelCompleteEvent();
	public LevelEvent OnLevelTransitionComplete = new LevelEvent();
	public LevelEvent OnLevelLoaded = new LevelEvent();

	public bool nextLevelReady { get { return loadedScene != default(Scene) && loadedScene.isLoaded && loadedScene.IsValid(); }}
	bool levelTransition = false;
	float levelProgess = 0;
	Vector3 playerSlide;

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
			if(levelProgess + moveAmount >= levelSeparationDistance) { // if at our destination, do cleanup of transition
																	   // finish amy movement
				moveAmount = levelSeparationDistance - levelProgess;
				Debug.Log(moveAmount);
				// cleanup
				levelProgess = levelSeparationDistance;
				levelTransition = false;
				currentLevel.OnEndReached.RemoveListener(GoToNextLevel);

				currentLevel.StartIcarus(currentScene);
				//SceneManager.UnloadSceneAsync(currentScene);
				currentScene = loadedScene;
				currentLevel = loadedLevel;
				currentLevel.OnEndReached.AddListener(GoToNextLevel);
				levelNamesIndex++;

				Vector3 movement = Vector3.up * moveAmount;

				// move both levels up by the amount required each update step
				foreach(GameObject g in currentScene.GetRootGameObjects()) {
					g.transform.position += movement;
				}
				foreach(GameObject g in loadedScene.GetRootGameObjects()) {
					g.transform.position += movement;
				}
				player.transform.position += playerSlide * moveAmount / levelSeparationDistance;

				Debug.Log("setting player " + player.transform.position + " to " + currentLevel.GetStartPoint().position);

				player.rb.isKinematic = false;
				//currentLevel.Respawn();

				OnLevelTransitionComplete.Invoke(currentLevel);

				StartLoadnext();
			} else {
				levelProgess += moveAmount;

				Vector3 movement = Vector3.up * moveAmount;

				// move both levels up by the amount required each update step
				foreach(GameObject g in currentScene.GetRootGameObjects()) {
					g.transform.position += movement;
				}
				foreach(GameObject g in loadedScene.GetRootGameObjects()) {
					g.transform.position += movement;
				}
				player.transform.position += playerSlide * moveAmount/levelSeparationDistance;
			}
		}
	}

	bool StartLoadnext(){
		if(levelNames.Length - 1 > levelNamesIndex) {
			Debug.Log("Begin Load Scene " + levelNames[levelNamesIndex + 1]);
			// load next level
			//levelNamesIndex++;
			SceneManager.LoadSceneAsync(levelNames[levelNamesIndex+1], LoadSceneMode.Additive);
			return true;
		}
		Debug.Log("No More Scenes to Load");
		return false;

	}

	// called to begin transition to new level, when current level is complete
	public void GoToNextLevel(){
		if(levelNamesIndex >= levelNames.Length-1){ // if I'm on the last level
			Debug.Log("All levels complete.");
			OnAllLevelsComplete.Invoke(this);
			return;
		}
		player.rb.isKinematic = true;
		currentLevel.player = null;

		// get difference between start and current pose
		playerSlide = loadedLevel.GetStartPoint().position + Vector3.up * levelSeparationDistance - player.transform.position;

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

	void WaitForLoad(Level level){
		Debug.Log("Waiting...");
		GoToNextLevel();
		OnLevelLoaded.RemoveListener(WaitForLoad);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log("Loading Scene " + scene.name);

		Level level = null;
		foreach(GameObject g in scene.GetRootGameObjects()) {
			level = g.GetComponent<Level>();
			if(level != null) {
				level.player = player;
				// scene has a Level object, is a loaded level. init here
				if(currentLevel == null) { // initial level needs to start up high so levels can load in below it.
					Debug.Log("Scene " + scene.name + " is first level");
					currentScene = scene;
					currentLevel = level;
					currentLevel.OnEndReached.AddListener(GoToNextLevel);
					level.Move(Vector3.up * levelSeparationDistance);
					StartLoadnext();
					// player init stuff
					Debug.Log("* setting player " + player.transform.position + " to " + level.GetStartPoint().position);

					player.transform.position = level.GetStartPoint().position;
				} else {
					Debug.Log("Scene " + scene.name + " is a level");

					loadedScene = scene;
					loadedLevel = level;
					level.AlignTo(currentLevel.GetGoalPoint());
				}
				OnLevelLoaded.Invoke(level);
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
	void OnDestroy() {
		SceneManager.UnloadSceneAsync(currentScene);
		SceneManager.UnloadSceneAsync(loadedScene);
	}
}
