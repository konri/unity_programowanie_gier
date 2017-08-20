using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainGame : MonoBehaviour {
	enum Direction {
		left, right
	};


	public List<GameObject> zombies;
	private List<GameObject> defaultListZombies = new List<GameObject> ();

	public Vector3 selectedVectorSize;
	public Vector3 defaultVectorSize;

	public Text scoreTextField;
	public Text levelTextField;

	public RawImage gameOver;
	public RawImage replayBtn;

	public AudioClip startNewGameAudio;
	public AudioClip swipePlayer;
	private AudioSource source;

	private GameObject selectedZombie;
	private int selectedIndex;

	private bool monitor = false;

	public List<float> defaultZposition;


	// Use this for initialization
	void Start () {
		SetRandomAngularDrag ();
		SelectZombie(RandomCharacter ());
		defaultListZombies.AddRange (zombies);
		gameOver.enabled = false;
		replayBtn.enabled = false;
		scoreTextField.enabled = false;
		levelTextField.enabled = false;
		source = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (GameStateManager.instance.PlayersActive) {
				CheckKeyboardEvents ();
		}

		if (!GameStateManager.instance.GameStarted && GameStateManager.instance.PlayersActive) {
			StartPlayersMove ();
			GameStateManager.instance.GameStart ();
			UpdateState ();
			source.PlayOneShot (startNewGameAudio);
			scoreTextField.enabled = true;
			levelTextField.enabled = true;
		}


	}

	public void UpdateState () { 
		scoreTextField.text = "Score: " + GameStateManager.instance.Points;
		levelTextField.text = "Level: " + GameStateManager.instance.Level;
	}

	public void RemovePlayer(GameObject zombie) {
//		while (monitor)
//			;
//		monitor = true;
		if (zombie == selectedZombie) {
			for (int i = 0; i < zombies.Count; i++) {
				if (zombie != zombies [i]) {
					SelectZombie (zombies [i]);
					break;
				}
			}
		}
		zombies.Remove (zombie);

		if (zombies.Count == 0) {
			Debug.Log ("End");
			GameStateManager.instance.PlayersFallDown ();
			GameOver ();
		}
//		monitor = false;
	}

	public void replay () { 
		zombies.AddRange (defaultListZombies);
		SetDefaultSize ();
		SetDefaultPositions ();
		SelectZombie(RandomCharacter ());

		gameOver.enabled = false;
		replayBtn.enabled = false;
		GameStateManager.instance.GameStart ();
		GameStateManager.instance.PlayerStartedGame ();
		source.PlayOneShot (startNewGameAudio);
	}

	private void CheckKeyboardEvents() {
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			Move (Direction.left);

		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			Move (Direction.right);
		
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			PushForceSelectedPlayer ();
	}

	private void Move(Direction direction) {
		if (zombies.Count == 1)
			return;
		
//		while (monitor)
//			;
//		monitor = true;
//
		
		int oldSelectedIndex = selectedIndex;
		switch (direction) {
		case Direction.left:
			selectedIndex -= 1;
			break;
		case Direction.right:
			selectedIndex += 1;
			break;
		}
			
		Debug.Log (direction + "old: " + oldSelectedIndex + " now: " + selectedIndex + "size: " + zombies.Count);

		if (selectedIndex < 0) {
			selectedIndex = zombies.Count - 1;
		}

		if (selectedIndex >= zombies.Count) {
			selectedIndex = 0;
		}
		if (selectedIndex < 0 || selectedIndex > zombies.Count -1)
			return;
		SelectZombie (zombies [selectedIndex]);
		source.PlayOneShot (swipePlayer);

		if (oldSelectedIndex < 0 || oldSelectedIndex > zombies.Count - 1)
			return;
		
		UnselectZombie (zombies [oldSelectedIndex]);

//		monitor = false;
	}

	private void SelectZombie(GameObject zombie) {
		selectedZombie = zombie;
		selectedZombie.transform.localScale = selectedVectorSize;
	}

	private void UnselectZombie (GameObject zombie) {
		zombie.transform.localScale = defaultVectorSize;
	}

	private GameObject RandomCharacter () {
		var rnd = new System.Random(DateTime.Now.Millisecond);
		int indx = rnd.Next(0, zombies.Count - 1);
		selectedIndex = indx;
		return zombies[indx];
	}

	private void SetRandomAngularDrag() {
		float level = GameStateManager.instance.Level;
		foreach (GameObject zombie in zombies) {
			Rigidbody rb = zombie.GetComponent<Rigidbody> ();
			rb.angularDrag = UnityEngine.Random.Range(level, level + 3.0f);
		}
	}

	private void StartPlayersMove() {
		foreach (GameObject zombie in zombies) {
			Rigidbody rb = zombie.GetComponent<Rigidbody> ();
			rb.useGravity = true;
		}
	}

	private void PushForceSelectedPlayer() {
		int force;
		if (GameStateManager.instance.Level > 3) {
			force = UnityEngine.Random.Range (5, 20);
		} else {
			force = 10;
		}

		Rigidbody rb = selectedZombie.GetComponent<Rigidbody> ();
		rb.AddForce (0, 0, force, ForceMode.Impulse);

	}

	private void GameOver() {
		Console.Write ("GameOVer");
		gameOver.enabled = true;
		replayBtn.enabled = true;
	}

	private void SetDefaultPositions() { 
		for (int i = 0; i < zombies.Count; i++) { 
			float x = UnityEngine.Random.Range (1.5f, 0.37f);
			float y = UnityEngine.Random.Range (18.0f, 12.0f); 
			zombies[i].transform.localPosition = new Vector3 (-x, -y, defaultZposition[i]);
		}
	}

	private void SetDefaultSize () {
		foreach (GameObject zombie in zombies) {
			UnselectZombie (zombie);
		}
	}
}
