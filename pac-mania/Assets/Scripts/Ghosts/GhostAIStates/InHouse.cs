using UnityEngine;

public class InHouse : GhostAIMachineState
{
	public InHouse(GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
		: base(ghostAI, ghostMove, pacman)
	{
		Name = GhostAIState.InHouse;
	}

	public override void Enter()
	{
		GhostAI.OnLeaveHouse += GhostAI_OnLeaveHouse;
		base.Enter();
	}

	private void GhostAI_OnLeaveHouse()
	{
		ChangeState(new LeavingHouse(GhostAI, GhostMove, Pacman));
	}

	public override void Exit()
	{
		GhostAI.OnLeaveHouse -= GhostAI_OnLeaveHouse;
		base.Exit();
	}
}
