using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class PlayerStateMachine : MonoBehaviour
{
	public Animator anim;
	public Player_Movement movement;
	public Interaction_Controller interaction;

	public event Action<Vector2> onMove;
	public event Action<bool> onLeftClick;
	public event Action<bool> onRightClick;
	public event Action<float> onScroll;

	private StateBase currentState;

#if UNITY_EDITOR
	[Header("Debug")]
	[ReadOnly, SerializeField] private string currentStateName;
#endif

	#region States
	public StateBase idle = new State_Idle();
	public State_Grabbing grabbing = new State_Grabbing();
	#endregion

	private void Start()
	{
		ChangeState(idle);
	}

	private void OnEnable()
	{
		//Hides cursor and locks it on screen
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		//Returns cursor back to normal
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		currentState.UpdateState();

#if UNITY_EDITOR
		if (Keyboard.current[Key.O].wasPressedThisFrame)
		{
			Debug.Break();
		}
#endif
	}

	public void ChangeState(StateBase newState)
	{
		currentState?.OnStateExit();
		currentState = newState;
		currentState.OnStateEnter(this);

#if UNITY_EDITOR
		currentStateName = $"{currentState.GetType()}";
#endif
	}

	private void OnMove(InputValue value)
	{
		onMove?.Invoke(value.Get<Vector2>());
	}

	private void OnLeftClick(InputValue value)
	{
		if (value.Get<float>() > 0)
		{
			onLeftClick?.Invoke(true);
		}
		else
		{
			onLeftClick?.Invoke(false);
		}
	}

	private void OnRightClick(InputValue value)
	{
		if (value.Get<float>() > 0)
		{
			onRightClick?.Invoke(true);
		}
		else
		{
			onRightClick?.Invoke(false);
		}
	}

	private void OnScroll(InputValue value)
	{
		onScroll?.Invoke(value.Get<float>());
	}
}
