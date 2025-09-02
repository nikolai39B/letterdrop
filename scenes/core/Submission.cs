using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Submission : Control
{

    //-- WORD

    public bool IsWordEmpty()
    {
        return Word.Text.Length == 0;
    }

    public bool IsWordFull()
    {
        return Word.Text.Length >= MaxWordLength;
    }

    /// <summary>
    /// Adds the given tile to the end of the submission.
    /// </summary>
    /// <param name="tile">The tile to add</param>
    public void AddTile(FieldTile tile)
    {
        // Validate
        if (DebugUtils.AssertFalse(IsWordFull()))
        {
            return;
        }

        // Add the tile and letter
        _tiles.Add(tile);
        Word.Text += tile.Letter;
    }

    /// <summary>
    /// Removes the given tile and all subsequent tiles from the submission.
    /// </summary>
    /// <param name="tile">The tile to remove</param>
    /// <returns>The removed tile(s)</returns>
    public List<FieldTile> RemoveTile(FieldTile tile)
    {
        // Validate
        if (DebugUtils.AssertFalse(IsWordEmpty() || !_tiles.Contains(tile)))
        {
            return new List<FieldTile>();
        }

        // Remove the tile and all subsequent tiles
        int tt = _tiles.IndexOf(tile);
        int numTilesToRemove = _tiles.Count - tt;
        var removedTiles = _tiles.GetRange(tt, numTilesToRemove);
        _tiles.RemoveRange(tt, numTilesToRemove);

        // Remove the letter
        _tiles.RemoveAt(_tiles.Count - 1);
        Word.Text = Word.Text.Substring(0, Word.Text.Length - 1);

        return removedTiles;
    }

    public FieldTile GetLastTile()
    {
        // Handle no tiles
        if (_tiles.Count == 0)
        {
            return null;
        }

        // Get the last tile
        return _tiles.Last();
    }


    //-- EXPORT ATTRIBUTES

    /// <summary>
    /// The maximum number of letters allowed in the submission word
    /// </summary>
    [Export]
    public int MaxWordLength { get; set; }


    //-- COMPONENTS
    public Label Word { get { return GetNode<Label>("Word"); } }


    //-- ATTRIBUTES

    /// <summary>
    /// The submitted tiles
    /// </summary>
    public List<FieldTile> Tiles { get { return _tiles; } }
    private List<FieldTile> _tiles = new List<FieldTile>();
}
