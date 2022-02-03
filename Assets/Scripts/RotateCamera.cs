using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 1;
    void Start()
    {
        
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); 
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
