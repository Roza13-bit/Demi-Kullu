using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandController : MonoBehaviour
{
    [SerializeField] private Vector3 standTranform0;

    [SerializeField] private Vector3 standTranform1;

    [SerializeField] private Vector3 standTranform2;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("cameraPick = " + CameraPicker.cameraPick);

        switch (CameraPicker.cameraPick)
        {
            case 0:

                transform.position = standTranform0;

                break;
            case 1:

                transform.position = standTranform1;

                break;
            case 2:

                transform.position = standTranform2;

                break;
        }

    }

}
