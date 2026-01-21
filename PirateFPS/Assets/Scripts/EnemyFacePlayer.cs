using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!cam) return;

        Vector3 lookDirection = transform.position - cam.transform.position;
        lookDirection.y = 0f; // keep sprite upright

        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}

