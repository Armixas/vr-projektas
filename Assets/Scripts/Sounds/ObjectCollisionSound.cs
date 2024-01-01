using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisionSound : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sounds to play when the object collides")]
    List<AudioClip> Sounds;

    private float startTime;
    private bool Enabled = false;

    private void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if(Time.time - startTime >= 2)
        {
            Enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Enabled)
        {
            GetComponent<AudioSource>().PlayOneShot(Sounds[Random.Range(0, Sounds.Count - 1)], 0.75F);
        }
    }
}
