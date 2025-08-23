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
        Disabled,
        Pending,
        Active,
        Hovered,
        Selected
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


    //-- OVERRIDES
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


    //-- CONSTANTS
    public const SceneType Type = SceneType.FIELD_TILE;


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
            if (_tileState == value)
            {
                return;
            }

            // Cache the old state and set the new state
            State oldState = _tileState;
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

            // Configure the pending stylebox as necessary
            switch (_tileState)
            {
                case State.Disabled:
                    RemoveThemeStyleboxOverride("disabled");
                    break;

                case State.Pending:
                    AddThemeStyleboxOverride("normal", GetThemeStylebox("pending"));
                    break;

                case State.Active:
                    RemoveThemeStyleboxOverride("normal");
                    break;

                case State.Hovered:
                    RemoveThemeStyleboxOverride("hover");
                    break;

                case State.Selected:
                    AddThemeStyleboxOverride("normal", GetThemeStylebox("pressed"));
                    break;

                default:
                    DebugUtils.Assert(false);
                    break;
            }
        }
    }
    public State _tileState = State.None;

    /// <summary>
    /// The parent column
    /// </summary>
    public FieldColumn Column { get => GetParent<FieldColumn>(); }
}