using Godot;
using System;

public partial class Player : Area2D
{
	private AnimatedSprite2D animatedSprite2D = null; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		checkInput();
	}

	private void checkInput() 
	{
		var velocity = Vector2.Zero;	// Player movement vector

		if (Input.IsActionPressed("move_left")) 
		{
			GD.Print("Move Left");
			velocity.X -= 1;
		}
		else if (Input.IsActionPressed("move_right")) 
		{
			GD.Print("Move Right");
			velocity.X += 1;
		}

		if (velocity.Length() > 0)
    	{
        	// velocity = velocity.Normalized() * Speed;
        	animatedSprite2D.Play();
    	}
    	else
   	 	{
        	animatedSprite2D.Stop();
   		}

		updateAnimation(velocity);
	}

	private void updateAnimation(Vector2 velocity) {
		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "run";
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else {
			animatedSprite2D.Animation = "idle";
		}
	}
}
