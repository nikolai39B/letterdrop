using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;

public partial class Arena : Node2D
{
    //-- OVERRIDES
    public override void _Ready()
    {
        // Set the instance as only one arena is expected at a time
        DebugUtils.Assert(Instance == null);
        Instance = this;
    }

    public override void _ExitTree()
    {
        // Clear the instance
        Instance = null;
    }


    //-- SUBMISSION


    /// <summary>
    /// Returns whether the given tile is selectable for word submission.
    /// </summary>
    /// <param name="tile">The tile</param>
    /// <returns>True if the tile is submittable; false otherwise</returns>
    public bool CanSubmitTile(FieldTile tile)
    {
        // Validate
        if (DebugUtils.AssertFalse(tile == null))
        {
            return false;
        }

        // If the tile is not active, it can't be submitted
        if (tile.TileState != FieldTile.State.Active)
        {
            return false;
        }

        // If the submission is full, can't submit the tile
        if (Submission.IsWordFull())
        {
            return false;
        }

        // If the submission is not empty, the tile is not submittable unless it is adjacent to the last selected tile
        if (!Submission.IsWordEmpty() && !Field.AreTilesAdjacent(tile, Submission.GetLastTile()))
        {
            return false;
        }

        return true;
    }

    public void SubmitTile(FieldTile tile)
    {
        Submission.AddTile(tile);
        tile.TileState = FieldTile.State.Submitted;
    }

    public void UnsubmitTile(FieldTile tile)
    {
        List<FieldTile> removedTiles = Submission.RemoveTile(tile);
        foreach (var removedTile in removedTiles)
        {
            removedTile.TileState = FieldTile.State.Active;
        }
    }


    //-- STATIC ATTRIBUTES
    public static Arena Instance { get; private set; }


    //-- COMPONENTS
    public Field Field { get { return GetNode<Field>("Field"); } }
    public Queue Queue { get { return GetNode<Queue>("Queue"); } }
    public Submission Submission { get { return GetNode<Submission>("Submission"); } }
}
