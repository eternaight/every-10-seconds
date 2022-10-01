using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterModel : MonoBehaviour
{
    [SerializeField] private Sprite[] library;
    [SerializeField] private float frameTime = 0.1f;

    private CharacterLegs legsParent;
    private SpriteRenderer sr;

    private void Start() {
        legsParent = GetComponentInParent<CharacterLegs>();
        sr = GetComponent<SpriteRenderer>();

        if (legsParent == null) {
            Debug.LogError($"Legs not detected on parent object! Disabling {nameof(CharacterModel)} component.");
            enabled = false;
        }
    }

    private void Update() {
        if (legsParent.driveX != 0) {
            AnimateLibrary();
            sr.flipX = legsParent.driveX > 0;
        } else {
            AnimateFrame(1);
        }
    }

    private void AnimateLibrary() {
        var i = Mathf.FloorToInt(Time.time / frameTime) % library.Length;
        sr.sprite = library[i];
    }

    private void AnimateFrame(int frame) {
        sr.sprite = library[frame];
    }
}
