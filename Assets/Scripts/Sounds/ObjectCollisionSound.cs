using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisionSound : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sounds to play when the object collides")]
    List<AudioClip> Sounds;

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioSource>().PlayOneShot(Sounds[Random.Range(0, Sounds.Count - 1)], 0.75F);
    }
}
