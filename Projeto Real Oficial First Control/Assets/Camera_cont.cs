using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_cont : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = new Quaternion(0, -180, 0, 1);
    }
}
