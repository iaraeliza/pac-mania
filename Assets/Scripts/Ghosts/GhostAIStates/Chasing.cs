using UnityEngine;

public class Chasing : Deadly
{
	public Chasing(GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
		: base(ghostAI, ghostMove, pacman)
	{
		Name = GhostAIState.Chasing;
	}

	public override void Enter()
	{
		GhostMove.OnUpdateMoveTarget += GhostMove_OnUpdateMoveTarget;
		GhostAI.OnSetVulnerable += GhostAI_OnSetVulnerable;
		base.Enter();
	}

	private void GhostMove_OnUpdateMoveTarget()
	{
		GhostMove.SetTargetMoveLocation(Pacman.position);
	}

	private void GhostAI_OnSetVulnerable(float timer)
	{
		ChangeState(new Vulnerable(timer, GhostAI, GhostMove, Pacman));
	}

	public override void Exit()
	{
		GhostMove.OnUpdateMoveTarget -= GhostMove_OnUpdateMoveTarget;
		GhostAI.OnSetVulnerable -= GhostAI_OnSetVulnerable;
		base.Exit();
	}
}
