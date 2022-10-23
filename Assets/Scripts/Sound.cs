using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
     
    [Range(0f, 1f)]
    public float volume;
    
    [Range(0f, 3f)]
    public float pitch;
    
    // the game object that we are going to attach the audio sorce into 
    public GameObject Object;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
}
