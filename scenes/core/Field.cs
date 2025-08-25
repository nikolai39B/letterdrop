using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;

public partial class Field : Control
{
    //-- OVERRIDES
    public override void _Ready()
    {
        // Center the field in the left half of the viewport
        Vector2 viewportSize = GetViewportRect().Size;
        Size = new Vector2(NumRows * TileSize, (NumColumns + 1) * TileSize);
        Position = new Vector2(viewportSize.X / 4 - Size.X / 2, viewportSize.Y / 2 - Size.Y / 2);

        // Create the columns
        ConfigureColumns();

        // Debug
        //AddTile('A', 0);
        //AddTile('Z', 1);
        //AddTile('Y', 1);
        //AddTile('C', 2);
        ////AddTile('D', 3);
        //AddTile('X', 4);
        //AddTile('M', 5);
        //AddTile('N', 5);
        //AddTile('O', 5);
        //AddTile('P', 5);
        //AddTile('Q', 5);
        //AddTile('R', 5);
        //AddTile('S', 5);
        //AddTile('T', 5);
        //AddTile('1', 6);
        //AddTile('2', 6);
        //AddTile('3', 6);
        //AddTile('4', 6);
        //AddTile('5', 6);
        //AddTile('6', 6);
        //AddTile('7', 6);
    }


    //-- CONFIGURATION

    /// <summary>
    /// Creates and configures the field columns.
    /// </summary>
    private void ConfigureColumns()
    {
        // Create the columns
        bool isHighColumn = false;
        for (int ii = 0; ii < NumColumns; ii++, isHighColumn = !isHighColumn)
        {
            // Create the column
            int capacity = isHighColumn ? NumRows + 1 : NumRows;
            FieldColumn column = FieldColumn.Create(capacity);
            AddChild(column);
            Columns.Add(column);

            // Set a descriptive name
            Name = $"Column{ii}";

            // Place the column
            float columnX = ii * TileSize;
            float columnY = isHighColumn ? 0 : TileSize / 2;
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
    public void AddTile(char letter, int column)
	{
        // Validate
        if (DebugUtils.AssertFalse(!IsValidCharacter(letter)) ||
            DebugUtils.AssertFalse(column >= NumColumns))
        {
            return;
        }

        // Add the tile
        Columns[column].DropTile(letter);
    }

    /// <summary>
    /// Returns whether the letter is valid for a tile.
    /// </summary>
    /// <param name="letter">The letter to validate</param>
    /// <returns>True if the letter is valid; false otherwise</returns>
    public bool IsValidCharacter(char letter)
    {
        return letter >= 'A' && letter <= 'Z';
    }

    public bool CanSelectTile(int column, int tile)
    {
        // TODO implement
        return true;
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
