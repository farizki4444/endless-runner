using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pindahscene : MonoBehaviour
{

    void Start()
    {
        
    }

    public void PindahScene()
{

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

   
}  

