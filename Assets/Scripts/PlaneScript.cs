using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    Rigidbody rb;
    public float forwardMultiplier=50;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*500);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(-transform.position*Physics.gravity.magnitude*Time.deltaTime);
        rb.AddForce(transform.forward *Time.deltaTime*forwardMultiplier);

        /*        transform.rotation.SetLookRotation(Vector3.Cross(transform.right, -transform.position),-transform.position );
        */

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Vector3 reflect = Vector3.Reflect(-transform.up, hit.normal);
            
        }

        if (Input.GetAxis("Jump")>0)
        {
            rb.AddForce(transform.up*Time.deltaTime*120);
        }
        if (Input.GetAxis("Vertical")>0)
        {
            rb.AddForce(transform.up*-1 *Time.deltaTime*60);
        }

        transform.RotateAround(transform.position, hit.normal, Input.GetAxis("Horizontal")*10*Time.deltaTime);
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position+transform.up);
    }
}
