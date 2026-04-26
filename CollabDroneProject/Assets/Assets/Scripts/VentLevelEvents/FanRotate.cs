using UnityEngine;

public class ConstantSpin : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 30f, 0); // degrees per second

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}