using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlayerGoOut : MonoBehaviour {

	public MainGame mainGame;
	public AudioClip fallDown;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) { 
		mainGame.RemovePlayer (collider.gameObject);
		source.PlayOneShot (fallDown);
	}
}
