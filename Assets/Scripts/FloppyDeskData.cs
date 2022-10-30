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

    [Range(0.3f, 2f)] public float difficulty; 
    public float timeToFinichTheGame;
    
    public string[] firstDialogue;
    public string[] LastDialogue;
    public int levelsInDesk;  
    
    [HideInInspector] public bool isFinished;

    public GameObject levelManager;

    public bool firstInsertion = true;
}
