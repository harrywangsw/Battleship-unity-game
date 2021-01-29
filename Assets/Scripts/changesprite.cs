using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public class changesprite : MonoBehaviour
{
    public Tilemap tilemap;
    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap> ();

    }

    public void ChangeTileTexture(Vector3Int coord, Sprite sprites_dig)
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprites_dig;
        //coord.y -= 1;//it's one tile off from y
        tilemap.SetTile(coord, tile);
        Debug.Log("Texture '" + tile.sprite.ToString() + "'");
    }
}
