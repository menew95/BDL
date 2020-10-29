using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;

[System.Serializable]
public class HardRootData
{
    public List<Vector3Int> rootList = new List<Vector3Int>()
    {
        new Vector3Int(),new Vector3Int(),new Vector3Int(),new Vector3Int(),new Vector3Int()
    };

    // 해당 뿌리가 안될시 안되는 목록
    public List<int> third = new List<int>();
    public List<int> fourth = new List<int>();
    public List<int> fifth = new List<int>();
}
[CreateAssetMenu(fileName = "RootDataInfoSO", menuName = "Create RootDataInfo", order = 2)]
[System.Serializable]
public class RootDataInfo : ScriptableObject
{
    public List<HardRootData> up_RootDataList = new List<HardRootData>();
    public List<HardRootData> right_rootDataList = new List<HardRootData>();
    public List<HardRootData> down_rootDataList = new List<HardRootData>();
    public List<HardRootData> left_rootDataList = new List<HardRootData>();
}
