using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimController : MonoBehaviour
{
    [Header("Cached Referances")]

    [SerializeField] private float aimMarbleSpeed;

    // Private Variables.

    private Joystick joystick;

    private Rigidbody aimMarbleRB;

    private bool isJoystickEnabled;

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();

        aimMarbleRB = GetComponent<Rigidbody>();

        isJoystickEnabled = false;

    }

    public void StartJoystickMotion()
    {
        isJoystickEnabled = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isJoystickEnabled)
        {
            aimMarbleRB.velocity = new Vector3(joystick.Horizontal * aimMarbleSpeed, aimMarbleRB.velocity.y, joystick.Vertical * aimMarbleSpeed);

        }
        
    }

}
