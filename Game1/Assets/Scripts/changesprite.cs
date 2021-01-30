using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public class changesprite : MonoBehaviour
{
    
    void Start()
    {
    }

    public void ChangeTileTexture(Tilemap tiles, Vector3Int coord, Sprite sprites_dig)
    {
        
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprites_dig;
        //coord.y -= 1;//it's one tile off from y
        tiles.SetTile(coord, tile);
        Debug.Log("Texture '" + tile.sprite.ToString() + "'");
    }
}
