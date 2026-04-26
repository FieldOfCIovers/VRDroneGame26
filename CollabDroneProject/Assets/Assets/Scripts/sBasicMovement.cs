using UnityEngine;

public class sBasicMovement : MonoBehaviour
{

    private float moveSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float yAxisMovement = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            yAxisMovement = 1f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            yAxisMovement = -1f;
        }

        Vector3 move = new Vector3(horizontal, yAxisMovement, vertical);
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
