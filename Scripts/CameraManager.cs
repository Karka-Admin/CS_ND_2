using Godot;
using System;

public partial class CameraManager : Node2D
{
	[Export] BirdController bird;
	[Export] Camera2D camera2D;
	public override void _Ready()
	{
		try
		{
			camera2D.Position = new Vector2(camera2D.Position.X, GetViewport().GetVisibleRect().Size.Y / 2);
			if (camera2D is null)
			{
				throw new MyCustomException("camera2D ain't defined nigga"); //rasiau sita klausydamas lendriko kamaro
			}
		}
		catch (MyCustomException e)
		{
			GD.PushError(e);
		}
	}

	public override void _Process(double delta)
	{
		camera2D.Position = new Vector2(bird.Position.X, camera2D.Position.Y);
	}
}
