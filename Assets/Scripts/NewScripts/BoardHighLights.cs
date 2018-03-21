using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighLights : MonoBehaviour 
{
	public static BoardHighLights Instance { set; get; }
	public GameObject HighLightPrefab;
	private List<GameObject> HighLights;

	private void Start()
	{
		Instance = this;
		HighLights = new List<GameObject>();
	}

	private GameObject GetHighLightObject()
	{
		GameObject GO = HighLights.Find(g => !g.activeSelf);
		if(GO == null)
		{
			GO = Instantiate(HighLightPrefab);
			HighLights.Add(GO);
		}
		return GO;
	}

	public void HighLightAllowedMoves(bool[,] Moves)
	{
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				if(Moves[i, j])
				{
					GameObject GO = GetHighLightObject();
					GO.SetActive(true);
					GO.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
				}
			}
		}
	}

	public void HideHighLights()
	{
		foreach(GameObject GO in HighLights) GO.SetActive(false);
	}
}
