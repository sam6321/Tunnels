using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform transformTarget;

    [SerializeField]
    private Vector3? positionTarget;

    [SerializeField]
    private float smoothing = 5.0f;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, -10);

    public Transform TransformTarget { get => transformTarget; set => transformTarget = value; }
    public Vector3? PositionTarget { get => positionTarget; set => positionTarget = value; }

    public Vector3? GetCurrentTarget()
    {
        if (transformTarget != null)
        {
            return TransformTarget.position;
        }
        else
        {
            return PositionTarget;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3? target = GetCurrentTarget();
        if(target.HasValue)
        {
            transform.position = Vector3.Lerp(transform.position, target.Value + offset, smoothing * Time.deltaTime);
        }
    }
}
