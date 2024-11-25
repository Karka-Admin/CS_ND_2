using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class WallManager : Node2D
{
    [Export]
    private PackedScene WallObjectScene;
    [Export]
    private BirdController Bird;
    [Export]
    private int NumberOfWalls = 5;      // Total number of walls to generate
    [Export]
    private float InitialYPosition = 0f; // Starting Y position for the walls
    [Export]
    private float WallSpacing = 400f;    // Horizontal spacing between walls
    [Export]
    private float WallGapSize = 200f;    // Vertical gap size between walls
    [Export]
    private float MinYPosition = -200f;  // Minimum Y deviation for randomness
    [Export]
    private float MaxYPosition = 200f;   // Maximum Y deviation for randomness

    private float GameViewCenterX = 480 / 2;
    private float GameViewCenterY = 800 / 2;
    private float LastWallPositionX = 200f;

    private List<WallObject> walls = new List<WallObject>();

    public override void _Ready()
    {
        foreach (var yPos in new WallPositionGenerator(NumberOfWalls, InitialYPosition, GameViewCenterY, MinYPosition, MaxYPosition)
                               .Where(y => y > GameViewCenterY - 100))
        {
            SpawnWall(LastWallPositionX, yPos);
        }

        Bird.Scored += OnScored;
    }

    private void OnScored()
    {
        float newYPosition = GameViewCenterY + (float)GD.RandRange(MinYPosition, MaxYPosition);
        SpawnWall(LastWallPositionX, newYPosition);
    }

    private void SpawnWall(float spawnPositionX, float spawnPositionY)
    {
        var WallObject = WallObjectScene.Instantiate<WallObject>();

        WallObject.Position = new Vector2(spawnPositionX + WallSpacing, spawnPositionY);
        LastWallPositionX = WallObject.Position.X;

        walls.Add(WallObject);

        CallDeferred("add_child", WallObject);
    }
}

public class WallPositionGenerator : IEnumerable<float>, IEnumerator<float>
{
    private int _count;
    private float _initialYPosition;
    private float _gameViewCenterY;
    private float _minYPosition;
    private float _maxYPosition;
    private int _currentIndex = -1;
    private float _currentValue;

    public WallPositionGenerator(int count, float initialYPosition, float gameViewCenterY, float minYPosition, float maxYPosition)
    {
        _count = count;
        _initialYPosition = initialYPosition;
        _gameViewCenterY = gameViewCenterY;
        _minYPosition = minYPosition;
        _maxYPosition = maxYPosition;
    }

    public float Current => _currentValue;

    object IEnumerator.Current => Current;

    public IEnumerator<float> GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool MoveNext()
    {
        _currentIndex++;
        if (_currentIndex < _count)
        {
            _currentValue = _initialYPosition + (_gameViewCenterY + (float)GD.RandRange(_minYPosition, _maxYPosition));
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _currentIndex = -1;
    }

    public void Dispose()
    {
        // No resources to dispose in this case
    }
}
