using UnityEngine;

public class GodrayFlicker : MonoBehaviour
{
    public Material mat;
    public float speed = 1f;
    public float amount = 0.3f;

    float baseIntensity;

    void Start()
    {
        if (mat != null)
            baseIntensity = mat.GetFloat("_Intensity");
    }

    void Update()
    {
        if (mat == null) return;

        float flicker = Mathf.Sin(Time.time * speed) * amount;
        mat.SetFloat("_Intensity", baseIntensity + flicker);
    }
}
