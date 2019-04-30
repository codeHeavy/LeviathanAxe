using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrow : MonoBehaviour
{
    Animator anim;
    PlayerController controller;
    bool aiming = false;
    public bool equipped = true;

    [SerializeField]
    Rigidbody axe;
    public float throwForce = 3;

    public Transform hand;
    public Transform bendLocation;
    private Transform axeTransform;
    Vector3 axeStopPosition;
    Vector3 axeStartPosition;
    Vector3 axeStartWrldPosition;
    Vector3 axeStartRotation;

    float returnTime;
    bool recallAxe = false;

    TrailRenderer axeTrail;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        axeTransform = axe.gameObject.transform;
        axeStartPosition = axeTransform.localPosition;
        axeStartRotation = axeTransform.localEulerAngles;
        axeStartWrldPosition = axeTransform.position;
        axeTrail = axeTransform.GetComponent<TrailRenderer>();
        axeTrail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Aim();
        if(!equipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                axeStopPosition = axeTransform.position;
                recallAxe = true;
                axe.gameObject.GetComponent<Axe>().thrown = true;
                axeTrail.enabled = true;
            }
            if (recallAxe)
                ReturnAxe();
        }
    }

    public void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            controller.RotateToCamera(transform);
            aiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
            aiming = false;
        anim.SetBool("aiming", aiming);

        if (aiming && Input.GetMouseButtonDown(0) && equipped)
        {
            anim.SetTrigger("throw");
        }
    }

    public void Throw()
    {
        axeTrail.enabled = true;
        equipped = false;
        axe.isKinematic = false;
        axe.transform.parent = null;
        axe.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        axe.gameObject.GetComponent<Axe>().thrown = true;
    }
    public void ReturnAxe()
    {
        if (returnTime < 1)
        {
            axeTransform.position = BezierCurve(returnTime, axeStopPosition , bendLocation.position, hand.position);
            returnTime += Time.deltaTime * 1.5f;
        }
        else
        {
            recallAxe = false;
            axeTransform.parent = hand;
            axeTrail.enabled = false;
            axe.gameObject.GetComponent<Axe>().thrown = false;
            axeTransform.localPosition = axeStartPosition;
            axeTransform.localEulerAngles = axeStartRotation;
            returnTime = 0;
            equipped = true;
        }
    }

    public Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

}
