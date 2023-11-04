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

	private Vector2 ScreenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
		var velocity = inputVelocity(delta);
		velocity = addGravity(velocity, delta);	// Player movement vector

		velocity.Y += this.Velocity.Y;
		this.Velocity = velocity;
		MoveAndSlide();

		updateAnimation(this.Velocity);

		clampPosition();
	}

	private Vector2 inputVelocity(double delta) 
	{
		var velocity = Vector2.Zero;	// Player movement vector

		if (Input.IsActionPressed("move_left")) {
			// GD.Print("Move Left");
			velocity.X -= 1;
		}
		else if (Input.IsActionPressed("move_right")) {
			velocity.X += 1;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
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

	private void clampPosition() 
	{
		var position = Position;
		position.X = Mathf.Clamp(position.X, 0, ScreenSize.X);
		position.Y = Mathf.Clamp(position.Y, 0, ScreenSize.Y);
		Position = position;
	}
}
