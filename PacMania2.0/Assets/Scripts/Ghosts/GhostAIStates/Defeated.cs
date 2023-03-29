using UnityEngine;

public class Defeated : GhostAIMachineState
{
	public Defeated(GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
		: base(ghostAI, ghostMove, pacman)
	{
		Name = GhostAIState.Defeated;
	}

	public override void Enter()
	{
		// No Pacman original o fantasminha anda mais rápido quando morre.
		GhostMove.CharacterMotor.MoveSpeed = GhostMove.CharacterMotor.MoveSpeed * 2;

		GhostMove.CharacterMotor.CollideWithGates(false);
		GhostMove.SetTargetMoveLocation(Vector3.zero);

		GhostAI.OnGhostRecovered += GhostAI_OnGhostRecovered;
		GhostAI.ChangeGhostAnimation(GhostAnimationState.Defeated);

		GhostAI.GhostCaught();

		base.Enter();
	}

	private void GhostAI_OnGhostRecovered()
	{
		ChangeState(new InHouse(GhostAI, GhostMove, Pacman));
	}

	public override void Exit()
	{
		// Voltamos sua velocidade original.
		GhostMove.CharacterMotor.MoveSpeed = GhostMove.CharacterMotor.MoveSpeed / 2;

		GhostMove.CharacterMotor.CollideWithGates(true);
		GhostAI.ChangeGhostAnimation(GhostAnimationState.Active);
		GhostAI.OnGhostRecovered -= GhostAI_OnGhostRecovered;
		base.Exit();
	}
}
