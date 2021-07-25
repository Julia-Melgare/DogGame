using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour

{
    private GameObject bone;
    public float speed = 0.8f;

    void Start()
    {
        bone = GameObject.Find("Bone");
    }

    void Update()
    {
        Vector3 direction = bone.transform.position - transform.position;
        Debug.DrawLine(transform.position, bone.transform.position);
        Vector3 newPos = (direction * speed) * Time.deltaTime;
        transform.position += new Vector3(newPos.x, 0, newPos.z);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

    }
}
