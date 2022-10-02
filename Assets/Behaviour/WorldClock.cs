using UnityEngine;

public class WorldClock : MonoBehaviour
{
    public static event System.Action OnTick;

    private void Start() {
        MenuManager.OnExitMenu += Restart;
    }

    private void Restart() {
        CancelInvoke(nameof(Tick));
        InvokeRepeating(nameof(Tick), 0f, MenuManager.ClockPeriod);
    }

    private void Tick() {
        OnTick?.Invoke();
    }
}
