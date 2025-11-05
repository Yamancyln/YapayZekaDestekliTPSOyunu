using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform cam;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
