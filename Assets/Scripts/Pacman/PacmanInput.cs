using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PacmanInput : MonoBehaviour//, IMovableCharacter
{
	private CharacterMotor _motor;

	public void ResetPosition()
	{
		_motor.ResetPosition();
	}

	public void StartMoving()
	{
		_motor.enabled = true;
	}

	public void StopMoving()
	{
		_motor.enabled = false;
	}

	private void Start()
	{
		_motor = GetComponent<CharacterMotor>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_motor.SetMoveDirection(Direction.Up);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_motor.SetMoveDirection(Direction.Left);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_motor.SetMoveDirection(Direction.Down);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			_motor.SetMoveDirection(Direction.Right);
		}
	}
}
