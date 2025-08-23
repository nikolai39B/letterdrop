using Godot;
using LetterDrop.Debug;
using System;

public partial class Arena : Node2D
{
    //-- OVERRIDES
    public override void _Ready()
    {
        // Set the instance as only one arena is expected at a time
        DebugUtils.Assert(Instance == null);
        Instance = this;
    }

    public override void _ExitTree()
    {
        // Clear the instance
        Instance = null;
    }


    //-- STATIC ATTRIBUTES
    public static Arena Instance { get; private set; }


    //-- ATTRIBUTES

    public Field Field { get { return GetNode<Field>("Field"); } }

    public Queue Queue { get { return GetNode<Queue>("Queue"); } }
}
