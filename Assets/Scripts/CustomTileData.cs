using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CustomTile", menuName = "Custom Tile", order = 1)]
public class CustomTileData : Tile
{

    public enum Interaction
    {
        None,
        Road,
        Fields,
        Forest,
        OldGrowth,
        Hills,
        Rocks,
        Mountain,
        Snow,
        Settlement,
        LoggingCamp,
        ElfVillage,
        DwarfMine,
        Flee,
    }

    public int food;
    public int energy;
    public Interaction interaction;
}
