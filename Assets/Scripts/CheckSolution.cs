using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class CheckSolution : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Tables;

    [SerializeField]
    Animator DoorHingeAnimator;

    [SerializeField]
    AudioSource DoorHingeAudioSource;

    [SerializeField]
    FadeScript fadeScript;

    List<GameObject> TableItems = new List<GameObject>() {null, null, null, null, null};



    public void Add(SelectEnterEventArgs args)
    {
        for(int i = 0; i<5; i++)
        {
            if(Tables[i].GetComponentInChildren<XRSocketInteractor>() == args.interactorObject)
            {
                TableItems[i] = args.interactableObject.transform.gameObject;
            }
        }
        Check();
    }

    public void Remove(SelectExitEventArgs args)
    {
        for (int i = 0; i < 5; i++)
        {
            if (Tables[i].GetComponentInChildren<XRSocketInteractor>() == args.interactorObject)
            {
                TableItems[i] = null;
            }
        }
    }

    public void Check()
    {
        bool solved = true;
        for (int i = 0; i < 5; i++)
        {
            if (TableItems[i] == null || (TableItems[i] != null && TableItems[i].tag != (i + 1).ToString()))
            {
                solved = false;
                break;
            }
        }
        if(solved)
        {
            DoorHingeAudioSource.Play();
            DoorHingeAnimator.enabled = true;
            StartCoroutine(TransferToMenuRoutine(4));
        }
    }

    public IEnumerator TransferToMenuRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        fadeScript.fadeDuration = 2;
        fadeScript.FadeOut();
        yield return new WaitForSeconds(fadeScript.fadeDuration);
        SceneManager.LoadScene("New Scene 1");
    }
}
