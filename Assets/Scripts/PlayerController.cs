
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    [HideInInspector]
	public float horizontal;
    [HideInInspector]
    public float vertical;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public Vector3 moveDirection;

	private Animator anim;
    private CharacterController controller;
	private Camera cam;
    public float playerRotation = 0.1f;

    // Use this for initialization
    void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		InputMagnitude ();

        if (speed > 0)
            anim.SetBool("walking", true);
        else
            anim.SetBool("walking", false);
    }

	void PlayerMoveAndRotation() {
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		moveDirection = forward * vertical + right * horizontal;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), playerRotation);
        controller.Move(moveDirection * Time.deltaTime * 3);
       
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        moveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), playerRotation);
    }

	void InputMagnitude() {

		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");

		speed = new Vector2(horizontal, vertical).sqrMagnitude;

        if(speed > 0)
		    PlayerMoveAndRotation ();
		
	}
}
