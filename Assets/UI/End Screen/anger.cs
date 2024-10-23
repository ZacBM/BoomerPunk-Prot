using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anger : MonoBehaviour
{
    /// <remarks>
    /// Changes to make:
    /// - Rename to "EndScreen"
    /// 
    /// - Joshua  
    /// </remarks>

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }
}
