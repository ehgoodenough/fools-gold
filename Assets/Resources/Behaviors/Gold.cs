using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Coords = Map.Coords;

public class Gold : MonoBehaviour {
    private const float Z_INDEX = 0.15f;

    void Update() {
        // Do the z-indexing off their y position.
        Vector3 position = transform.position;
        position.z = position.y + Z_INDEX;
        transform.position = position;
    }
}
