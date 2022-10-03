using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform toFollow;
    [SerializeField] private float smoothness = 0.05f;
    [SerializeField] private float cameraDistance = 10f;

    void LateUpdate() => transform.position = Vector3.Lerp(
        transform.position, 
        toFollow.position + Vector3.back * cameraDistance, 
        smoothness);
}
