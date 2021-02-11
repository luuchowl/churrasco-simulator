using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Grabbing : StateBase
{
	private Stick stick;

	//Animation Parameters
	private int ap_isHolding = Animator.StringToHash("IsHolding");

	public override void OnStateEnter(PlayerStateMachine _player)
	{
		base.OnStateEnter(_player);

		player.anim.SetBool(ap_isHolding, true);
		player.onMove += Player_onMove;
		player.onLeftClick += Player_onLeftClick;
		player.onRightClick += Player_onRightClick;
		player.onScroll += Player_onScroll;

		Grabbable[] itens = player.interaction.GetGrabbedItens();

		if(itens.Length > 0)
		{
			stick = itens[0].GetComponent<Stick>();
		}
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
			if (stick != null) stick.EnableGrabPoint(true);
		}
		else
		{
			player.movement.ReturnToRestPosition();
			if (stick != null) stick.EnableGrabPoint(false);
		}
	}

	private void Player_onMove(Vector2 dir)
	{
		player.movement.Move(new Vector3(dir.x, 0, dir.y));
	}

	private void Player_onLeftClick(bool down)
	{
		if (!down)
		{
			player.interaction.ReleaseHeldObject();
			player.ChangeState(player.idle);
		}
	}

	public override void OnStateExit()
	{
		base.OnStateExit();
		player.onMove -= Player_onMove;
		player.onLeftClick -= Player_onLeftClick;
		player.onRightClick -= Player_onRightClick;
		player.anim.SetBool(ap_isHolding, false);
		player.onScroll -= Player_onScroll;
	}
}
