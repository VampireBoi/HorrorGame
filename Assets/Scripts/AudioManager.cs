using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public Sound[] sounds;
    
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        foreach (Sound sound in sounds)
        {
            AudioSource[] source = sound.Object.GetComponents<AudioSource>();
                    
            if(source != null)
            {
                foreach (AudioSource s in source)
                {                   
                    if(s.clip.name == sound.clip.name)
                    {
                        sound.source = s;   
                        sound.clip= s.clip;
                        sound.volume = s.volume;
                        sound.pitch = s.pitch;
                        sound.loop = s.loop;
                    }
                }                             
            }
            if (sound.source == null)
            {
                sound.source = sound.Object.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }     
    }

    // Update is called once per frame
    public void playSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if(sound.name == name)
            {
                if (sound.allowOverlaping)
                {
                    sound.source.Play();
                }
                else if (!sound.source.isPlaying)
                {
                    sound.source.Play();
                }             
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

    public void pauseSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.Pause();
                
            }
        }
    }
    public void unPauseSound(string name)
    {
        
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.UnPause();
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

    public void stopAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            stopSound(sound.name);
        }
    }

    public void speedSound(string name ,float a)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.pitch += a;
            }
        }
    }

    public bool isSoundPlaying(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                return sound.source.isPlaying;
            }
        }
        Debug.Log("sound not found");
        return false;
    }
}
