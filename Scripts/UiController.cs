using Godot;
using System;

public partial class UiController : Control
{
    [Export]
    public Label ScoreLabelReference;
	[Export]
	public Label DebugLabel;

    private Label ScoreLabel = null;

    [Export]
    public BirdController Bird;

    public override void _Ready()
    {
        Bird.Scored += OnScored;

        ScoreLabel ??= ScoreLabelReference; // Jei score label yra null, tik tuo atveju jam assignini ta reference, jeigu jis nera null, sita kodo eilute ignorinama
    }
    public override void _PhysicsProcess(double delta)
    {
		DebugLabel.SetText(Bird.ToString("D", null));
    }
    private void OnScored()
    {
        string formattedScore = Bird.Score.ToString();
        ScoreLabel.SetText(formattedScore);

        // Example of using 'switch' with 'when'
        switch (Bird.Score)
        {
            case int score when score >= 25:
                GD.Print("Impressive score!");
                break;
            case int score when score < 25 && score >= 10:
                GD.Print("Good score!");
                break;
            default:
                GD.Print("Keep trying!");
                break;
        }
    }
}
