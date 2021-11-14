using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    [SerializeField]
    private Rigidbody _rigidBody;
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    public bool _isGrounded;



    public float groundDistance = 0.4f;
    public float speed  = 15f;
    public float jumpHeight = 3f;
    private float gravity = -9.81f * 2;
    private Vector3 velocity;
   


    void Start(){

    }

    void Update(){
        PlayerMovement();
    }

    private void PlayerMovement(){
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");
        
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(_isGrounded && velocity.y < 0) velocity.y = -2f;
        if(Input.GetButtonDown("Jump") && _isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        Vector3 move = transform.right * xAxis + transform.forward * zAxis;
        _controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }

}
