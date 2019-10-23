using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Controller : MonoBehaviour {
	public Camera mainCamera;
	public Player_Controller player;
	public MainMenu_Controller menu;
	public Orders_Manager orders;
	public Speech_Manager speeches;
	public Grill_Manager grill;
	public ObjectPool[] pools;

	private void Awake() {
		Game_Manager.Instance.levelController = this;

		orders = FindObjectOfType<Orders_Manager>();
		speeches = FindObjectOfType<Speech_Manager>();
		grill = FindObjectOfType<Grill_Manager>();

		pools = FindObjectsOfType<ObjectPool>();
	}
}
