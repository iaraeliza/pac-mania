using UnityEngine;

public abstract class Deadly : GhostAIMachineState
{
	public Deadly(GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
		: base(ghostAI, ghostMove, pacman)
	{

	}

	public override void Enter()
	{
		GhostAI.OnCollideWithPacman += GhostAI_OnCollideWithPacman;
		base.Enter();
	}

	private void GhostAI_OnCollideWithPacman(Collider2D pacman)
	{
		pacman.GetComponent<Life>().RemoveLife();
	}

	public override void Exit()
	{
		GhostAI.OnCollideWithPacman -= GhostAI_OnCollideWithPacman;
		base.Exit();
	}
}
