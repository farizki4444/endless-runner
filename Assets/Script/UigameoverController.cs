using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UigameoverController : MonoBehaviour
{
    // Start is called before the first frame update
   
    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
