using Godot;
using System;

public class GreyCat : Chubby
{
    // Declare member variables here.
    protected override byte Row => 4;

    protected override byte NeededCookies => 5;

    private static int GreyCatCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        GreyCatCount++;
        Position = new Vector2(MIN_POSITION + 20 * (GreyCatCount - 1), Position.y);
        StartPosition = Position.x;
    }
}
