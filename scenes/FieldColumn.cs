using Godot;
using LetterDrop.Debug;
using LetterDrop;
using System;
using System.Collections.Generic;
using System.Data.Common;

public partial class FieldColumn : Control
{
    //-- CREATOR

    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="capacity">The maximum number of tiles supported by the column</param>
    /// <param name="number">The index of the column in the field</param>
    /// <returns>The instance</returns>
    public static FieldColumn Create(int capacity, int number)
    {
        // Instantiate
        FieldColumn instance = SceneCatalog.Instantiate<FieldColumn>(Type);
        if (DebugUtils.AssertFalse(instance == null))
        {
            return null;
        }

        // Initialize
        instance._capacity = capacity;
        instance._number = number;

        return instance;
    }


    //-- OVERRIDES
    public override void _Ready()
    {
        // Set a descriptive name
        Name = $"Column{_number}";
    }


    //-- TILES

    /// <summary>
    /// Returns whether the column capacity is filled with tiles.
    /// </summary>
    /// <returns>True if the column is full; false otherwise</returns>
    public bool IsFull()
    {
        return Tiles.Count >= Capacity;
    }

    /// <summary>
    /// Instantiates and places in the column a tile with the given letter.
    /// </summary>
    /// <param name="letter">The letter of the tile to add</param>
    /// <returns>The created tile</returns>
    public Tile DropTile(char letter)
    {
        // Validate the capacity
        if (DebugUtils.AssertFalse(IsFull()))
        {
            return null;
        }

        // Create and add the tile
        Tile tile = Tile.Create(letter);
        if (DebugUtils.AssertFalse(tile == null))
        {
            return null;
        }
        AddChild(tile);
        Tiles.Add(tile);
        TotalTiles++;

        // Position the tile
        Field field = GetParent<Field>();
        float tileY = (Tiles.Count - 1) * -field.TileSize - field.TileSize / 2;
        tile.Position = new Vector2(0, tileY);

        return tile;
    }


    //-- CONSTANTS
    public const SceneType Type = SceneType.FIELD_COLUMN;


    //-- INITIALIZATION ATTRIBUTES

    /// <summary>
    /// The tile capacity of the column
    /// </summary>
    public int Capacity { get => _capacity; }
    private int _capacity;

    /// <summary>
    /// The number of the column
    /// </summary>
    public int Number { get => _number; }
    private int _number;


    //-- ATTRIBUTES

    /// <summary>
    /// The parent field
    /// </summary>
    public Field ParentField { get => GetParent<Field>(); }

    /// <summary>
    /// The stack of tiles in the column, ordered from the bottom of the column
    /// </summary>
    public List<Tile> Tiles { get; private set; } = new List<Tile>();

    /// <summary>
    /// The total number of tiles dropped into the column since its creation
    /// </summary>
    public int TotalTiles { get; private set; } = 0;
}
