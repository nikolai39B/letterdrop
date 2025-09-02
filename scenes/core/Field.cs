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
    /// Gets the tile at the given column and tile number.
    /// </summary>
    /// <param name="cc">The column number</param>
    /// <param name="tt">The tile number in the column</param>
    /// <returns>The tile, or null if there is no tile</returns>
    private FieldTile GetTile(int cc, int tt)
    {
        // Get the column
        if (DebugUtils.AssertFalse(cc < 0 || cc >= NumColumns))
        {
            return null;
        }
        var column = Columns[cc];

        // Get the tile
        if (DebugUtils.AssertFalse(tt < 0 || tt >= column.ActiveTilesCount))
        {
            return null;
        }
        var tile = column.Tiles[tt];
        return tile;
    }

    /// <summary>
    /// Instantiates a tile with the given letter and adds it to the given column.
    /// </summary>
    /// <param name="letter">The tile's letter</param>
    /// <param name="cc">The tile's column</param>
    /// <returns>The newly created tile</returns>
    public void AddTile(char letter, int cc)
	{
        // Validate
        if (DebugUtils.AssertFalse(!IsValidCharacter(letter)) ||
            DebugUtils.AssertFalse(cc >= NumColumns))
        {
            return;
        }

        // Add the tile
        Columns[cc].DropTile(letter);
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

    /// <summary>
    /// Checks if the given tiles are adjacent.
    /// </summary>
    /// <param name="tile1">The first tile to check</param>
    /// <param name="tile2">The second tile to check</param>
    /// <returns>True if the tiles are adjacent; false otherwise</returns>
    public bool AreTilesAdjacent(FieldTile tile1, FieldTile tile2)
    {
        // Check null and equality
        if (tile1 == null || tile2 == null || tile1 == tile2)
        {
            return false;
        }

        // Get the tile numbers
        int tt1 = tile1.GetTileNumber();
        int tt2 = tile2.GetTileNumber();

        // Handle same column
        if (tile1.Column == tile2.Column)
        {
            return Math.Abs(tt1 - tt2) == 1;
        }

        // Handle adjacent columns
        int cc1 = tile1.Column.GetColumnNumber();
        int cc2 = tile2.Column.GetColumnNumber();
        if (Math.Abs(cc1 - cc2) == 1)
        {
            // Since the columns are adjacent, one should be short and one should be long - determine which is which
            if (DebugUtils.AssertFalse(tile1.Column.IsTallColumn() == tile2.Column.IsTallColumn()))
            {
                return false;
            }
            (int ttLong, int ttShort) = tile1.Column.IsTallColumn() ? (tt1, tt2) : (tt2, tt1);

            // The tiles are adjacent if one of the following is true:
            //   - short column tile number == long column tile number
            //   - short column tile number == long column tile number + 1
            return ttShort == ttLong || ttShort == ttLong + 1;
        }

        // If we get here, the tiles aren't adjacent
        return false;
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
