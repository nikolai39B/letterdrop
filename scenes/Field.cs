using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;

public partial class Field : Control
{
    //-- OVERRIDES

    public override void _EnterTree()
    {
        base._EnterTree();

        //NumRows = 1;
        //NumColumns = 3;
        TileSize = 100;
        Position = new Vector2(10 + GapSize, 10 + GapSize + TileSize / 2);
    }
    public override void _Ready()
	{

        // Create the columns
        ConfigureColumns();

        // Debug
        AddTile('A', 0);
        AddTile('Z', 1);
        AddTile('Y', 1);
        AddTile('C', 2);
        AddTile('D', 3);
        AddTile('X', 4);
        AddTile('M', 5);
        AddTile('N', 5);
        AddTile('O', 5);
        AddTile('P', 5);
        AddTile('Q', 5);
        AddTile('R', 5);
        AddTile('S', 5);
        AddTile('T', 5);
        AddTile('1', 6);
        AddTile('2', 6);
        AddTile('3', 6);
        AddTile('4', 6);
        AddTile('5', 6);
        AddTile('6', 6);
        AddTile('7', 6);
    }

    public override void _Draw()
    {
        base._Draw();

        // Add the left wall
        List<Vector2> points = new List<Vector2>();
        float xCurr = 0;
        float yCurr = 0;
        points.Add(new Vector2(xCurr - GapSize, yCurr + GapSize));
        yCurr += NumRows * TileSize;
        points.Add(new Vector2(xCurr - GapSize, yCurr + GapSize));
        
        // Add the floor, alternating between high and low tiles
        for (int ii = 0; ii < NumColumns; ++ii)
        {
            // Get the new x position
            xCurr += TileSize;
        
            // Draw the appropriate floor based on whether this is a high or low tile
            bool isHighTile = ii % 2 == 0;
            if (isHighTile)
            {
                // If this is the last column, extend the floor past the tile one gap length
                if (ii == NumColumns - 1)
                {
                    points.Add(new Vector2(xCurr + GapSize, yCurr + GapSize));
                }
        
                // Otherwise, cut the floor off one gap length and draw down
                else
                {
                    points.Add(new Vector2(xCurr - GapSize, yCurr + GapSize));
                    points.Add(new Vector2(xCurr - GapSize, yCurr + TileSize / 2 + GapSize));
                }
            }
            else
            {
                // Extend the floor past the tile one gap length and draw up
                points.Add(new Vector2(xCurr + GapSize, yCurr + TileSize / 2 + GapSize));
                points.Add(new Vector2(xCurr + GapSize, yCurr + GapSize));
            }
        }
        
        // Add the right wall
        yCurr -= NumRows * TileSize;
        points.Add(new Vector2(xCurr + GapSize, yCurr + GapSize));
        DrawPolyline(points.ToArray(), new Color("white"), 1);
    }


    //-- CONFIGURATION

    /// <summary>
    /// Creates and configures the field columns.
    /// </summary>
    private void ConfigureColumns()
    {
        // Calculate the height of the field
        float fieldHeight = NumRows * TileSize;

        // Create the columns
        bool isHighColumn = false;
        for (int ii = 0; ii < NumColumns; ii++, isHighColumn = !isHighColumn)
        {
            // Create the column
            int capacity = isHighColumn ? NumRows + 1 : NumRows;
            FieldColumn column = FieldColumn.Create(capacity, ii);
            AddChild(column);
            Columns.Add(column);

            // Place the column
            float columnX = ii * TileSize + TileSize / 2;
            float columnY = fieldHeight + (isHighColumn ? TileSize / 2 : 0);
            column.Position = new Vector2(columnX, columnY);
        }
    }


    //-- TILES

    /// <summary>
    /// Instantiates a tile with the given letter and adds it to the given column.
    /// </summary>
    /// <param name="letter">The tile's letter</param>
    /// <param name="column">The tile's column</param>
    /// <returns>The newly created tile</returns>
    public Tile AddTile(char letter, int column)
	{
        // Validate
        if (DebugUtils.AssertFalse(!IsValidCharacter(letter)) ||
            DebugUtils.AssertFalse(column >= NumColumns))
        {
            return null;
        }

        // Add the tile
        return Columns[column].DropTile(letter);
    }

    /// <summary>
    /// Returns whether the letter is valid for a tile.
    /// </summary>
    /// <param name="letter">The letter to validate</param>
    /// <returns>True if the letter is valid; false otherwise</returns>
    public bool IsValidCharacter(char letter)
    {
        return true; // letter >= 'A' && letter <= 'Z';
    }


    //-- EXPORT ATTRIBUTES

    /// <summary>
    /// The number of rows in the field
    /// </summary>
    [Export]
    public int NumRows { get; set; }

    /// <summary>
    /// The number of columns in the field
    /// </summary>
    [Export]
    public int NumColumns { get; set; }

    /// <summary>
    /// The base size of the tiles
    /// </summary>
    [Export]
    public float TileSize { get; set; } = 50.0f;


    //-- ATTRIBUTES

    /// <summary>
    /// The columns in the field
    /// </summary>
    public List<FieldColumn> Columns { get; private set; } = new List<FieldColumn>();

    /// <summary>
    /// The gap between the tile and the wall as a percentage of the tile size
    /// </summary>
    public float GapPercent
    {
        get
        {
            return Math.Clamp(_gapPercent, 0, 1);
        }
        set
        {
            _gapPercent = value;
        }
    }

    /// <summary>
    /// The gap between the tile and the wall as an absolute distance
    /// </summary>
    public float GapSize
    {
        get
        {
            return GapPercent * TileSize;
        }
    }
    private float _gapPercent = 0.05f;
}
