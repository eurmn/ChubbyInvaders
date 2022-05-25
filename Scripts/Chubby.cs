using Godot;
using System;

public abstract class Chubby : KinematicBody2D
{
    /// <summary>
    /// The row [1-4] in which the character will be in.
    /// Also affects the <see cref="Sprite"/> and the <see cref="Position"/> of the character. <br/>
    /// </summary>
    /// <value></value>
    protected abstract byte Row { get; }
    /// <summary>
    /// The amount of eaten cookies necessary for the <see cref="Chubby"/> to d̶i̶e̶ get satisfied.
    /// </summary>
    /// <value></value>
    protected abstract byte NeededCookies { get; }
    /// <summary>
    /// The sound the <see cref="Chubby"/> makes when it d̶i̶e̶s̶ gets satisfied.
    /// </summary>
    /// <value></value>
    protected abstract AudioStream satisfactionSFX { get; set; }
    /// <summary>
    /// Minimum position any <see cref="Chubby"/> can get.
    /// </summary>
    protected const int MIN_POSITION = 30;
    /// <summary>
    /// The position the <see cref="Chubby"/> had when it spawned.
    /// </summary>
    protected float StartPosition;
    private Area2D singularityArea;
    public Area2D DeathZone;
    private Sprite sprite;
    private AudioStreamPlayer2D SFX;
    private AudioStream blipSFX;

    private const int SPEED = 10;
    private sbyte direction = 1;
    private byte cookiesBeingEaten = 0;
    private byte cookiesEaten = 0;
    private byte eatingFrame;
    private byte idleFrame;
    private bool disabled = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Generate Sprite.
        sprite = new Sprite();
        sprite.Texture = (Texture)GD.Load("res://Assets/Sprites/chubbies.png");
        sprite.Hframes = 9;
        sprite.Vframes = 3;

        // Generate singularity area.
        singularityArea = new Area2D();
        // See: https://docs.godotengine.org/pt_BR/stable/tutorials/physics/physics_introduction.html#code-example
        singularityArea.CollisionMask = 3; // Set the mask to be 2 (2^2 - 1).
        var singularityCollision = new CollisionShape2D();
        singularityArea.AddChild(singularityCollision);
        var singularityShape = new CircleShape2D();
        singularityShape.Radius = 9;
        singularityCollision.Shape = singularityShape;

        // Generate the death zone area (where cookies die D:).
        DeathZone = new Area2D();
        DeathZone.CollisionMask = 3; // Set the mask to be 2 (2^2 - 1).
        var DeathZoneCollision = new CollisionShape2D();
        DeathZone.AddChild(DeathZoneCollision);
        var DeathZoneShape = new CircleShape2D();
        DeathZoneShape.Radius = 3;
        DeathZoneCollision.Shape = DeathZoneShape;

        // Generate Audio Player.
        SFX = new AudioStreamPlayer2D();
        blipSFX = (AudioStream)GD.Load("res://Assets/Audios/blip.mp3");
        SFX.Stream = blipSFX;
        SFX.VolumeDb = -10;

        // Add children.
        AddChild(sprite);
        AddChild(singularityArea);
        AddChild(DeathZone);
        AddChild(SFX);

        // Set up the cookie interaction.
        singularityArea.Connect("body_entered", this, nameof(GrabCookie));

        switch (Row)
        {
            case 4:
                idleFrame = 1;
                eatingFrame = 9;
                break;
            case 3:
                idleFrame = 7;
                eatingFrame = 17;
                break;
            case 2:
                idleFrame = 3;
                eatingFrame = 11;
                break;
            case 1:
                idleFrame = 5;
                eatingFrame = 15;
                break;
        }

        sprite.Frame = idleFrame;
        Position = new Vector2(Position.x, Row * 30);
    }

    public void GrabCookie(Node body)
    {
        if (body is CookieMissile)
        {
            var cookie = body as CookieMissile;
            if (cookie.ReachedSingularity)
            {
                return;
            }
            cookie.ReachSingularity(this);
            StartEating();

            DeathZone.Connect("body_entered", cookie, nameof(CookieMissile.GetEaten));
        }
    }

    public void StartEating()
    {
        cookiesBeingEaten++;
        sprite.Frame = eatingFrame;
    }

    public void StopEating()
    {
        cookiesBeingEaten--;
        cookiesEaten++;
        SFX.Play();
        // Don't close the mouth if there are cookies still coming.
        if (cookiesBeingEaten == 0)
        {
            sprite.Frame = idleFrame;
        }
        if (cookiesEaten == NeededCookies)
        {
            // Play satisfaction sound effect.
            var sSFX = new AudioStreamPlayer2D();
            sSFX.Stream = satisfactionSFX;
            sSFX.VolumeDb = -10;
            sSFX.Autoplay = true;
            GetTree().Root.AddChild(sSFX);

            // Satisfaction particle.
            var particle = new Sprite();
            particle.Texture = (Texture)GD.Load("res://Assets/Sprites/chubbies.png");
            particle.Hframes = 9;
            particle.Vframes = 3;
            particle.Frame = 19;
            particle.Position = GlobalPosition;
            GetTree().Root.AddChild(particle);

            sSFX.Connect("finished", sSFX, "queue_free");
            sSFX.Connect("finished", particle, "queue_free");

            // Delete itself.
            QueueFree();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!disabled)
        {
            if (Position.x >= StartPosition + 56) // Time to turn.
            {
                Position = new Vector2(StartPosition + 56, Position.y + 30);
                direction = -1;
            } else if (Position.x <= StartPosition && direction == -1)
            {
                Position = new Vector2(StartPosition, Position.y - 30);
                direction = 1;
            }
            MoveAndSlide(new Vector2(SPEED * direction, 0));
        }
    }
}
