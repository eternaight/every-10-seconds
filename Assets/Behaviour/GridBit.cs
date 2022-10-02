using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBit : MonoBehaviour {
    [SerializeField] private Collider2D blockCollider;
    [SerializeField] private SpriteRenderer blockSR;
    [SerializeField] private Gradient blockGradient;

    [SerializeField] private SpriteRenderer glyphSR;
    [SerializeField] private Color glyphExtensionColor = Color.green;
    [SerializeField] private Color glyphRetractionColor = Color.red;
    [SerializeField] private Color glyphNeutralColor = Color.clear;

    [SerializeField] private float smoothness = 0.05f;
    private float MoveNotice => WorldClock.PeriodSeconds * 0.5f;

    private float state;
    private float queuedState;
    private float noticeTimer;

    private void Update () {
        noticeTimer -= Time.deltaTime;

        if (noticeTimer > 0f) {
            glyphSR.color = 
                Color.Lerp(glyphNeutralColor, 
                queuedState > state ? glyphExtensionColor : glyphRetractionColor, 
                1f - noticeTimer / MoveNotice);
        }
        else {
            if (queuedState != state) {
                state = queuedState;
                blockCollider.enabled = state == 1f;
            }

            glyphSR.color = Color.Lerp(glyphSR.color, glyphNeutralColor, smoothness);
            blockSR.color = Color.Lerp(blockSR.color, blockGradient.Evaluate(state), smoothness);
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, transform.position.y, -state * 2f), smoothness);
        }
    }

    public void QueueState (float queuedState) {
        if (queuedState == state) return;
        this.queuedState = queuedState;
        noticeTimer = MoveNotice;
    }
}
