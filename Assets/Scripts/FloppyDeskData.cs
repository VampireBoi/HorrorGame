using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new floppy disk", menuName = "Items/ new floppy disk")]
public class FloppyDeskData : ScriptableObject
{
    public string gameTitle;
    [Header("the game title color")]
    public Color gameTitleScreenColor;
    
    [Header("the color of the screen light")]
    public Color screenLightColor;
 
    public string[] firstDialogue;
    public string[] LastDialogue;
    
    [HideInInspector] public bool isFinished;

    public GameObject miniGame;

    public bool firstInsertion = true;
}
