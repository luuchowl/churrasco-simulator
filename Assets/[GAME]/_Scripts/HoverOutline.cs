using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    public Renderer[] renderers;

    private int outlineWithProperty = Shader.PropertyToID("_OutlineSize");
    private MaterialPropertyBlock propBlock;

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        OnHoverExit();
    }

    public void OnHoverEnter()
    {
        foreach (var rend in renderers)
        {
            rend.GetPropertyBlock(propBlock);
            propBlock.SetFloat(outlineWithProperty, 1);
            rend.SetPropertyBlock(propBlock);
        }
    }

    public void OnHoverExit()
    {
        foreach (var rend in renderers)
        {
            rend.GetPropertyBlock(propBlock);
            propBlock.SetFloat(outlineWithProperty, 0);
            rend.SetPropertyBlock(propBlock);
        }
    }
}
