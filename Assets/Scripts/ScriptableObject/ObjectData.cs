using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Object", menuName = "New Object")]
public class ObjectData : ScriptableObject
{
    [Header("오브젝트 정보")]
    public string displayName;
    public string description;
    public Vector3 uIPos = Vector3.zero; // 오브젝트 위치에서 보정에서 출력되는 UI 위치
    public Vector3 screenPos = Vector3.zero; // 화면에서 보정된 UI 위치
}
