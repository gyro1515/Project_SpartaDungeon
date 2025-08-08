using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Object", menuName = "New Object")]
public class ObjectData : ScriptableObject
{
    [Header("오브젝트 정보")]
    public string displayName;
    public string description;
}
