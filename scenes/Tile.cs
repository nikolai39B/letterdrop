using Godot;
using LetterDrop.Debug;
using LetterDrop;
using System;

public partial class Tile : Control
{
    //-- CREATOR

    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="letter">The letter of the tile</param>
    /// <returns>The instance</returns>
    public static Tile Create(char letter)
    {
        // Instantiate
        Tile instance = SceneCatalog.Instantiate<Tile>(Type);
        if (DebugUtils.AssertFalse(instance == null))
        {
            return null;
        }

        // Initialize
        instance._letter = letter;

        return instance;
    }


    //-- OVERRIDES
    public override void _Ready()
    {
        // Set a descriptive name
        Name = $"Tile{ParentColumn.Number}-{ParentColumn.TotalTiles}";
    }


    //-- CONSTANTS
    public const SceneType Type = SceneType.TILE;


    //-- INITIALIZATION ATTRIBUTES

    /// <summary>
    /// The tile letter
    /// </summary>
    public char Letter { get => _letter; }
    private char _letter;


    //-- ATTRIBUTES

    /// <summary>
    /// The parent field
    /// </summary>
    public FieldColumn ParentColumn { get => GetParent<FieldColumn>(); }
}
