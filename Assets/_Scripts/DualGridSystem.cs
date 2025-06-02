using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TileType;

public class DualGridSystem : MonoBehaviour
{
    protected static Vector3Int[] NEIGHBOURS = new Vector3Int[] {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 1, 0)
    };

    protected static Dictionary<Tuple<TileType, TileType, TileType, TileType>, Tile> neighbourTupleToTile;
    protected static Dictionary<Tuple<TileType, TileType, TileType, TileType>, AnimatedTile> neighbourTupleToAnimatedTile;

    // Provide references to each tilemap in the inspector
    public Tilemap placeholderTilemap;
    public Tilemap displayTilemap;

    // Provide the placeholder tiles in the inspector
    public Tile grassPlaceholderTile;
    public Tile dirtPlaceholderTile;
    public Tile roadPlaceholderTile;
    public Tile sandPlaceholderTile;
    public Tile waterPlaceholderTile;

    // Provide the 16 tiles in the inspector
    public Tile[] tiles;
    public AnimatedTile[] aniTiles;

    void Start()
    {
        // This dictionary stores the "rules", each 4-neighbour configuration corresponds to a tile
        // |_1_|_2_|
        // |_3_|_4_|
        neighbourTupleToTile = new() {
            {new (Grass, Grass, Grass, Grass), tiles[12]},
            {new (Dirt, Dirt, Dirt, Grass), tiles[7]}, // OUTER_BOTTOM_RIGHT
            {new (Dirt, Dirt, Grass, Dirt), tiles[10]}, // OUTER_BOTTOM_LEFT
            {new (Dirt, Grass, Dirt, Dirt), tiles[2]}, // OUTER_TOP_RIGHT
            {new (Grass, Dirt, Dirt, Dirt), tiles[5]}, // OUTER_TOP_LEFT
            {new (Dirt, Grass, Dirt, Grass), tiles[11]}, // EDGE_RIGHT
            {new (Grass, Dirt, Grass, Dirt), tiles[1]}, // EDGE_LEFT
            {new (Dirt, Dirt, Grass, Grass), tiles[9]}, // EDGE_BOTTOM
            {new (Grass, Grass, Dirt, Dirt), tiles[3]}, // EDGE_TOP
            {new (Dirt, Grass, Grass, Grass), tiles[15]}, // INNER_BOTTOM_RIGHT
            {new (Grass, Dirt, Grass, Grass), tiles[8]}, // INNER_BOTTOM_LEFT
            {new (Grass, Grass, Dirt, Grass), tiles[0]}, // INNER_TOP_RIGHT
            {new (Grass, Grass, Grass, Dirt), tiles[13]}, // INNER_TOP_LEFT
            {new (Dirt, Grass, Grass, Dirt), tiles[4]}, // DUAL_UP_RIGHT
            {new (Grass, Dirt, Dirt, Grass), tiles[14]}, // DUAL_DOWN_RIGHT
            {new (Dirt, Dirt, Dirt, Dirt), tiles[6]},

            //{new (Grass, Grass, Grass, Grass), tiles[22]},
            {new (Sand, Sand, Sand, Grass), tiles[29]}, // OUTER_BOTTOM_RIGHT
            {new (Sand, Sand, Grass, Sand), tiles[16]}, // OUTER_BOTTOM_LEFT
            {new (Sand, Grass, Sand, Sand), tiles[24]}, // OUTER_TOP_RIGHT
            {new (Grass, Sand, Sand, Sand), tiles[31]}, // OUTER_TOP_LEFT
            {new (Sand, Grass, Sand, Grass), tiles[17]}, // EDGE_RIGHT
            {new (Grass, Sand, Grass, Sand), tiles[27]}, // EDGE_LEFT
            {new (Sand, Sand, Grass, Grass), tiles[19]}, // EDGE_BOTTOM
            {new (Grass, Grass, Sand, Sand), tiles[25]}, // EDGE_TOP
            {new (Sand, Grass, Grass, Grass), tiles[21]}, // INNER_BOTTOM_RIGHT
            {new (Grass, Sand, Grass, Grass), tiles[18]}, // INNER_BOTTOM_LEFT
            {new (Grass, Grass, Sand, Grass), tiles[26]}, // INNER_TOP_RIGHT
            {new (Grass, Grass, Grass, Sand), tiles[23]}, // INNER_TOP_LEFT
            {new (Sand, Grass, Grass, Sand), tiles[30]}, // DUAL_UP_RIGHT
            {new (Grass, Sand, Sand, Grass), tiles[20]}, // DUAL_DOWN_RIGHT
            {new (Sand, Sand, Sand, Sand), tiles[28]},

            //{new (Grass, Grass, Grass, Grass), tiles[38]},
            {new (Road, Road, Road, Grass), tiles[45]}, // OUTER_BOTTOM_RIGHT
            {new (Road, Road, Grass, Road), tiles[32]}, // OUTER_BOTTOM_LEFT
            {new (Road, Grass, Road, Road), tiles[40]}, // OUTER_TOP_RIGHT
            {new (Grass, Road, Road, Road), tiles[47]}, // OUTER_TOP_LEFT
            {new (Road, Grass, Road, Grass), tiles[33]}, // EDGE_RIGHT
            {new (Grass, Road, Grass, Road), tiles[43]}, // EDGE_LEFT
            {new (Road, Road, Grass, Grass), tiles[35]}, // EDGE_BOTTOM
            {new (Grass, Grass, Road, Road), tiles[41]}, // EDGE_TOP
            {new (Road, Grass, Grass, Grass), tiles[37]}, // INNER_BOTTOM_RIGHT
            {new (Grass, Road, Grass, Grass), tiles[34]}, // INNER_BOTTOM_LEFT
            {new (Grass, Grass, Road, Grass), tiles[42]}, // INNER_TOP_RIGHT
            {new (Grass, Grass, Grass, Road), tiles[39]}, // INNER_TOP_LEFT
            {new (Road, Grass, Grass, Road), tiles[46]}, // DUAL_UP_RIGHT
            {new (Grass, Road, Road, Grass), tiles[36]}, // DUAL_DOWN_RIGHT
            {new (Road, Road, Road, Road), tiles[44]},
        };

        neighbourTupleToAnimatedTile = new()
        {
            {new (Grass, Grass, Grass, Grass), aniTiles[12]},
            {new (Water, Water, Water, Grass), aniTiles[7]}, // OUTER_BOTTOM_RIGHT
            {new (Water, Water, Grass, Water), aniTiles[10]}, // OUTER_BOTTOM_LEFT
            {new (Water, Grass, Water, Water), aniTiles[2]}, // OUTER_TOP_RIGHT
            {new (Grass, Water, Water, Water), aniTiles[5]}, // OUTER_TOP_LEFT
            {new (Water, Grass, Water, Grass), aniTiles[11]}, // EDGE_RIGHT
            {new (Grass, Water, Grass, Water), aniTiles[1]}, // EDGE_LEFT
            {new (Water, Water, Grass, Grass), aniTiles[9]}, // EDGE_BOTTOM
            {new (Grass, Grass, Water, Water), aniTiles[3]}, // EDGE_TOP
            {new (Water, Grass, Grass, Grass), aniTiles[15]}, // INNER_BOTTOM_RIGHT
            {new (Grass, Water, Grass, Grass), aniTiles[8]}, // INNER_BOTTOM_LEFT
            {new (Grass, Grass, Water, Grass), aniTiles[0]}, // INNER_TOP_RIGHT
            {new (Grass, Grass, Grass, Water), aniTiles[13]}, // INNER_TOP_LEFT
            {new (Water, Grass, Grass, Water), aniTiles[4]}, // DUAL_UP_RIGHT
            {new (Grass, Water, Water, Grass), aniTiles[14]}, // DUAL_DOWN_RIGHT
            {new (Water, Water, Water, Water), aniTiles[6]},

            {new (Sand, Sand, Sand, Sand), aniTiles[28]},
            {new (Water, Water, Water, Sand), aniTiles[23]}, // OUTER_BOTTOM_RIGHT
            {new (Water, Water, Sand, Water), aniTiles[26]}, // OUTER_BOTTOM_LEFT
            {new (Water, Sand, Water, Water), aniTiles[18]}, // OUTER_TOP_RIGHT
            {new (Sand, Water, Water, Water), aniTiles[21]}, // OUTER_TOP_LEFT
            {new (Water, Sand, Water, Sand), aniTiles[27]}, // EDGE_RIGHT
            {new (Sand, Water, Sand, Water), aniTiles[17]}, // EDGE_LEFT
            {new (Water, Water, Sand, Sand), aniTiles[25]}, // EDGE_BOTTOM
            {new (Sand, Sand, Water, Water), aniTiles[19]}, // EDGE_TOP
            {new (Water, Sand, Sand, Sand), aniTiles[31]}, // INNER_BOTTOM_RIGHT
            {new (Sand, Water, Sand, Sand), aniTiles[24]}, // INNER_BOTTOM_LEFT
            {new (Sand, Sand, Water, Sand), aniTiles[16]}, // INNER_TOP_RIGHT
            {new (Sand, Sand, Sand, Water), aniTiles[29]}, // INNER_TOP_LEFT
            {new (Water, Sand, Sand, Water), aniTiles[20]}, // DUAL_UP_RIGHT
            {new (Sand, Water, Water, Sand), aniTiles[30]}, // DUAL_DOWN_RIGHT
            //{new (Water, Water, Water, Water), aniTiles[22]}
        };


        RefreshDisplayTilemap();
    }

    public void SetCell(Vector3Int coords, Tile tile)
    {
        placeholderTilemap.SetTile(coords, tile);
        setDisplayTile(coords);
    }

    private TileType GetPlaceholderTileTypeAt(Vector3Int coords)
    {
        Tile tile = placeholderTilemap.GetTile<Tile>(coords);

        if (tile == grassPlaceholderTile) return Grass;
        else if (tile == sandPlaceholderTile) return Sand;
        else if (tile == roadPlaceholderTile) return Road;
        else if (tile == waterPlaceholderTile) return Water;
        else if (tile == dirtPlaceholderTile) return Dirt;
        else
            return Grass;
    }

    protected TileBase calculateDisplayTile(Vector3Int coords)
    {
        // 4 neighbours
        TileType topRight = GetPlaceholderTileTypeAt(coords - NEIGHBOURS[0]);
        TileType topLeft = GetPlaceholderTileTypeAt(coords - NEIGHBOURS[1]);
        TileType botRight = GetPlaceholderTileTypeAt(coords - NEIGHBOURS[2]);
        TileType botLeft = GetPlaceholderTileTypeAt(coords - NEIGHBOURS[3]);

        Tuple<TileType, TileType, TileType, TileType> neighbourTuple = new(topLeft, topRight, botLeft, botRight);


        if (topLeft == Water || topRight == Water || botLeft == Water || botRight == Water)
        {
            if (neighbourTupleToAnimatedTile.TryGetValue(neighbourTuple, out AnimatedTile aniTile))
                return aniTile;
        }
        else
        {
            if (neighbourTupleToTile.TryGetValue(neighbourTuple, out Tile staticTile))
                return staticTile;
        }
            

        return null;
    }

    protected void setDisplayTile(Vector3Int pos)
    {
        for (int i = 0; i < NEIGHBOURS.Length; i++)
        {
            Vector3Int newPos = pos + NEIGHBOURS[i];
            displayTilemap.SetTile(newPos, calculateDisplayTile(newPos));
        }
    }

    // The tiles on the display tilemap will recalculate themselves based on the placeholder tilemap
    public void RefreshDisplayTilemap()
    {
        for (int i = -100; i < 100; i++)
        {
            for (int j = -100; j < 100; j++)
            {
                setDisplayTile(new Vector3Int(i, j, 0));
            }
        }
    }
}

public enum TileType
{
    None,
    Grass,
    Dirt,
    Sand,
    Road,
    Water
}
