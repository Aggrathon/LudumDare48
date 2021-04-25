using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CustomTile", menuName = "Custom Tile", order = 1)]
public class CustomTile : Tile
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


    public struct TileInfo
    {
        public int energy;
        public int food;
        public Interaction interaction;

        public TileInfo(CustomTile ct)
        {
            energy = ct.energy;
            food = ct.food;
            interaction = ct.interaction;
        }

        public string GetTitle()
        {
            return interaction switch
            {
                Interaction.None => "Direction",
                Interaction.Road => "Road",
                Interaction.Fields => "Fields",
                Interaction.Forest => "Forest",
                Interaction.OldGrowth => "Old Growth Forest",
                Interaction.Hills => "Hills",
                Interaction.Rocks => "Rocky Ground",
                Interaction.Mountain => "Mountain",
                Interaction.Snow => "Snowy Peaks",
                Interaction.Settlement => "Village",
                Interaction.LoggingCamp => "Logging Camp",
                Interaction.ElfVillage => "Elven Village",
                Interaction.DwarfMine => "Dwarwen Mine",
                Interaction.Flee => "Village",
            };
        }

        public void Combine(TileInfo other)
        {
            energy += other.energy;
            food += other.food;
            interaction = other.interaction == Interaction.None ? interaction : other.interaction;
        }

        public void Combine(CustomTile other)
        {
            energy += other.energy;
            food += other.food;
            interaction = other.interaction == Interaction.None ? interaction : other.interaction;
        }
    }
}
