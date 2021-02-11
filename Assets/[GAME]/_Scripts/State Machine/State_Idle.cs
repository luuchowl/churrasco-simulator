using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : StateBase
{
	public override void OnStateEnter(PlayerStateMachine _player)
	{
		base.OnStateEnter(_player);

		player.onMove += Player_onMove;
		player.onLeftClick += Player_onLeftClick;
		player.onRightClick += Player_onRightClick;
		player.onScroll += Player_onScroll;
	}

	private void Player_onScroll(float dir)
	{
		player.movement.Rotate(dir);
	}

	private void Player_onRightClick(bool down)
	{
		if (down)
		{
			player.movement.MoveToTarget();
		}
		else
		{
			player.movement.ReturnToRestPosition();
		}
	}

	private void Player_onLeftClick(bool down)
	{
		if (down)
		{
			player.interaction.GrabTarget();
			player.ChangeState(player.grabbing);
		}
	}

	private void Player_onMove(Vector2 dir)
	{
		player.movement.Move(new Vector3(dir.x, 0, dir.y));
	}

	public override void OnStateExit()
	{
		base.OnStateExit();
		player.onMove -= Player_onMove;
		player.onLeftClick -= Player_onLeftClick;
		player.onRightClick -= Player_onRightClick;
		player.onScroll -= Player_onScroll;
	}
}
