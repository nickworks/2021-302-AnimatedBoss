using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianMovement : MonoBehaviour
{

    public Animator animMachine;

    void Start()
    {
        animMachine = GetComponent<Animator>();
    }

    void Update()
    {
        float speed = Input.GetAxisRaw("Vertical");

        animMachine.SetFloat("current speed", speed);

        transform.position += transform.forward * speed * Time.deltaTime * 3;

    }
}
