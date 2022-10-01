using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBit : MonoBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Gradient gradient;

    public void UpdateState (float state) {
        spriteRenderer.color = gradient.Evaluate(state);
    }
}
