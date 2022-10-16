using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new floppy disk", menuName = "Items/ new floppy disk")]
public class FloppyDeskData : ScriptableObject
{
    [Range(0.3f, 2f)]
    public float difficulty;
    public string startScene;
    public string exitScene;
    public int levelsInDesk;  
    [HideInInspector]
    public bool isFinished;

    [HideInInspector]
    public bool firstInsertion;
}
