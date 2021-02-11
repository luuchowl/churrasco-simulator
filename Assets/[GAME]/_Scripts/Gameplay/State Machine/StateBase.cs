using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
	public PlayerStateMachine player;


	public virtual void OnStateEnter(PlayerStateMachine _player)
	{
		player = _player;
	}

	public virtual void UpdateState()
	{

	}

	public virtual void OnStateExit()
	{

	}
}
