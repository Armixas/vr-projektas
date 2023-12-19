using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public void SwitchLevel()
    {
        SceneManager.LoadScene("Sword Level");
    }
}
