using UnityEngine;

public class Ring : MonoBehaviour
{
    public int ringIndex;
    public RaceManager raceManager;

    private Renderer[] rend;
    private Collider col;

    void Awake()
    {
        rend = GetComponentsInChildren<Renderer>();
        col = GetComponent<Collider>();

        Debug.Log(gameObject.name + " rends count: " + rend.Length);
        Debug.Log(gameObject.name + " collider: " + col);

        SetVisible(false);
    }

    public void SetVisible(bool state)
    {
        Debug.Log(gameObject.name + " -  " + state);
        foreach (var r in rend)
        {
            r.enabled = state;
        }
        col.enabled = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!col.enabled) return;

        if (other.CompareTag("Player"))
        {
            raceManager.RingPassed(ringIndex);
        }
    }
}
