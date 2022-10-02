using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterSound : MonoBehaviour
{
    private AudioSource source;    
    private CharacterLegs legs;

    private bool running;
    private void SetRunning(bool value) {
        if (running != value) {
            if (value) {
                source.UnPause();
            } else {
                source.Pause();
            }
            running = value;
        }
    }

    private void Start() {
        source = GetComponent<AudioSource>();
        legs = GetComponentInParent<CharacterLegs>();

        source.loop = true;
        source.Play();
        source.Pause();
    }

    private void Update() {
        var runningOnGround = legs.driveX != 0 && legs.Grounded;
        SetRunning(runningOnGround);
    }
}
