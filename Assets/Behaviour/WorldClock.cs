using UnityEngine;

public class WorldClock : MonoBehaviour
{
    public static event System.Action OnPreTick, OnTick;

    private void Start() {
        MenuManager.OnExitMenu += Restart;
    }

    private void Restart() {
        CancelInvoke(nameof(PreTick));
        InvokeRepeating(nameof(PreTick), 0f, MenuManager.ClockPeriod);

        CancelInvoke(nameof(Tick));
        InvokeRepeating(nameof(Tick), MenuManager.ClockPeriod * 0.5f, MenuManager.ClockPeriod);
    }

    private void PreTick() {
        OnPreTick?.Invoke();
    }

    private void Tick() {
        OnTick?.Invoke();
    }
}
