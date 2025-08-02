using Godot;
using LetterDrop.Debug;
using LetterDrop;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;

public partial class FieldColumn : Control
{
    //-- CREATOR

    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="capacity">The maximum number of tiles supported by the column</param>
    /// <returns>The instance</returns>
    public static FieldColumn Create(int capacity)
    {
        // Instantiate
        FieldColumn instance = SceneCatalog.Instantiate<FieldColumn>(Type);
        if (DebugUtils.AssertFalse(instance == null))
        {
            return null;
        }

        // Initialize
        instance._capacity = capacity;

        return instance;
    }


    //-- OVERRIDES
    public override void _Ready()
    {
        // Populate the column with tiles
        InitializeTiles();
    }

    public override void _Draw()
    {
        base._Draw();

        //Vector2[] bottomPoints = { 
        //    new Vector2(0, 0),
        //    new Vector2(ParentField.TileSize, 0),
        //};
        //DrawPolyline(bottomPoints, new Color("pink"), 4);
    }


    //-- TILES

        /// <summary>
        /// Returns whether the column capacity is filled with tiles.
        /// </summary>
        /// <returns>True if the column is full; false otherwise</returns>
    public bool IsFull()
    {
        return ActiveTilesCount >= Capacity;
    }

    /// <summary>
    /// Instantiates and places in the column a tile with the given letter.
    /// </summary>
    /// <param name="letter">The letter of the tile to add</param>
    public void DropTile(char letter)
    {
        // Get the pending tile
        Tile currPendingTile = PendingTile;
        if (DebugUtils.AssertFalse(currPendingTile == null))
        {
            return;
        }

        // Configure the pending tile
        currPendingTile.Letter = letter;
        currPendingTile.TileState = Tile.State.Active;
        ActiveTilesCount++;

        // Set the next tile as pending
        Tile nextPendingTile = PendingTile;
        if (nextPendingTile != null)
        {
            nextPendingTile.TileState = Tile.State.Pending;
        }
    }

    private void InitializeTiles()
    {
        // Populate the field with tiles
        for (int ii = 0; ii < Capacity; ii++)
        {
            // Create and add the tile
            Tile tile = Tile.Create();
            if (DebugUtils.AssertFalse(tile == null))
            {
                continue;
            }
            AddChild(tile);
            Tiles.Add(tile);
            TotalTiles++;

            // Set a descriptive name
            tile.Name = $"Tile{TotalTiles}";

            // Configure the tile
            tile.Letter = null;
            tile.TileState = ii == 0 ? Tile.State.Pending : Tile.State.Disabled;
            

            // Place the tile
            Field field = ParentField;
            float width = field.TileSize - field.GapSize * 2;
            tile.Size = new Vector2(width, width);
            tile.PivotOffset = new Vector2(width / 2, width / 2);
            tile.Position = new Vector2(field.GapSize, (Capacity - Tiles.Count) * field.TileSize + field.GapSize);
        }
    }


    //-- CONSTANTS
    public const SceneType Type = SceneType.FIELD_COLUMN;


    //-- INITIALIZATION ATTRIBUTES

    /// <summary>
    /// The tile capacity of the column
    /// </summary>
    public int Capacity { get => _capacity; }
    private int _capacity;


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
    /// The number of tiles in the column that are active.
    /// </summary>
    public int ActiveTilesCount { get; private set; } = 0;

    private Tile PendingTile
    {
        get => IsFull() ? null : Tiles[ActiveTilesCount];
    }

    /// <summary>
    /// The total number of tiles dropped into the column since its creation
    /// </summary>
    public int TotalTiles { get; private set; } = 0;
}
