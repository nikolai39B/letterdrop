using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class Queue : Control
{
	//-- OVERRIDES
	public override void _Ready()
	{
        // Configure the queue
        ConfigureLetterQueue();
        UpdateDisplay();
    }


	//-- CONFIGURATION

	private void ConfigureLetterQueue()
	{
		var letterFrenquencies = new Dictionary<char, int>
        {
            { 'A', 9 },
            { 'B', 2 },
            { 'C', 2 },
            { 'D', 4 },
            { 'E', 12 },
            { 'F', 2 },
            { 'G', 3 },
            { 'H', 2 },
            { 'I', 9 },
            { 'J', 1 },
            { 'K', 1 },
            { 'L', 4 },
            { 'M', 2 },
            { 'N', 6 },
            { 'O', 8 },
            { 'P', 2 },
            { 'Q', 1 },
            { 'R', 6 },
            { 'S', 4 },
            { 'T', 6 },
            { 'U', 4 },
            { 'V', 2 },
            { 'W', 2 },
            { 'X', 1 },
            { 'Y', 2 },
            { 'Z', 1 }
        };

        // Flatten to list
        LetterQueue.Clear();
        foreach (var (letter, frequency) in letterFrenquencies)
        {
            for (int ii = 0; ii < frequency; ii++)
            {
                LetterQueue.Add(letter);
            }
        }

        // Shuffle the queue
        var rng = new RandomNumberGenerator();
        rng.Seed = 0; // TODO get random seed
        for (int ii = LetterQueue.Count - 1; ii >= 1; ii--)
        {
            // Get a random index to swap with
            int jj = (int)(rng.Randi() % ii);

            // If the indices happen to be the same, nothing to do
            if (ii == jj)
            {
                continue;
            }

            // Swap the values
            (LetterQueue[ii], LetterQueue[jj]) = (LetterQueue[jj], LetterQueue[ii]);
        }
    }


    //-- QUEUE

    public bool IsQueueEmpty()
    {
        return LetterQueue.Count == 0;
    }

    public char? PopNextTile()
    {
        // Make sure there is a tile to pop
        if (DebugUtils.AssertFalse(IsQueueEmpty()))
        {
            return null;
        }

        // Pop the first letter
        char letter = LetterQueue[0];
        LetterQueue.RemoveAt(0);

        // Update the display
        UpdateDisplay();

        return letter;
    }


    //-- DISPLAY

    private void UpdateDisplay()
    {
        // Populate the tiles
        int remainingTiles = LetterQueue.Count;
        for (int ii = 0; ii < Tiles.Count; ii++)
        {
            // If there are no more remaining tiles, blank the label
            if (remainingTiles == 0)
            {
                // TODO implement
                Tiles[ii].Text = "";
            }
            else
            {
                char letter = LetterQueue[ii];
                Tiles[ii].Text = letter.ToString();
            }
        }

        // Populate the remaining label
        if (remainingTiles == 0)
        {
            // TODO
            RemainingLabel.Text = "";
        }
        else
        {
            RemainingLabel.Text = "+" + remainingTiles;
        }
    }


	//-- ATTRIBUTES

	private List<char> LetterQueue { get; set; } = new List<char>();

    public char? NextTile { get { return IsQueueEmpty() ? null : LetterQueue[0]; } }
    
    private List<Label> Tiles
    {
        get
        {
            var tiles = new List<Label>
            {
                GetNode<Label>("Tile1"),
                GetNode<Label>("Tile2"),
                GetNode<Label>("Tile3"),
                GetNode<Label>("Tile4"),
                GetNode<Label>("Tile5"),
                GetNode<Label>("Tile6"),
                GetNode<Label>("Tile7"),
            };
            return tiles;
        }
    }

    private Label RemainingLabel { get { return GetNode<Label>("Remaining"); } }
}
