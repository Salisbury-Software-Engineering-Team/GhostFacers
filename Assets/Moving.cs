using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {
	//yellow tiles are help tiles.
	// Use this for initialization
	void Start () {
		//left and right movement is 6.4 per tile to move accross board, works like noraml x-y axis.
		//up and down movement is 
		transform.position = new Vector3 (38f, 28f, -9.6f);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
