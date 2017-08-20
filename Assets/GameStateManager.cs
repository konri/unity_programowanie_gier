using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

	public static GameStateManager instance = null;

	[SerializeField] public GameObject mainMenu;

	private bool playersActive = false;
	private bool gameOver = false;
	private bool gameStarted = false;
	private int level = 1;
	private int points = 0;

	public bool PlayersActive {
		get { return playersActive; }
	}

	public bool GameOver {
		get { return gameOver; }
	}
		
	public bool GameStarted {
		get { return gameStarted; }
	}

	public int Level {
		get { return level; }
	}

	public int Points { 
		get { return points; }
	}

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

//		Assert.IsNotNull (mainMenu);
	}
		
	void Start () {
	}
		
	void Update () {
	}

	public void PlayersFallDown() {
		gameOver = true;
		gameStarted = false;
		playersActive = false;

	}

	public void PlayerStartedGame() {
		playersActive = true;
	}

	public void GameStart() {
		gameStarted = true;
		gameOver = false;
		level = 1;
		points = 0;
	}

	public void EnterGame() {
		mainMenu.SetActive (false);
		PlayerStartedGame ();
	}

	public void IncreaseLevel () {
		level += 1;
	}

	public void AddPoint() { 
		points += 1;
		if (points % 10 == 0) {
			IncreaseLevel ();
		}
	}

}
