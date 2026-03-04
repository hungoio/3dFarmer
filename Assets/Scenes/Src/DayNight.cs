using UnityEngine;

public class DayNight : MonoBehaviour
{
    public Light sun;
    public float dayDuration = 60f;

    // Update is called once per frame
    void Update()
    {
        float rotationSpeed = 360f / dayDuration;
        sun.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
