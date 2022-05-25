using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    private float speed = 50f;
    // 0 = idle. -1 = left. 1 = right.
    private sbyte direction = 0;
    private Vector2 velocity = new Vector2(0, 0);
    private PackedScene cookieMissile;
    private bool shootQueued = false;
    private float cookieCooldown = .3f;
    private float cookieCooldownTimer = 0;
    private AudioStreamPlayer2D SFX;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cookieMissile = GD.Load<PackedScene>("res://Scenes/CookieMissile.tscn");
        GetNode<Sprite>("Fire").GetNode<AnimationPlayer>("FireAnimPlayer").Play("Active");
        SFX = GetNode<AudioStreamPlayer2D>("SFX");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_left"))
        {
            direction = -1;
        } else if (Input.IsActionPressed("ui_right"))
        {
            direction = 1;
        } else
        {
            direction = 0;
        }

        if (Input.IsActionPressed("shoot"))
        {
            if (cookieCooldownTimer <= 0)
            {
                shootQueued = true;
                cookieCooldownTimer = cookieCooldown;
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (cookieCooldownTimer > 0)
        {
            cookieCooldownTimer -= delta;
            if (shootQueued)
            {
                Shoot();
                shootQueued = false;
            }
        }
        velocity = new Vector2(speed * direction, 0);
        MoveAndSlide(velocity);
    }

    private void Shoot()
    {
        var newCookie = cookieMissile.Instance<CookieMissile>();
        newCookie.Position = Position;
        GetTree().Root.AddChild(newCookie);
        SFX.Play();
    }
}
