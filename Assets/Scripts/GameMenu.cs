using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour 
{
	public void OnClickNewGame()
	{
		SceneManager.LoadScene("Chess Remaster", LoadSceneMode.Single);
	}
	public void OnClickExit()
	{
		Application.Quit();
	}
}