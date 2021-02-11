using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum CookState
{
    Raw,
    WellDone,
    Burnt
}

public class CookableFood : Cookable
{
    [Range(.1f, 10)] public float burnTime = 1;
    [MinMaxSlider(.1f, 10)] public Vector2 wellDoneRange;
    public Renderer rend;
    public ParticleSystem smoke;
    [Header("State textures")]
    public Texture2D rawTexture;
    public Texture2D wellDoneTexture;
    public Texture2D burntTexture;
    public CookState state { get; private set; }

    private float currentCookingLevel;

    private void OnEnable()
    {
        smoke.Stop();
        currentCookingLevel = 0;
        ChangeState(CookState.Raw);
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
        currentCookingLevel += Time.deltaTime;

        if (currentCookingLevel < wellDoneRange.x)
        {
            ChangeState(CookState.Raw);
        }
        else if (currentCookingLevel > wellDoneRange.y)
        {
            ChangeState(CookState.Burnt);
        }
        else
        {
            ChangeState(CookState.WellDone);
        }
    }

    private void ChangeState(CookState targetState)
    {
        if (state == targetState)
            return;

        state = targetState;

        switch (state)
        {
            case CookState.Raw:
                rend.material.mainTexture = rawTexture;
                break;
            case CookState.WellDone:
                rend.material.mainTexture = wellDoneTexture;
                break;
            case CookState.Burnt:
                rend.material.mainTexture = burntTexture;
                break;
            default:
                break;
        }
    }
}
