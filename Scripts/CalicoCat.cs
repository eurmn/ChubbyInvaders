using Godot;
using System;

public class CalicoCat : Chubby
{
    // Declare member variables here.
    protected override byte Row => 2;

    protected override byte NeededCookies => 13;

    protected override AudioStream satisfactionSFX { get; set; }

    private static int CalicoCatCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        CalicoCatCount++;
        Position = new Vector2(MIN_POSITION + 20 * (CalicoCatCount - 1), Position.y);
        StartPosition = Position.x;

        satisfactionSFX = (AudioStream)GD.Load("res://Assets/Audios/meow.mp3");
    }
}
