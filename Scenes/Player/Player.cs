using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int MovementSpeed { get; set; } = 350;
	[Export]
	public int GravityVelocity { get; set; } = 200;
	[Export]
	public int jumpVelocity { get; set; } = 300;

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

		this.Velocity = velocity;
		updateAnimation(velocity);

		// MoveAndSlide();
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
			velocity.Y -= jumpVelocity * (float)delta;
		}

		velocity.X = velocity.X * (MovementSpeed * (float)delta);

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
		else if (velocity.Y > 0) {
			// Fall animation
			AnimatedSprite2D.Animation = "fall";
		}
		else if (velocity.X != 0)
		{
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
		AnimatedSprite2D.FlipH = velocity.X < 0;
	}

	private void updatePosition(Vector2 velocity, double delta) 
	{
		Position += velocity * (float)delta;
	}
}
