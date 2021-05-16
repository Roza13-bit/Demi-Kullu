using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPicker : MonoBehaviour
{
    public static int cameraPick;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);  
    }

    public void PickLowCamera()
    {
        cameraPick = 0;

        //Debug.Log("cameraPick = " + cameraPick);

    }

    public void PickHigherCamera()
    {
        cameraPick = 1;

        //Debug.Log("cameraPick = " + cameraPick);

    }

    public void PickHighestCamera()
    {
        cameraPick = 2;

        //Debug.Log("cameraPick = " + cameraPick);

    }

    public void LoadTheFirstScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}
