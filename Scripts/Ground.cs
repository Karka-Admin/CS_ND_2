using Godot;
using System;

public partial class Ground : Area2D
{
	[Export] BirdController bird;
	private float positionY;
	private float positionX;

	public override void _Ready()
	{
		positionY = Position.Y;
	}

	public override void _Process(double delta)
	{
		Vector2 pos = bird.Position;
		pos.Y = positionY;
		Position = pos;
	}
}
