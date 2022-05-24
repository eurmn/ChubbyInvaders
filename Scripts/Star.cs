using Godot;
using System;

public class Star : Node2D
{
    // Declare member variables here.
    private float timePassed = 0;
    private float circleRadius = 1f;
    private float starSizeInterval;
    private float starSizeTimer;
    private RandomNumberGenerator RNG = new RandomNumberGenerator();
    private Color starColor;
    private Vector2 position = new Vector2();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RNG.Randomize();
        starSizeInterval = RNG.RandfRange(3, 10);
        starSizeTimer = starSizeInterval;
        RNG.Randomize();
        starColor = new Color(1, 1, 1, RNG.RandfRange(.1f, .8f));

        RNG.Randomize();
        var viewport = GetViewportRect().Size;
        position = new Vector2(RNG.RandfRange(0, viewport.x), RNG.RandfRange(0, viewport.y)).Floor();
    }

    public override void _PhysicsProcess(float delta)
    {
        starSizeTimer -= delta;
        if (starSizeTimer <= 0)
        {
            if (circleRadius == 1)
            {
                circleRadius = 2;
            } else
            {
                circleRadius = 1;
            }
            starSizeTimer = starSizeInterval;
            Update();
        }
    }

    public override void _Draw()
    {
        DrawCircle(position, circleRadius, starColor);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
