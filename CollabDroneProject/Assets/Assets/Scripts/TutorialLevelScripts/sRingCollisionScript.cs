using UnityEngine;

public class sRingCollisionScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool finishedCourse = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit");

        GameObject triggerObject = gameObject;

        if (triggerObject.tag == "Finish" && finishedCourse == false)
        {
            //Temporary Code for Tutorial Debug
            finishedCourse = true;
            Debug.Log("Test");
        }
    }
}
