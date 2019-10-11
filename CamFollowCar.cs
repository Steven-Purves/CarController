using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowCar : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float speed = 10;
    public float lookspeed = 10;

    public void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookspeed * Time.deltaTime);
    }
    public void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }

}
