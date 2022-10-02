using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClockSounds : MonoBehaviour
{
    private bool doTick = true;
    private AudioSource source;
    [SerializeField] private AudioClip tick;
    [SerializeField] private AudioClip tock;

    private void Start() {

        source = GetComponent<AudioSource>();
        WorldClock.OnTick += PlaySound;
    }

    private void PlaySound() {
        
        if (doTick) {
            source.PlayOneShot(tick);
        } else {
            source.PlayOneShot(tock);
        }
        
        doTick = !doTick;
    }
}
