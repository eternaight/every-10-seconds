using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFloor : MonoBehaviour {
    [SerializeField] private Transform forWhom;

    void Update() => transform.position = new Vector3(forWhom.position.x, transform.position.y, transform.position.z);
}
