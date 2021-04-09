using System.Collections.Generic;
using UnityEngine;

public class BoardHighLights : MonoBehaviour
{
    public GameObject HighLightPrefab;
    private List<GameObject> HighLights;
    public static BoardHighLights Instance { set; get; }

    private void Start()
    {
        Instance = this;
        HighLights = new List<GameObject>();
    }

    private GameObject GetHighLightObject()
    {
        var go = HighLights.Find(g => !g.activeSelf);
        if(go == null)
        {
            go = Instantiate(HighLightPrefab);
            HighLights.Add(go);
        }

        return go;
    }

    public void HighLightAllowedMoves(bool[,] moves)
    {
        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
            {
                if(moves[i, j])
                {
                    var go = GetHighLightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }

    public void HideHighLights()
    {
        foreach(var go in HighLights) go.SetActive(false);
    }
}