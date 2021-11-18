using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapSetup : MonoBehaviour
{

    [System.Serializable]
    public struct TileRamp
    {
        public Tile tile;

        public Direction rampDirection;
    }

    public List<TileRamp> tileRamps = new List<TileRamp>();

    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        //https://gamedev.stackexchange.com/questions/150917/how-to-get-all-tiles-from-a-tilemap
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if(tilemap.HasTile(localPlace))
            {
                var tileSprite = tilemap.GetSprite(localPlace);

                BaseTile baseTile = (BaseTile) ScriptableObject.CreateInstance("BaseTile");
                baseTile.sprite = tileSprite;

                bool isRamp = false;
                Direction rampDirection = Direction.None;

                for (int i = 0; i < tileRamps.Count; i++)
                {
                    if (tileSprite.Equals(tileRamps[i].tile.sprite))
                    {
                        UnityEngine.Debug.Log("There was a ramp!");
                        isRamp = true;
                        rampDirection = tileRamps[i].rampDirection;
                        break;
                    }
                }

                baseTile.isRamp = isRamp;
                baseTile.rampDirection = rampDirection;

                tilemap.SetTile(localPlace, baseTile);
            }

        }
    }
}
