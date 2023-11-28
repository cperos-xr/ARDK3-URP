using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // Rotation speed in degrees per second

    // Booleans to control rotation around each axis
    public bool rotateX = true;
    public bool rotateY = false;
    public bool rotateZ = false;

    void Update()
    {
        // Calculate rotation for each axis
        float xRotation = rotateX ? rotationSpeed * Time.deltaTime : 0;
        float yRotation = rotateY ? rotationSpeed * Time.deltaTime : 0;
        float zRotation = rotateZ ? rotationSpeed * Time.deltaTime : 0;

        // Rotate the object around the specified axes at the specified speed
        transform.Rotate(xRotation, yRotation, zRotation);
    }
}
