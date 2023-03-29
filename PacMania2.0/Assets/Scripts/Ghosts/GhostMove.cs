using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class GhostMove : MonoBehaviour
{
	private CharacterMotor _motor;
	private Vector2 _boxSize;
	private Vector2 _targetMoveLocation;
	private bool _allowReverseDirection;

	public event Action OnUpdateMoveTarget;

	public CharacterMotor CharacterMotor { get => _motor; }

	public void SetTargetMoveLocation(Vector2 targetMoveLocation)
	{
		_targetMoveLocation = targetMoveLocation;
	}

	public void AllowReverseDirection()
	{
		_allowReverseDirection = true;
	}

	private void Start()
	{
		_motor = GetComponent<CharacterMotor>();
		_motor.OnAlignedWithGrid += CharacterMotor_OnAlignedWithGrid;

		_boxSize = GetComponent<BoxCollider2D>().size;

		CharacterMotor_OnAlignedWithGrid();

		_allowReverseDirection = false;
	}

	private void CharacterMotor_OnAlignedWithGrid()
	{
		OnUpdateMoveTarget?.Invoke();
		ChangeDirection();
	}

	private void ChangeDirection()
	{
		var closestDistance = float.MaxValue;
		Direction finalDirection = Direction.None;

		UpdateFinalDirection(Direction.Up, Vector3.up, ref closestDistance, ref finalDirection);
		UpdateFinalDirection(Direction.Left, Vector3.left, ref closestDistance, ref finalDirection);
		UpdateFinalDirection(Direction.Down, Vector3.down, ref closestDistance, ref finalDirection);
		UpdateFinalDirection(Direction.Right, Vector3.right, ref closestDistance, ref finalDirection);

		_motor.SetMoveDirection(finalDirection);

		_allowReverseDirection = false;
	}

	private void UpdateFinalDirection(Direction direction, Vector3 offset, ref float closestDistance, ref Direction finalDirection)
	{
		if (CheckIfDirectionMovable(direction))
		{
			var dist = Vector2.Distance(transform.position + offset, _targetMoveLocation);
			if (dist < closestDistance)
			{
				closestDistance = dist;
				finalDirection = direction;
			}
		}
	}

	private bool CheckIfDirectionMovable(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.up, 1f, _motor.CollistionLayerMask)
					&& (_motor.CurrentMoveDirection != Direction.Down || _allowReverseDirection);

			case Direction.Left:
				return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.left, 1f, _motor.CollistionLayerMask)
					&& (_motor.CurrentMoveDirection != Direction.Right || _allowReverseDirection);

			case Direction.Down:
				return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, 1f, _motor.CollistionLayerMask)
					&& (_motor.CurrentMoveDirection != Direction.Up || _allowReverseDirection);

			case Direction.Right:
				return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.right, 1f, _motor.CollistionLayerMask)
					&& (_motor.CurrentMoveDirection != Direction.Left || _allowReverseDirection);
		}

		return false;
	}
}
