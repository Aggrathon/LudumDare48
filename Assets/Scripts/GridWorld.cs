using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridWorld : MonoBehaviour
{
    public Tilemap terrain;
    public Tilemap decorators;

    public (int, int, CustomTileData.Interaction) GetTileCost(Vector3Int pos)
    {
        var terrainTile = terrain.GetTile<CustomTileData>(pos);
        int energy = terrainTile.energy;
        int food = terrainTile.food;
        var otherTile = decorators.GetTile<CustomTileData>(pos);
        if (otherTile != null)
        {
            energy += otherTile.energy;
            food += otherTile.food;
            if (otherTile.interaction != CustomTileData.Interaction.None)
                return (energy, food, otherTile.interaction);
        }
        return (energy, food, terrainTile.interaction);
    }

    public Vector3 SnapToTile(Vector3 pos)
    {
        return terrain.CellToWorld(terrain.WorldToCell(pos));
    }

    public Vector3 SnapToTile(Vector3Int pos)
    {
        return terrain.CellToWorld(pos);
    }

    public Vector3Int GetTile(Vector3 pos)
    {
        return terrain.WorldToCell(pos);
    }

    public (int, int, CustomTileData.Interaction) GetTileCost(Vector3 pos)
    {
        return GetTileCost(GetTile(pos));
    }

    public Vector3Int LeftOf(Vector3Int pos)
    {
        return terrain.WorldToCell(SnapToTile(pos) + new Vector3(-0.7f, 0.5f, 0f));
    }
    public Vector3Int LeftOf(Vector3 pos)
    {
        return terrain.WorldToCell(pos + new Vector3(-0.7f, 0.5f, 0f));
    }

    public Vector3Int RightOf(Vector3Int pos)
    {
        return terrain.WorldToCell(SnapToTile(pos) + new Vector3(0.7f, 0.5f, 0f));
    }

    public Vector3Int RightOf(Vector3 pos)
    {
        return terrain.WorldToCell(pos + new Vector3(0.7f, 0.5f, 0f));
    }

    public Vector3Int TopOf(Vector3Int pos)
    {
        pos.x++;
        return pos;
    }
}
