using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeAnim : MonoBehaviour
{
    public static fadeAnim instance;
    public float fadeSpeed;
    public float TimeBetweenFades;
    [HideInInspector] public bool isFading;
    Image fade;
    float alphaValue;


    private void Start()
    {
        isFading = true;
        instance = this;
        fade = transform.GetComponent<Image>();
        fade.raycastTarget = false;

    }

    private void Update()
    {
        alphaValue = Mathf.Clamp01(alphaValue);
        if (fade.gameObject.activeSelf)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alphaValue);
        }

    }
  
    public void startFadeAnim()
    {
        StartCoroutine(fadeIn());
    }
    
    IEnumerator fadeIn()
    {
        while (true)
        {
            fade.raycastTarget = true;
            yield return new WaitForEndOfFrame();
            if (alphaValue >= 1f)
            {
                yield return new WaitForSeconds(TimeBetweenFades);
                StartCoroutine(fadeOut());
                break;
            }
            alphaValue += Time.deltaTime * fadeSpeed;
        }    
    }

    IEnumerator fadeOut()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (alphaValue <= 0f)
            {
                fade.raycastTarget = false;
                break;
            }
            alphaValue -= Time.deltaTime * fadeSpeed;
        }
    }




}
