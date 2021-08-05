using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour

{
    public float speed;
    public float enemySensorAngle;
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

    //TODO: use this as base when the first enemy is added
    /*void LookForEnemies()
    {
        GameObject[] buffer = new GameObject[10];
        if (los.Filter(buffer, "Enemies") > 0)
        {
            var enemy = buffer[0];
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 dir = dest - origin;
            dir.y = 0;
            float deltaAngle = Vector3.Angle(dir, transform.forward);
            if (deltaAngle > enemySensorAngle)
            {
                //TODO: do nothing
            }
        }
    }*/
}
