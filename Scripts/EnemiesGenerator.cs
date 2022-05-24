using Godot;
using System;

public class EnemiesGenerator : Node2D
{
    // Declare member variables here. Examples:
    private const sbyte CHUBBYBYROW = 8;
    private uint distance = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < CHUBBYBYROW; i++)
        {
            var greyCat = new GreyCat();
            AddChild(greyCat);
        }
        for (int i = 0; i < CHUBBYBYROW; i++)
        {
            var pug = new Pug();
            AddChild(pug);
        }
        for (int i = 0; i < CHUBBYBYROW; i++)
        {
            var calicoCat = new CalicoCat();
            AddChild(calicoCat);
        }
        for (int i = 0; i < CHUBBYBYROW; i++)
        {
            var shibaInu = new ShibaInu();
            AddChild(shibaInu);
        }
    }
}
