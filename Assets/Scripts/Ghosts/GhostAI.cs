using System;
using UnityEngine;

public enum GhostAnimationState
{
	Active,
	Vulnerable,
	VulnerabilityEnding,
	Defeated
}

[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour, IMovableCharacter
{
	// O Blinky começa fora da casinha, ele precisa começar direto no estado de Chasing.
	// Criamos esta variável 'StartInHouse' para poder diferenciar o Blinky dos outros.
	public bool StartsInHouse = true;

	private GhostMove _ghostMove;
	private Transform _pacman;

	public event Action<GhostAnimationState> OnGhostStateChanged;
	public event Action<GhostAI> OnGhostCaught;
	public event Action<float> OnSetVulnerable;
	public event Action OnGhostRecovered;
	public event Action OnLeaveHouse;
	public event Action<Collider2D> OnCollideWithPacman;

	private GhostAIMachineState _currentMachineState;

	public void ResetPosition()
	{
		_ghostMove.CharacterMotor.ResetPosition();

		// Ao reiniciar o jogo nos precisamos forçar a mudança do estado atual para garantir
		// que os métodos de Exit dos estados estão sendo chamados pois é lá que fazemos os
		// unsubscribes dos eventos.
		if (StartsInHouse)
		{
			_currentMachineState.ForceChangeState(new InHouse(this, _ghostMove, _pacman));
		}
		else
		{
			_currentMachineState.ForceChangeState(new Chasing(this, _ghostMove, _pacman));
		}
	}

	public void StartMoving()
	{
		_ghostMove.CharacterMotor.enabled = true;
	}

	public void StopMoving()
	{
		_ghostMove.CharacterMotor.enabled = false;
	}
	public void GhostCaught()
	{
		OnGhostCaught?.Invoke(this);
	}

	public void ChangeGhostAnimation(GhostAnimationState newGhostAnimation)
	{
		OnGhostStateChanged?.Invoke(newGhostAnimation);
	}

	public void SetVulnerable(float duration)
	{
		OnSetVulnerable?.Invoke(duration);
	}

	public void Recover()
	{
		OnGhostRecovered?.Invoke();
	}

	public void LeaveHouse()
	{
		OnLeaveHouse?.Invoke();
	}

	private void Start()
	{
		_ghostMove = GetComponent<GhostMove>();
		_pacman = GameObject.FindWithTag("Player").transform;

		if (StartsInHouse)
		{
			_currentMachineState = new InHouse(this, _ghostMove, _pacman);
		}
		else
		{
			_currentMachineState = new Chasing(this, _ghostMove, _pacman);
		}
	}

	private void Update()
	{
		_currentMachineState = _currentMachineState.Handle();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnCollideWithPacman?.Invoke(other);
		}
	}
}