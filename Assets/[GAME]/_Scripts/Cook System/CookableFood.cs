using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookableFood : Cookable
{
    public ParticleSystem smoke;

	private void OnEnable()
	{
		smoke.Stop();
	}

	public override void OnStartCooking()
	{
		smoke.Play();
	}

	public override void OnStopCooking()
	{
		smoke.Stop();
	}

	public override void Cook()
	{

	}
}
