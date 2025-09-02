using Godot;
using LetterDrop;
using LetterDrop.Debug;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.Metrics;

[GlobalClass]
public partial class FieldTile : Button
{
    //-- ENUMS
    public enum State
    {
        None,
        Disabled,  // Blank tile that cannot be interacted with
        Pending,   // Blank tile that can be chosen for the next letter drop
        Active,    // Letter tile that is not submitted
        Submitted  // Letter tile that is submitted
    }


    //-- SIGNALS
    //public event Action<FieldTile> OnTilePressed;
    //public event Action<FieldTile> OnMouseEnteredTile;
    //public event Action<FieldTile> OnMouseExitedTile;


    //-- CREATOR

    /// <summary>
    /// Creator
    /// </summary>
    /// <returns>The created instance</returns>
    public static FieldTile Create()
    {
        // Instantiate
        FieldTile instance = SceneCatalog.Instantiate<FieldTile>(Type);
        if (DebugUtils.AssertFalse(instance == null))
        {
            return null;
        }

        return instance;
    }


    //-- OVERRIDES

    public override void _Ready()
    {
        // Set the tile size
        Field field = Column?.Field;
        float width = field.TileSize - field.GapSize * 2;
        Size = new Vector2(width, width);
        PivotOffset = new Vector2(width / 2, width / 2);
    }

    private void OnMouseEntered()
    {
        if (TileState == State.Pending)
        {
            char? letter = Arena.Instance?.Queue?.NextTile;
            if (letter != null)
            {
                Text = letter.ToString();
            }
        }
    }

    private void OnMouseExited()
    {
        if (TileState == State.Pending)
        {
            Text = "";
        }
    }

    private void OnPressed()
    {
        // Handle pending tiles
        if (TileState == FieldTile.State.Pending)
        {
            // Drop a tile from the queue
            char? letter = Arena.Instance.Queue.PopNextTile();
            if (letter == null)
            {
                return;
            }
            Column.DropTile(letter.Value);
        }

        // Handle active tiles
        else if (TileState == FieldTile.State.Active)
        {
            // If the tile isn't submittable, do nothing
            if (!Arena.Instance.CanSubmitTile(this))
            {
                return;
            }

            // Submit the tile
            Arena.Instance.SubmitTile(this);
        }

        // Handle selected tiles
        else if (TileState == FieldTile.State.Submitted)
        {
            // Desubmit the tile
            Arena.Instance.UnsubmitTile(this);
        }
    }


    //-- ACCESSORS

    /// <summary>
    /// Gets the number of the tile in the parent column.
    /// </summary>
    /// <returns>The tile number</returns>
    public int GetTileNumber()
    {
        return Column.Tiles.IndexOf(this);
    }


    //-- CONSTANTS
    public const SceneType Type = SceneType.FIELD_TILE;


    //-- COMPONENTS
    public FieldColumn Column { get => GetParent<FieldColumn>(); }


    //-- EXPORT ATTRIBUTES

    /// <summary>
    /// The state of the tile
    /// </summary>
    [Export]
    public State TileState
    {
        get => _tileState;
        set
        {
            // If the state is already set, nothing to do
            if (_tileState == value)
            {
                return;
            }

            // Cache the old tile state and set the new tile state
            State oldTileState = _tileState;
            _tileState = value;

            // Update the disabled state
            bool disabled = _tileState == State.Disabled;
            if (disabled != Disabled)
            {
                Disabled = disabled;
            }

            // Update the focus mode
            var focusMode = _tileState == State.Disabled ? FocusModeEnum.None : FocusModeEnum.All;
            if (focusMode != FocusMode)
            {
                FocusMode = focusMode;
            }

            // Remove style overrides as necessary
            switch (oldTileState)
            {
                case State.Pending:
                    RemoveThemeStyleboxOverride("normal");
                    RemoveThemeStyleboxOverride("hover");
                    break;

                case State.Submitted:
                    RemoveThemeStyleboxOverride("normal");
                    break;
            }

            // Add style overrides as necessary
            switch (_tileState)
            {
                case State.Pending:
                    AddThemeStyleboxOverride("normal", GetThemeStylebox("pending"));
                    AddThemeStyleboxOverride("hover", GetThemeStylebox("pending"));
                    break;

                case State.Submitted:
                    AddThemeStyleboxOverride("normal", GetThemeStylebox("pressed"));
                    break;

                default:
                    DebugUtils.Assert(false);
                    break;
            }
        }
    }
    public State _tileState = State.None;


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
}
