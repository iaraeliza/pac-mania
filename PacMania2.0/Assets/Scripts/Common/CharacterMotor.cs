using System;
using UnityEngine;

public enum Direction
{
	None,
	Up,
	Left,
	Down,
	Right
}

public class CharacterMotor : MonoBehaviour
{
	public float MoveSpeed;

	private Rigidbody2D _rigidbody;

	private Vector2 _currentMovementDirection;
	private Vector2 _desiredMovementDirection;

	private Vector2 _boxSize;

	private LayerMask _collisionLayerMask;

	private Vector3 _initialPosition;

	public event Action OnAlignedWithGrid;
	public event Action<Direction> OnDirectionChanged;
	public event Action OnResetPosition;
	public event Action OnEnabled;

	public LayerMask CollistionLayerMask
	{
		get => _collisionLayerMask;
	}

	public Direction CurrentMoveDirection
	{
		get
		{
			// Up
			if (_currentMovementDirection.y > 0)
			{
				return Direction.Up;
			}
			// Left
			if (_currentMovementDirection.x < 0)
			{
				return Direction.Left;
			}
			// Down
			if (_currentMovementDirection.y < 0)
			{
				return Direction.Down;
			}
			// Right
			if (_currentMovementDirection.x > 0)
			{
				return Direction.Right;
			}

			return Direction.None;
		}
	}

	public void SetMoveDirection(Direction newMoveDirection)
	{
		switch (newMoveDirection)
		{
			default:
			case Direction.None:
				break;

			case Direction.Up:
				_desiredMovementDirection = Vector2.up;
				break;

			case Direction.Left:
				_desiredMovementDirection = Vector2.left;
				break;

			case Direction.Down:
				_desiredMovementDirection = Vector2.down;
				break;

			case Direction.Right:
				_desiredMovementDirection = Vector2.right;
				break;
		}
	}

	public void ResetPosition()
	{
		_desiredMovementDirection = Vector2.zero;
		_currentMovementDirection = Vector2.zero;
		transform.position = _initialPosition;
		OnResetPosition?.Invoke();
	}

	public void CollideWithGates(bool shouldCollide)
	{
		if (shouldCollide)
		{
			_collisionLayerMask = LayerMask.GetMask(new string[] { "Level", "Gates" });
		}
		else
		{
			_collisionLayerMask = LayerMask.GetMask(new string[] { "Level" });
		}
	}

	private void Start()
	{
		_desiredMovementDirection = Vector2.zero;
		_currentMovementDirection = Vector2.zero;
		_rigidbody = GetComponent<Rigidbody2D>();
		_boxSize = GetComponent<BoxCollider2D>().size;
		CollideWithGates(true);
		_initialPosition = transform.position;
	}

	private void FixedUpdate()
	{
		float moveDistance = MoveSpeed * Time.fixedDeltaTime;
		var nextMovePosition = _rigidbody.position + _currentMovementDirection * moveDistance;

		// Up
		if (_currentMovementDirection.y > 0)
		{
			var maxY = Mathf.CeilToInt(_rigidbody.position.y);
			if (nextMovePosition.y >= maxY)
			{
				transform.position = new Vector2(_rigidbody.position.x, maxY);
				moveDistance = nextMovePosition.y - maxY;
			}
		}
		// Left
		if (_currentMovementDirection.x < 0)
		{
			var minX = Mathf.FloorToInt(_rigidbody.position.x);
			if (nextMovePosition.x <= minX)
			{
				transform.position = new Vector2(minX, _rigidbody.position.y);
				moveDistance = minX - nextMovePosition.x;
			}
		}
		// Down
		if (_currentMovementDirection.y < 0)
		{
			var minY = Mathf.FloorToInt(_rigidbody.position.y);
			if (nextMovePosition.y <= minY)
			{
				transform.position = new Vector2(_rigidbody.position.x, minY);
				moveDistance = minY - nextMovePosition.y;
			}
		}
		// Right
		if (_currentMovementDirection.x > 0)
		{
			var maxX = Mathf.CeilToInt(_rigidbody.position.x);
			if (nextMovePosition.x >= maxX)
			{
				transform.position = new Vector2(maxX, _rigidbody.position.y);
				moveDistance = nextMovePosition.x - maxX;
			}
		}

		Physics2D.SyncTransforms();

		// Verifica alinhamento
		if ((_rigidbody.position.x == Mathf.CeilToInt(_rigidbody.position.x) &&
			_rigidbody.position.y == Mathf.CeilToInt(_rigidbody.position.y)) ||
			_currentMovementDirection == Vector2.zero)
		{
			OnAlignedWithGrid?.Invoke();

			if (_currentMovementDirection != _desiredMovementDirection)
			{
				if (!Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _desiredMovementDirection, 1f, _collisionLayerMask))
				{
					_currentMovementDirection = _desiredMovementDirection;
					OnDirectionChanged?.Invoke(CurrentMoveDirection);
				}
			}

			if (Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _currentMovementDirection, 1f, _collisionLayerMask))
			{
				_currentMovementDirection = Vector2.zero;
				OnDirectionChanged?.Invoke(CurrentMoveDirection);
			}
		}

		_rigidbody.MovePosition(_rigidbody.position + _currentMovementDirection * moveDistance);
	}

	private void OnEnable()
	{
		OnEnabled?.Invoke();
	}
}
