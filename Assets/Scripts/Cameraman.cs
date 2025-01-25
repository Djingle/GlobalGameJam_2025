using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    private Transform target;
    private void Start()
    {
        target = Player.Instance.transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
