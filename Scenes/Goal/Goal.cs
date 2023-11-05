using Godot;
using System;

public partial class Goal : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += bodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void bodyEntered(Node2D body) {
		if (body.Name == "Player") {
			GD.Print("Player entered");
		} else {
			GD.Print("Something entered");
		}
	}
}
