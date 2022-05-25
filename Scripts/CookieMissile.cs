using Godot;
using System;

public class CookieMissile : KinematicBody2D
{
    public bool ReachedSingularity = false;
    private int speed = 125;
    private Vector2 velocity = new Vector2(0, 0);
    private Chubby eater;
    private bool gettingEatten = false;
    private float particleTimer = .5f;

    /// <summary>
    /// Force the cookie to move towards the center of its eater.
    /// </summary>
    /// <param name="eater">The <see cref="Chubby"/> who ate the cookie.</param>
    public void ReachSingularity(Chubby eater)
    {
        this.eater = eater;
        ReachedSingularity = true;
    }

    public void GetEaten(Node _body)
    {
        eater.StopEating();
        gettingEatten = true;
        GetNode<Sprite>("Sprite").Frame = 22;
        eater.DeathZone.Disconnect("body_entered", this, nameof(GetEaten));
    }

    public override void _PhysicsProcess(float delta)
    {
        if (gettingEatten)
        {
            particleTimer -= delta;
            if (particleTimer <= 0)
            {
                QueueFree();
            }
            return;
        }
        if (Position.y <= -10)
        {
            QueueFree();
            return;
        }
        if (ReachedSingularity)
        {
            velocity = Position.DirectionTo(eater.Position) * speed / 5;
        } else
        {
            velocity = new Vector2(0, -speed);
        }
        MoveAndSlide(new Vector2(velocity));
    }
}
