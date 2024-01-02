using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckSolution : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Tables;

    [SerializeField]
    GameObject DoorHinge;

    [SerializeField]
    float DoorOpenDuration;

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
            // Solution correct, do something

            // Open door, enable level switching
            StartCoroutine(DoorOpenRoutine());
        }
    }

    public IEnumerator DoorOpenRoutine()
    {
        float timer = 0;
        while (timer <= DoorOpenDuration)
        {
            var newRotation = new Vector3(0,0,0);    
            newRotation.y = Mathf.Lerp(0, -90, timer/DoorOpenDuration);
            var newPosition = new Vector3(-0.469f, 1.5f, 0);
            newPosition.x = Mathf.Lerp(-0.469f, -1.689f, timer / DoorOpenDuration);
            newPosition.z = Mathf.Lerp(0, 1.24f, timer / DoorOpenDuration);

            DoorHinge.transform.localEulerAngles = newRotation;
            DoorHinge.transform.localPosition = newPosition;

            timer += Time.deltaTime;
            yield return null;
        }

        var newRotation2 = new Vector3(0, 0, 0);
        newRotation2.y = -90;
        var newPosition2 = new Vector3(-0.469f, 1.5f, 0);
        newPosition2.x = -1.689f;
        newPosition2.z = 1.24f;

        DoorHinge.transform.localEulerAngles = newRotation2;
        DoorHinge.transform.localPosition = newPosition2;
    }
}
