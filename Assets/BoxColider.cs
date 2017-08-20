using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColider : MonoBehaviour {

	public MainGame mainGame;
	public AudioClip hit;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) { 
		GameStateManager.instance.AddPoint ();
		mainGame.UpdateState ();
		source.PlayOneShot (hit);
	}

}
