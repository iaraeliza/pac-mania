using System.Collections.Generic;
using UnityEngine;

public class GhostHouse : MonoBehaviour
{
	public float LeaveHouseInterval;

	List<GhostAI> _allGhost;

	private float _leaveHouseTimer;

	private void Awake()
	{
		_allGhost = new List<GhostAI>();
		_leaveHouseTimer = LeaveHouseInterval;
	}

	private void Update()
	{
		if (_allGhost.Count > 0)
		{
			_leaveHouseTimer -= Time.deltaTime;

			if (_leaveHouseTimer <= 0)
			{
				_leaveHouseTimer += LeaveHouseInterval;
				_allGhost[0].LeaveHouse();
				_allGhost.RemoveAt(0);
			}
		}
	}

	// A GhostHouse na verdade tinha um bug. Fantasmas que ainda não tinham saído da área de colisão
	// no momento de um restart não ativavam o TriggerEnter. Além disso o timer não reiniciava entre
	// as rodadas quando rolava um restart.
	public void Reset()
	{
		_leaveHouseTimer = LeaveHouseInterval;

		var boxCollider = GetComponent<BoxCollider2D>();

		// Fazendo a checagem utilizando OverlapBox com o mesmo tamanho da nossa área de trigger, nós
		// garantimos que vamos pegar todos os fantasmas sem precisar se preocupar com o OnTriggerEnter
		var ghostColliders = Physics2D.OverlapBoxAll(
			new Vector2(
				transform.position.x, transform.position.y) + boxCollider.offset,
				boxCollider.size,
				0,
				1 << LayerMask.NameToLayer("Ghosts"));

		foreach (var ghostCollider in ghostColliders)
		{
			AddGhost(ghostCollider);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		AddGhost(other);
	}

	private void AddGhost(Collider2D ghostCollider)
	{
		var ghost = ghostCollider.GetComponent<GhostAI>();

		if (_allGhost.Contains(ghost))
		{
			return;
		}

		ghost.Recover();

		if (_allGhost.Count == 0)
		{
			_leaveHouseTimer = LeaveHouseInterval;
		}

		_allGhost.Add(ghost);
	}
}
