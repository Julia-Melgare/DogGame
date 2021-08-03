using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour

{
    public float speed;
    private GameObject bone;
    private Transform head;
    private LineOfSight los;
    private bool lookingForBone;

    void Start()
    {
        bone = GameObject.Find("Bone");
        los = GetComponent<LineOfSight>();
        head = transform.Find("Head");
        lookingForBone = false;
    }

    void Update()
    {
        if (bone == null)
        {
            FindBone();
            head.rotation = transform.rotation;
        }
        else
        {
            Vector3 direction = bone.transform.position - transform.position;
            Debug.DrawLine(transform.position, bone.transform.position);
            Vector3 newPos = (direction.normalized * speed) * Time.deltaTime;
            transform.position += new Vector3(newPos.x, 0, newPos.z);
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            head.rotation = Quaternion.LookRotation(direction);
            FindBone();
        }    
    }

    void FindBone()
    {
        GameObject[] buffer = new GameObject[1];
        if(los.Filter(buffer, "Bone") > 0)
        {
            bone = buffer[0];
        }
        else
        {
            bone = null;
        }
    }

    //WIP
    IEnumerator LookForBone(float duration)
    {
        float time = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(new Vector3(0, -45, 0)) * startRotation;
        while (time < duration)
        {
            //Debug.Log("Time: " + time);
            time += Time.deltaTime;
            float t = time / duration;
           //Debug.Log("T: " + t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
        }
        Debug.Log("Finished looking once");
        /*yield return new WaitForSeconds(0.2f);
        time = 0f;
        duration *= 2;
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(new Vector3(0, -45, 0)) * startRotation;
        while (time < duration)
        {
            //Debug.Log("Time: " + time);
            time += Time.deltaTime;
            float t = time / duration;
            //Debug.Log("T: " + t);
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
        }*/
        lookingForBone = false;
        yield return null;
    }
}
