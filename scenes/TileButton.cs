using Godot;
using System;

public partial class TileButton : Button
{
    //-- OVERRIDES
	public override void _Ready()
	{
		// Configure the label
		Tile tile = GetParent<Tile>();
		Text = tile?.Letter.ToString();

        // Configure the scale
        Field field = tile.ParentColumn.ParentField;
        float size = field.TileSize - field.GapSize * 2;
        Size = new Vector2(size, size);
        Position = new Vector2(-size / 2, -size / 2);
        PivotOffset = new Vector2(size / 2, size / 2);
        //float scale = size / Size.X;
        //Scale = new Vector2(scale, scale);

        // Reset the border
        //StyleBox styleBox = GetThemeStylebox("normal");
        //if (styleBox is StyleBoxFlat flat)
        //{
        //    flat.SetBorderWidthAll(1);
        //}
        //AddThemeStyleboxOverride("normal", styleBox);
    }
}
