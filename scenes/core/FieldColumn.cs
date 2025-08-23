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
        FieldTile currPendingTile = PendingTile;
        if (DebugUtils.AssertFalse(currPendingTile == null))
        {
            return;
        }

        // Configure the pending tile
        currPendingTile.Letter = letter;
        currPendingTile.TileState = FieldTile.State.Active;
        currPendingTile.ButtonPressed = false;
        ActiveTilesCount++;

        // Set the next tile as pending
        FieldTile nextPendingTile = PendingTile;
        if (nextPendingTile != null)
        {
            nextPendingTile.TileState = FieldTile.State.Pending;
        }
    }

    private void InitializeTiles()
    {
        // Populate the field with tiles
        for (int ii = 0; ii < Capacity; ii++)
        {
            // Create and add the tile
            FieldTile tile = FieldTile.Create();
            if (DebugUtils.AssertFalse(tile == null))
            {
                continue;
            }
            AddTile(tile);

            // Set a descriptive name
            tile.Name = $"Tile{TotalTiles}";

            // Configure the tile
            tile.Letter = null;
            tile.TileState = ii == 0 ? FieldTile.State.Pending : FieldTile.State.Disabled;

            // Set the tile placement
            Field field = Field;
            tile.Position = new Vector2(field.GapSize, (Capacity - Tiles.Count) * field.TileSize + field.GapSize);
        }
    }

    private void OnTilePressed(FieldTile tile)
    {
        // Handle pending tiles
        if (tile.TileState == FieldTile.State.Pending)
        {
            // Drop a tile from the queue
            char? letter = Arena.Instance?.Queue?.PopNextTile();
            if (letter != null)
            {
                DropTile(letter.Value);
            }
        }

        // Handle active tiles
        else if (tile.TileState == FieldTile.State.Active)
        {
            // Select the tile
            tile.TileState = FieldTile.State.Selected;

            // TODO only if no tiles selected or if adjacent to a selected tile
        }

        // Handle selected tiles
        else if (tile.TileState != FieldTile.State.Disabled)
        {
            // Deselect the tile
            tile.TileState = FieldTile.State.Selected;

            // TODO deselect 
        }
    }

    private void AddTile(FieldTile tile)
    {
        // Add the child
        AddChild(tile);
        Tiles.Add(tile);
        TotalTiles++;

        //Action<>
        //
        //// Register the button pressed signal
        //var onTilePressed = () => { OnTilePressed(tile); };
        //_onTilePressedDelegates[tile] = onTilePressed;
        //tile.Pressed += onTilePressed;

        // TODO: figure out the event handling here. This is not completely safe if the column gets destroyed before the tile (which probably won't happen but still...)

        var err = tile.Connect(FieldTile.SignalName.Pressed, Callable.From(() => { OnTilePressed(tile); }));
        var err2 = tile.Connect(FieldTile.SignalName.Pressed, Callable.From((int x) => { OnTilePressed(tile); return ""; }));

        //var t = tile.GetType();
        //var ev = t.GetEvent("Pressed");
        //ev.AddEventHandler(tile, () => { OnTilePressed(tile); });
        //
        ////var ev = tile.Pressed;
        //
        //_delegates[tile].Add(new EventRegistrant(, () => { OnTilePressed(tile); }))

        int x = 0;
        x++;
    }
    //private Dictionary<Node, Tuple<Action> _delegates = new Dictionary<FieldTile, Action>();

    //private Dictionary<FieldTile, List<IEventRegistrant>> _delegates = new Dictionary<FieldTile, List<IEventRegistrant>>();

    //private void OnTileStateChanged(FieldTile tile, FieldTile.State oldState, FieldTile.State newState)
    //{
    //    // Handle pending...
    //    if (oldState == FieldTile.State.Pending)
    //    {
    //        // ...to active
    //        if (newState == FieldTile.State.Active)
    //        {
    //            // Drop a tile from the queue
    //            char? letter = Arena.Instance?.Queue?.PopNextTile();
    //            if (letter != null)
    //            {
    //                DropTile(letter.Value);
    //            }
    //        }
    //    }
    //}


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
    public Field Field { get => GetParent<Field>(); }

    /// <summary>
    /// The stack of tiles in the column, ordered from the bottom of the column
    /// </summary>
    public List<FieldTile> Tiles { get; private set; } = new List<FieldTile>();

    /// <summary>
    /// The number of tiles in the column that are active
    /// </summary>
    public int ActiveTilesCount { get; private set; } = 0;

    private FieldTile PendingTile
    {
        get => IsFull() ? null : Tiles[ActiveTilesCount];
    }

    /// <summary>
    /// The total number of tiles dropped into the column since its creation
    /// </summary>
    public int TotalTiles { get; private set; } = 0;
}
