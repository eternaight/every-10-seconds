using UnityEngine;

public class WorldClock : MonoBehaviour
{
    public static event System.Action OnTick;
    private static WorldClock singletonInstance;

    [SerializeField] private float periodSeconds = 10;

    private void Start() {

        if (singletonInstance != null) {
            Destroy(this);
            return;
        }

        singletonInstance = this;
        InvokeRepeating(nameof(Tick), 0f, periodSeconds);
    }

    private void Tick() {
        OnTick?.Invoke();        
    }
}
