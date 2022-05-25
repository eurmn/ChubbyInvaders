using Godot;
using System;

public class ShibaInu : Chubby
{
    // Declare member variables here.
    protected override byte Row => 1;

    protected override byte NeededCookies => 20;

    protected override AudioStream satisfactionSFX { get; set; }

    private static int ShibaInuCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        ShibaInuCount++;
        Position = new Vector2(MIN_POSITION + 20 * (ShibaInuCount - 1), Position.y);
        StartPosition = Position.x;

        satisfactionSFX = (AudioStream)GD.Load("res://Assets/Audios/woof.mp3");
    }
}
