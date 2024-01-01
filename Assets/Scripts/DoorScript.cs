using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public FadeScript fadeScript;

    public void SwitchLevel()
    {
        StartCoroutine(SwitchRoutine());
    }

    IEnumerator SwitchRoutine()
    {
        fadeScript.FadeOut();
        yield return new WaitForSeconds(fadeScript.fadeDuration);
        SceneManager.LoadScene("Sword Level");
    }
}
