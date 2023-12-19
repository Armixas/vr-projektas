using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckSolution : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Tables;

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
        }
    }
}
