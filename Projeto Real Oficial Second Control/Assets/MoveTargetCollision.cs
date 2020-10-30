using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetCollision : MonoBehaviour
{
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.localPosition, Vector3.up, out hit))
        {
            if (hit.collider.tag == "Environment")
            {
                Debug.Log("Target Moved Raycast!");
                transform.position += Vector3.up * 15;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.transform.localPosition = new Vector3(Random.Range(-280, 280),
                                           16f,
                                           Random.Range(-280, 280));
        Debug.Log("Target Moved Collision!");
    }


}
