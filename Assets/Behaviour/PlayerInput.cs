using UnityEngine;

[RequireComponent(typeof(CharacterLegs))]
public class PlayerInput : MonoBehaviour
{
    private CharacterLegs legs;

    private void Start() {
        legs = GetComponent<CharacterLegs>();
    }

    private void Update() {
        legs.driveX = Input.GetAxisRaw("Horizontal");
        var vert = Input.GetAxisRaw("Vertical");
        legs.SetJump(vert > 0);
    }
}
