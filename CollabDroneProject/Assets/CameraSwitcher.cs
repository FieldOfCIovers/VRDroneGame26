using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera fppCamera;
    public Camera tppCamera;

    private bool isFPP = true;

    void Start()
    {
        SetCamera(true);
    }

    void Update()
    {
        // Press Y on Xbox controller
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            isFPP = !isFPP;
            SetCamera(isFPP);
        }
    }

    void SetCamera(bool fpp)
    {
        fppCamera.enabled = fpp;
        tppCamera.enabled = !fpp;
    }
}