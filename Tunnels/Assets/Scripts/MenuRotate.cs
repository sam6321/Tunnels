using UnityEngine;

public class MenuRotate : MonoBehaviour
{
    [SerializeField]
    private Transform[] objects;

    [SerializeField]
    private float offsetDistanceMin;

    [SerializeField]
    private float offsetDistanceMax;

    [SerializeField]
    private float offsetPulseSpeed;

    [SerializeField]
    private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        float rotationPortion = 360.0f / objects.Length;

        for(int i = 0; i < objects.Length; i++)
        {
            float distance = Mathf.Lerp(offsetDistanceMin, offsetDistanceMax, Factor(Time.time * offsetPulseSpeed));
            Vector3 offset = Quaternion.AngleAxis(rotationPortion * i + Time.time * rotateSpeed, Vector3.forward) * new Vector3(distance, 0, 0);
            objects[i].position = transform.position + offset;
        }
    }

    float Factor(float x)
    {
        return Mathf.Cos(x) * 0.5f + 0.5f;
    }
}
