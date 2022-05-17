using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource fondo;

    // Start is called before the first frame update
    void Start()
    {
        fondo = this.gameObject.GetComponent<AudioSource>();
        fondo.Play();
        fondo.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
