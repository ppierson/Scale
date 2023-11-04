using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int MovementSpeed { get; set; } = 250;
	[Export]
	public int GravityVelocity { get; set; } = 600;
	[Export]
	public int jumpVelocity { get; set; } = 500;

	private AnimatedSprite2D AnimatedSprite2D = null; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
		var velocity = inputVelocity(delta);
		velocity = addGravity(velocity, delta);	// Player movement vector

		GD.Print($"Velocity: X - {velocity.X}, Y - {velocity.Y}");

		updateAnimation(velocity);

		velocity.Y += this.Velocity.Y;
		this.Velocity = velocity;
		MoveAndSlide();
	}

	private Vector2 inputVelocity(double delta) 
	{
		var velocity = Vector2.Zero;	// Player movement vector

		if (Input.IsActionPressed("move_left")) 
		{
			// GD.Print("Move Left");
			velocity.X -= 1;
		}
		else if (Input.IsActionPressed("move_right")) 
		{
			// GD.Print("Move Right");
			velocity.X += 1;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor()) 
		{
			velocity.Y = -jumpVelocity;
		}

		velocity.X = velocity.X * MovementSpeed;

		return velocity;
	}

	private Vector2 addGravity(Vector2 velocity, double delta) 
	{
		var vector = velocity;
		vector.Y += GravityVelocity * (float)delta;
		return vector;
	}

	private void updateAnimation(Vector2 velocity) 
	{
		if (velocity.Y < 0) {
			// Jump animation
			AnimatedSprite2D.Animation = "jump";
		}
		else if (velocity.Y > 0 && !IsOnFloor()) {
			// Fall animation
			AnimatedSprite2D.Animation = "fall";
		}
		else if (velocity.X != 0) {
			// Run animation
			AnimatedSprite2D.Animation = "run";
			AnimatedSprite2D.Play();
		}
		else {
			// Idle.
			AnimatedSprite2D.Animation = "idle";
			AnimatedSprite2D.Play();
		}

		// Flip direction for X velocity
		if (velocity.X != 0) {
			AnimatedSprite2D.FlipH = velocity.X < 0;
		}
	}

	private void updatePosition(Vector2 velocity, double delta) 
	{
		Position += velocity * (float)delta;
	}
}
