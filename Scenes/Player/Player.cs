using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 350;
	private AnimatedSprite2D AnimatedSprite2D = null; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		checkInput(delta);
	}

	private void checkInput(double delta) 
	{
		var velocity = velocityForCurrentInput();	// Player movement vector

		updateAnimation(velocity);
		updatePosition(velocity, delta);
	}

	private Vector2 velocityForCurrentInput()
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

		if (velocity.Length() > 0)
    	{
        	velocity = velocity.Normalized() * Speed;
    	}

		return velocity;
	}

	private void updateAnimation(Vector2 velocity) {
		if (velocity.X != 0)
		{
			AnimatedSprite2D.Animation = "run";
			AnimatedSprite2D.FlipH = velocity.X < 0;
			AnimatedSprite2D.Play();
		}
		else {
			AnimatedSprite2D.Animation = "idle";
			AnimatedSprite2D.Play();
		}
	}

	private void updatePosition(Vector2 velocity, double delta) {
		Position += velocity * (float)delta;
	}
}
