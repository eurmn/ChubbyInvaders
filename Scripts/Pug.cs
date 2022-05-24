using Godot;
using System;

public class Pug : Chubby
{
    // Declare member variables here.
    protected override byte Row => 3;

    protected override byte NeededCookies => 8;

    private static int PugCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        PugCount++;
        Position = new Vector2(MIN_POSITION + 20 * (PugCount - 1), Position.y);
        StartPosition = Position.x;
    }
}
