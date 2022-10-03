using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class FiniteFloor : MonoBehaviour {
    [SerializeField] private Transform forWhom;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2d;

    private float lifetime;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();

        MenuManager.OnExitMenu += Floor;
    }

    private void Update () {
        lifetime -= Time.deltaTime;

        Color newColor = spriteRenderer.color;
        newColor.a = Mathf.Clamp01(lifetime);
        spriteRenderer.color = newColor;
        
        collider2d.enabled = lifetime > 0f;
    }

    private void Floor () {
        transform.position = forWhom.position + Vector3.down * 0.5f;
        lifetime = MenuManager.ClockPeriod * 0.5f;
    }
}
