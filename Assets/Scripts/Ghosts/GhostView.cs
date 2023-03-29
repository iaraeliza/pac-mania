using UnityEngine;

public enum GhostType
{
	Blinky,
	Pinky,
	Inky,
	Clyde
}

public class GhostView : BaseView
{
	public CharacterMotor CharacterMotor;

	public GhostAI GhostAI;

	public Animator Animator;

	public GhostType GhostType;

	private void Start()
	{
		Animator.SetInteger("GhostType", (int)GhostType);
		CharacterMotor.OnDirectionChanged += CharacterMotor_OnDirectionChanged;
		GhostAI.OnGhostStateChanged += GhostAI_OnGhostStateChanged;
	}

	private void GhostAI_OnGhostStateChanged(GhostAnimationState ghostState)
	{
		Animator.SetInteger("State", (int)ghostState);
	}

	private void CharacterMotor_OnDirectionChanged(Direction direction)
	{
		Animator.SetInteger("Direction", (int)direction - 1);
	}
}