using Godot;
using System;

using LetterDrop.Debug;

public partial class FieldOutline : Line2D
{
    //-- OVERRIDES

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Visible = false;

        // Configure the outline
        ConfigureOutline();
    }


    //-- CONFIGURATION

    /// <summary>
    /// Sets the points for the given field size.
    /// </summary>
    public void ConfigureOutline()
	{
        Field field = GetParent<Field>();

        // Add the left wall
        ClearPoints();
        float xCurr = 0.0f;
        float yCurr = 0.0f;
		AddPoint(new Vector2(xCurr - field.GapSize, yCurr + field.GapSize));
        yCurr += field.NumRows * field.TileSize;
        AddPoint(new Vector2(xCurr - field.GapSize, yCurr + field.GapSize));

        // Add the floor, alternating between high and low tiles
        for (int ii = 0; ii < field.NumColumns; ++ii)
        {
            // Get the new x position
            xCurr += field.TileSize;

            // Draw the appropriate floor based on whether this is a high or low tile
            bool isHighTile = ii % 2 == 0;
            if (isHighTile)
            {
                // If this is the last column, extend the floor past the tile one gap length
                if (ii == field.NumColumns - 1)
                {
                    AddPoint(new Vector2(xCurr + field.GapSize, yCurr + field.GapSize));
                }

                // Otherwise, cut the floor off one gap length and draw down
                else
                {
                    AddPoint(new Vector2(xCurr - field.GapSize, yCurr + field.GapSize));
                    AddPoint(new Vector2(xCurr - field.GapSize, yCurr + field.TileSize / 2 + field.GapSize));
                }
            }
            else
            {
                // Extend the floor past the tile one gap length and draw up
                AddPoint(new Vector2(xCurr + field.GapSize, yCurr + field.TileSize / 2 + field.GapSize));
                AddPoint(new Vector2(xCurr + field.GapSize, yCurr + field.GapSize));
            }
        }

        // Add the right wall
        yCurr -= field.NumRows * field.TileSize;
        AddPoint(new Vector2(xCurr + field.GapSize, yCurr + field.GapSize));
    }
}
