using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private TileGenerator tileGenerator;

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
            Vector3 boundedTarget = target.Value;
            Vector2 rightmost = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, 0.0f));
            Vector2 leftmost = Camera.main.ScreenToWorldPoint(new Vector2(0.0f, 0.0f));
            Vector2 tileSize = tileGenerator.GetTileSize();
            float xMax = tileGenerator.GetExtent().x + tileSize.x * 2;
            float xMin = -tileSize.x * 2;
            if((rightmost.x > xMax && boundedTarget.x > transform.position.x)
            || (leftmost.x < xMin && boundedTarget.x < transform.position.x))
            {
                boundedTarget.x = transform.position.x;
            }
            transform.position = Vector3.Lerp(transform.position, boundedTarget + offset, smoothing * Time.deltaTime);
        }
    }
}
