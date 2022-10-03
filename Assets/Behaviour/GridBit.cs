using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBit : MonoBehaviour {
    [SerializeField] private Collider2D blockCollider;
    [SerializeField] private SpriteRenderer blockSR;
    [SerializeField] private Gradient blockGradient;

    [SerializeField] private SpriteRenderer glyphSR;
    [SerializeField] private Color glyphActiveColor = Color.white;
    [SerializeField] private Color glyphInactiveColor = Color.clear;

    [SerializeField] private float smoothness = 0.05f;

    private float state = 0f;
    private float queuedState;

    private bool IsGlyphworthy => state != queuedState && ( state == 0f || state == 1f );

    private void Start () {
        glyphSR.color = glyphInactiveColor;
        blockSR.color = blockGradient.Evaluate(0f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
        blockCollider.enabled = false;

        WorldClock.OnTick += DequeueState;
    }

    private void Update () {
        glyphSR.color = Color.Lerp(
            glyphSR.color, 
            IsGlyphworthy ? glyphActiveColor : glyphInactiveColor, 
            smoothness);

        blockSR.color = Color.Lerp(blockSR.color, blockGradient.Evaluate(state), smoothness);
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(transform.position.x, transform.position.y, 1f - state), smoothness);
    }

    public void EnqueueState (float state) {
        queuedState = state;
    }

    public void DequeueState() {
        state = queuedState;
        blockCollider.enabled = state == 1f;
    }
}
