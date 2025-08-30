using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;

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
        Tiles.Add(tile);
        Word.Text += tile.Letter;
    }

    /// <summary>
    /// Removes the given tile and all subsequent tiles from the submission.
    /// </summary>
    /// <param name="tile">The tile to remove</param>
    public void RemoveTile(FieldTile tile)
    {
        // Validate
        if (DebugUtils.AssertFalse(IsWordEmpty() || !Tiles.Contains(tile)))
        {
            return;
        }

        // Remove the tile and all subsequent tiles
        int tt = Tiles.IndexOf(tile);
        Tiles.RemoveRange(tt, Tiles.Count - tt);

        // Remove the letter
        Tiles.RemoveAt(Tiles.Count - 1);
        Word.Text = Word.Text.Substring(0, Word.Text.Length - 1);
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
    public List<FieldTile> Tiles { get; private set; } = new List<FieldTile>();
}
