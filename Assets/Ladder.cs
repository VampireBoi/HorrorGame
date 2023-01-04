using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject player;
    public Transform roofPoint;
    public Transform roomPoint;
    bool inTheAttic;
    // Start is called before the first frame update
   
    // Update is called once per frame
  

    public void useLadder()
    {
        StartCoroutine(movePlayer());
    }

    IEnumerator movePlayer()
    {
        fadeAnim.instance.startFadeAnim();
        yield return new WaitForSeconds(fadeAnim.instance.TimeBetweenFades / 2);

        if (!inTheAttic)
        {
            player.transform.position = roofPoint.position;
            inTheAttic = true;
        }
        else
        {
            player.transform.position = roomPoint.position;
            inTheAttic = false;
        }


    }
}
