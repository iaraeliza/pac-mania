using UnityEngine;

public class Vulnerable : GhostAIMachineState
{
	private float _vulnerabilityTimer;
	private bool _vulnerabilityEnding = false;

	public Vulnerable(float timer, GhostAI ghostAI, GhostMove ghostMove, Transform pacman)
		: base(ghostAI, ghostMove, pacman)
	{
		Name = GhostAIState.Vulnerable;
		_vulnerabilityTimer = timer;
	}

	public override void Enter()
	{
		GhostMove.AllowReverseDirection();
		GhostMove.OnUpdateMoveTarget += GhostMove_OnUpdateMoveTarget;
		GhostAI.OnCollideWithPacman += GhostAI_OnCollideWithPacman;
		GhostAI.OnSetVulnerable += GhostAI_OnSetVulnerable;
		GhostAI.ChangeGhostAnimation(GhostAnimationState.Vulnerable);
		base.Enter();
	}

	private void GhostMove_OnUpdateMoveTarget()
	{
		GhostMove.SetTargetMoveLocation((GhostAI.transform.position - Pacman.position) * 2);
	}

	private void GhostAI_OnSetVulnerable(float timer)
	{
		GhostAI.ChangeGhostAnimation(GhostAnimationState.Vulnerable);
		_vulnerabilityTimer = timer;
	}

	private void GhostAI_OnCollideWithPacman(Collider2D obj)
	{
		ChangeState(new Defeated(GhostAI, GhostMove, Pacman));
	}

	public override void Update()
	{
		// Foi preciso fazer esta checagem para que o tempo não corresse enquanto o pacman está
		// fazendo a animação de morte.
		if (GhostMove.CharacterMotor.enabled == false)
		{
			return;
		}

		_vulnerabilityTimer -= Time.deltaTime;

		// Poderia ter sido jogado em outro estado, mas nem sempre é necessário fazer isso,
		// como o comportamento é o mesmo e praticamente só a animação que muda eu decidi que
		// seria melhor deixar tudo neste estado mesmo.
		if (_vulnerabilityTimer < VulnerabilityEndingTime &&
			!_vulnerabilityEnding)
		{
			_vulnerabilityEnding = true;
			GhostAI.ChangeGhostAnimation(GhostAnimationState.VulnerabilityEnding);
		}

		if (_vulnerabilityTimer <= 0)
		{
			ChangeState(new Chasing(GhostAI, GhostMove, Pacman));
			return;
		}

		base.Update();
	}

	public override void Exit()
	{
		GhostMove.OnUpdateMoveTarget -= GhostMove_OnUpdateMoveTarget;
		GhostAI.OnCollideWithPacman -= GhostAI_OnCollideWithPacman;
		GhostAI.OnSetVulnerable -= GhostAI_OnSetVulnerable;
		GhostAI.ChangeGhostAnimation(GhostAnimationState.Active);
		base.Exit();
	}
}
