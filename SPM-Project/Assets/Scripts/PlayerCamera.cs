using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Vector3 cameraOffset;
    float rotationX, rotationY;
    float camRadius = 0.25f;
    [SerializeField] float lookSensitivity = 1.5f;
    [SerializeField] float minCameraDistance = 2f;
    [SerializeField] bool cameraIsFirstPerson;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] Transform player;

    public Quaternion GetRotation() { return transform.rotation; }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        /*if(!cameraIsFirstPerson)
            cameraOffset = new Vector3(0, 1, -7);*/
    }

    void Update() {
        cameraOffset = cameraIsFirstPerson ? Vector3.zero : new Vector3(0, 1, -7);
        RotateCamera();
        transform.position = player.transform.position + GetAdjustedCameraPosition(transform.rotation * cameraOffset);
    }

    Vector3 GetAdjustedCameraPosition(Vector3 relationVector) {
        if (Physics.SphereCast(player.transform.position, camRadius, relationVector.normalized, out RaycastHit hit, relationVector.magnitude + camRadius, collisionLayer)) {
            if (hit.distance > minCameraDistance)
                return relationVector.normalized * (hit.distance - camRadius);
            else return Vector3.zero;
        }
        else return relationVector;
    }

    void RotateCamera() {
        rotationY += lookSensitivity * Input.GetAxis("Mouse X");
        rotationX -= lookSensitivity * Input.GetAxis("Mouse Y");
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}
