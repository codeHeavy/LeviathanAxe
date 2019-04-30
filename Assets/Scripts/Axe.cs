using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public bool thrown = false;
    public float rotationSpeed = 30;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thrown)
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        thrown = false;
    }
}
