using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHeadController : MonoBehaviour
{
    private GameObject bone;
    // Start is called before the first frame update
    void Start()
    {
        bone = GameObject.Find("Bone");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = bone.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
    }
}
