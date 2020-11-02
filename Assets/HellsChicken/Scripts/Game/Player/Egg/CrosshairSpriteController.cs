using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairSpriteController : MonoBehaviour
{

    [SerializeField] private Sprite enabledCrosshair;
    [SerializeField] private Sprite blockedCrosshair;
    
    private SpriteRenderer _spriteRenderer;
    
    //CrossAir can be in 3 states: 
    // enabled
    // disabled
    // blocked

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetCrosshairToIdle();
    }
    
    public void SetCrosshairToAiming()
    {
        _spriteRenderer.sprite = enabledCrosshair;
        var temp = _spriteRenderer.color;
        temp.a = 1f;
        _spriteRenderer.color = temp;
    }

    public void SetCrosshairToIdle()
    {
        _spriteRenderer.sprite = enabledCrosshair;
        var temp = _spriteRenderer.color;
        temp.a = 0.3f;
        _spriteRenderer.color = temp;
    }
    
    public void SetCrosshairToWaiting()
    {
        _spriteRenderer.sprite = blockedCrosshair;
        var temp = _spriteRenderer.color;
        temp.a = 1f;
        _spriteRenderer.color = temp;
    }
}
