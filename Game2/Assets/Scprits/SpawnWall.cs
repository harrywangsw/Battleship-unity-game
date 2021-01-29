using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall : MonoBehaviour
{
    public GameObject Walloriginal;
    void Start()
    {
	CopyWalls(50);
    }
    private void CopyWalls(int wallnum)
    {
	for (int i = 0; i < wallnum; i++)
	{
		GameObject WallClones= Instantiate(Walloriginal, new Vector2(i*2.8f, Walloriginal.transform.position.y), Walloriginal.transform.rotation);   
		WallClones.name = "WallClones-"+(i+1);
		
	}
    }
}
