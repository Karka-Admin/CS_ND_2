using Godot;
using System;
using System.Threading.Tasks;

public partial class BirdController : CharacterBody2D, IFormattable
{
	// By naming conventions annotacijos arba siuo atveju C# atitikmuo - atributas. Turi buti virs variable ar methodo o ne buti is kaires jam.
	[Signal]
	public delegate void DiedEventHandler();
	[Signal]
	public delegate void ScoredEventHandler(); // Score signala turetu emtinti score manager, cia yra didziausia bezambrazija, nu bet tarkim... sunku neapsivemt sita matant
	[Export]
	public float speed = 175f;
	[Export]
	public float jumpSpeed = 350f;
	[Export]
	public float gravity = 800f;
	[Export]
	public float restartCooldown = 0.25f;

	private bool isDead = false;

	public int Score = 0;

	static BirdController()
	{
		GD.Print("Bird created.");
		RenderingServer.SetDefaultClearColor(new Color("A8AD52"));
	}

	public void Deconstruct(out Vector2 velocity)
	{
		velocity = Velocity;
	}

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenTouch touchEvent && touchEvent.IsPressed())
		{
			Jump();
		}
	}

	public override void _Process(double delta)
	{

		if(isDead)
		{
			return;
		}

		// Speed kiekviena frame yra constant, todel jo nereikia daugint is deltos, 
		// kol tuo tarpu y direkcija kiekviena frame yra skirtinga, todel ja reikai daugint is deltos. 
		Velocity = new Vector2(speed, (float)(Velocity.Y + gravity * delta));

		SetVelocity(Velocity);
		MoveAndSlide();

		if(Position.Y > GetViewport().GetVisibleRect().Size.Y || Position.Y < 0)
		{
			OnDied();
		}
	}

    private void Jump()
	{
		Velocity = new Vector2(Velocity.X, -jumpSpeed);
	}

	public async void OnDied()
	{
		isDead = true;
		GD.Print("DIED");
		EmitSignal(SignalName.Died);

		await Task.Delay(TimeSpan.FromSeconds(restartCooldown));

		GetTree().CallDeferred("reload_current_scene"); // Defers the reload to avoid immediate interruption
	}


	public void OnScored()
	{
		GD.Print("SCORED");
		
		// Fancy Shmancy sudas kad vietoj score++ naudot bitwise operations.
		int increment = 1;
		while (increment != 0)
		{
			int carry = Score & increment;  // Calculate the carry
			Score = Score ^ increment;      // XOR to add without the carry
			increment = carry << 1;         // Shift the carry to the left
		}

		EmitSignal(SignalName.Scored);
	}
	public string ToString(string format, IFormatProvider formatProvider)
	{
		if (string.IsNullOrEmpty(format)) format = "G";
		switch (format.ToUpperInvariant())
		{
			case "G": // General format
				return $"BirdController:\n" +
					$"Velocity = ({Velocity.X.ToString("F2")}, {Velocity.Y.ToString("F2")})\n" +
					$"Score = {Score}\n" +
					$"IsDead = {isDead}";
			case "S": // Short format
				return $"Velocity = ({Velocity.X.ToString("F2")}, {Velocity.Y.ToString("F2")}), Score = {Score}";
			case "D": // Detailed format
				return $"Bird Debug ->\n" +
					$"Position: ({Position.X.ToString("F2")}, {Position.Y.ToString("F2")})\n" +
					$"Velocity: ({Velocity.X.ToString("F2")}, {Velocity.Y.ToString("F2")})\n" +
					$"Restart Cooldown: {restartCooldown}\n" +
					$"Score: {Score}\n" +
					$"Is Dead: {isDead}";
			default:
				throw new FormatException($"The format '{format}' is not supported.");
		}
	}
}
