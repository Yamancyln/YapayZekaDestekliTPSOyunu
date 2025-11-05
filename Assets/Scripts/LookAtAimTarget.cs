using UnityEngine;

public class LookAtAimTarget : MonoBehaviour
{
    public Transform aimTarget; // inspector'dan atanacak
    public float aimDistance = 500f; // ray uzaklığı
    public Transform camera;
    public bool Aiming { get; private set; }

    void LateUpdate()
    {
        Aiming = Input.GetButton("Fire2");

        if (Aiming)
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out RaycastHit hit, aimDistance))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = camera.transform.position + camera.transform.forward * aimDistance;
            }
            aimTarget.position = targetPoint;
        }
    }
}
