using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public Sound[] sounds;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        
        foreach (Sound sound in sounds)
        {
            sound.source = sound.Object.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
        playSound("horror ambience");
    }

    // Update is called once per frame
    public void playSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if(sound.name == name)
            {
                sound.source.Play();
            }
        }
    }
    public void stopSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.Stop();
            }
        }
    }

    public void changePitch(string name, float pitch)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.pitch = pitch;
            }
        }
    }
}
