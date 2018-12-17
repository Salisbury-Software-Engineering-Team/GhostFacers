using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame(){
		SceneManager.LoadScene ("Board/Scenes/Boards/Main_Board");


	}

	public void QuitGame(){
		Debug.Log ("QUIT");
		Application.Quit ();

	}
}
