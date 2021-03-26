using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonController : MonoBehaviour
{

    public enum States {
        Idle,
        Walk
    }

    private CharacterController pawn;

    public float moveSpeed = 5;
    public float stepSpeed = 5;

    public Vector3 walkScale = Vector3.one;

    public AnimationCurve ankleRotationCurve;


    public States state { get; private set; }
    public Vector3 moveDir { get; private set; }


    void Start()
    {
        state = States.Idle;
        pawn = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDir = transform.forward * v + transform.right * h;
        if(moveDir.sqrMagnitude > 1) moveDir.Normalize();

        pawn.SimpleMove(moveDir * moveSpeed);


        state = (moveDir.sqrMagnitude > .1f) ? States.Walk : States.Idle;

    }
}
