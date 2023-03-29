using UnityEngine;

public class LeavingHouse : Deadly
{
	public LeavingHouse(GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
		: base(ghostAI, ghostMove, pacman)
	{
		Name = GhostAIState.LeavingHouse;
	}

	public override void Enter()
	{
		GhostMove.CharacterMotor.CollideWithGates(false);
		GhostMove.SetTargetMoveLocation(new Vector3(0, 3, 0));

		GhostMove.OnUpdateMoveTarget += GhostMove_OnUpdateMoveTarget;

		base.Enter();
	}

	private void GhostMove_OnUpdateMoveTarget()
	{
		if (GhostAI.transform.position == new Vector3(0, 3, 0))
		{
			ChangeState(new Chasing(GhostAI, GhostMove, Pacman));
		}
	}

	public override void Exit()
	{
		GhostMove.OnUpdateMoveTarget -= GhostMove_OnUpdateMoveTarget;
		GhostMove.CharacterMotor.CollideWithGates(true);
		base.Exit();
	}
}
