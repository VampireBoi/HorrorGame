using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "Items/ new item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
}
