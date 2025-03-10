using UnityEngine;

public class CameraFollow : MonoBehaviour

{
    public Transform target;


    void FixedUpdate()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 5, target.transform.position.z);
    }
}