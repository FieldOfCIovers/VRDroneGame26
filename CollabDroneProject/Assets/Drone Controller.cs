using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float liftSpeed = 5f;
    public float maxSpeed = 10f;

    [Header("Rotation")]
    public float rotationSpeed = 80f;
    public float pitchSpeed = 60f;

    [Header("Tilt")]
    public float tiltAmount = 15f;
    public float tiltSpeed = 5f;

    [Header("Camera Shake")]
    public float shakeSpeed = 20f;
    public float shakeMultiplier = 0.02f;

    public Transform cameraTransform;

    private Rigidbody rb;
    private Drone input;

    private Vector2 move;
    private Vector2 rotate;
    private float lift;

    private float pitch = 0f;

    void Awake()
    {
        input = new Drone();
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        rb.linearDamping = 2f;
        rb.angularDamping = 3f;
    }

    void Update()
    {
        //  INPUT
        move = input.DroneControls.Move.ReadValue<Vector2>();
        rotate = input.DroneControls.Rotate.ReadValue<Vector2>();
        lift = input.DroneControls.Lift.ReadValue<float>();

        //  YAW (make it responsive)
        transform.Rotate(Vector3.up * rotate.x * rotationSpeed * Time.deltaTime);

        //  PITCH (camera only)
        pitch -= rotate.y * pitchSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        //  TILT (visual only)
        float tiltX = -move.y * tiltAmount;
        float tiltZ = -move.x * tiltAmount;

        // Apply tilt WITHOUT affecting yaw
        Quaternion tiltRotation = Quaternion.Euler(tiltX, 0f, tiltZ);

        // Combine yaw + tilt
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * tiltRotation,
            Time.deltaTime * tiltSpeed
        );

        //  CAMERA SHAKE (based on speed)
        float intensity = rb.linearVelocity.magnitude * shakeMultiplier;

        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * intensity;
        float shakeY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * intensity;

        cameraTransform.localPosition = new Vector3(shakeX, shakeY, 0);
    }

    void FixedUpdate()
    {
        // MOVE IN CAMERA DIRECTION
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        Vector3 direction = (forward * move.y + right * move.x).normalized;

        if (move.magnitude > 0.1f)
        {
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime * 100f);
        }
        else
        {
            // Stop horizontal drift smoothly
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, rb.linearVelocity.y, 0), 0.2f);
        }

        // TRIGGER LIFT (UP + DOWN)
        if (Mathf.Abs(lift) > 0.1f)
        {
            rb.AddForce(Vector3.up * lift * liftSpeed);
        }
        else
        {
            // Stop vertical drift
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z), 0.2f);
        }

        // LIMIT MAX SPEED
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }
}