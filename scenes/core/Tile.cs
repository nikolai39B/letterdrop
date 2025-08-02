using Godot;
using LetterDrop.Debug;
using LetterDrop;
using System;
using System.Diagnostics.Metrics;
using System.ComponentModel;

[GlobalClass]
public partial class Tile : Button
{
    //-- ENUMS
    public enum State
    {
        None,
        Disabled,
        Pending,
        Active
    }


    //-- CREATOR

    /// <summary>
    /// Creator
    /// </summary>
    /// <returns>The created instance</returns>
    public static Tile Create()
    {
        // Instantiate
        Tile instance = SceneCatalog.Instantiate<Tile>(Type);
        if (DebugUtils.AssertFalse(instance == null))
        {
            return null;
        }

        return instance;
    }


    //-- OVERRIDES
    public override void _Ready()
    {
        // Configure the label
        StyleBox pendingStyleBox = GetThemeStylebox("pending");
        AddThemeStyleboxOverride("disabled", pendingStyleBox);
    }


    //-- EVENTS
    private void OnMouseEntered()
    {
        if (TileState == Tile.State.Pending)
        {
            // TODO: Get letter of next tile in queue
            Text = "Q";
        }
    }

    private void OnMouseExited()
    {
        if (TileState == Tile.State.Pending)
        {
            // Get letter of next tile in queue
            Text = "";
        }
    }


    //-- CONSTANTS
    public const SceneType Type = SceneType.TILE;


    //-- ATTRIBUTES

    /// <summary>
    /// The tile letter
    /// </summary>
    public char? Letter
    {
        get => _letter;
        set 
        {
            _letter = value;
            Text = _letter != null ? value.ToString() : "";
        }
    }
    private char? _letter = null;

    /// <summary>
    /// The state of the tile
    /// </summary>
    [Export]
    public State TileState
    {
        get => _tileState;
        set
        {
            _tileState = value;

            // Update the disabled state
            Disabled = _tileState == State.Disabled || _tileState == State.Pending;

            // Update the focus mode
            FocusMode = _tileState == State.Disabled ? FocusModeEnum.None : FocusModeEnum.All;

            // Configure the pending stylebox as necessary
            if (value == State.Pending)
            {
                StyleBox pendingStyleBox = GetThemeStylebox("pending");
                AddThemeStyleboxOverride("disabled", pendingStyleBox);
            }
            else if (HasThemeStyleboxOverride("disabled"))
            {
                RemoveThemeStyleboxOverride("disabled");
            }
        }
    }
    public State _tileState = State.None;

    /// <summary>
    /// The parent column
    /// </summary>
    public FieldColumn ParentColumn { get => GetParent<FieldColumn>(); }
}