using UnityEngine;
using System.Collections.Generic;

public class UnitData : MonoBehaviour {

    public static List<Unit> unitList = new List<Unit>();

    public static Dictionary<int, List<int>> techTree = new Dictionary<int, List<int>>();

    public enum UnitType
    {
        UNIT_NONE = 0,
        UNIT_ZERGGLING,
    }

    void Awake()
    {
        UnitDataEditor.LoadJsonData();
        UnitDataEditor.CreatePrefabs();
        UnitDataEditor.UpdateTechTree();
        
    }

    void Start()
    {
        PrintLoadingUnitList();
    }

    void PrintLoadingUnitList()
    {
        Debug.Log("Loading Unit Count : " + unitList.Count);

        for (int i = 0; i < unitList.Count; ++i)
        {
            if(i == 0)
                Debug.Log(i+1 + "st" + unitList[i].unitName);
            else if(i == 1)
                Debug.Log(i+1 + "nd" + unitList[i].unitName);
            else if(i == 2)
                Debug.Log(i+1 + "rd" + unitList[i].unitName);
            else
                Debug.Log(i+1 + "th" + unitList[i].unitName);
        }
    }

    public static GameObject CreateUnit(UnitType ut)
    {
        for(int i = 0; i < unitList.Count;++i)
        {
            if(unitList[i].enumType == ut)
            {
                return VEasyPoolerManager.GetObjectRequest(unitList[i].unitName);
            }
        }

        Debug.LogError("Non-Loading UnitType : " + ut);
        return null;
    }

}
