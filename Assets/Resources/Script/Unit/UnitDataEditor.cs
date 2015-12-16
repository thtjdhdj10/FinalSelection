using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(UnitData))]
[CanEditMultipleObjects]
public class UnitDataEditor : Editor
{
    static TextAsset jsonFile;
    static LitJson.JsonData unitJsonData;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Load Unit Data"))
        {
            LoadJsonData();
            CreatePrefabs();

            UpdateTechTree();
        }
    }

    public static void CreatePrefabs()
    {
        UnitData.unitList.Clear();

        GameObject obj = new GameObject();
        Unit unit = obj.AddComponent<Unit>();
        
        for (int i = 0; i < unitJsonData[FilenameManager.jsonList].Count; ++i)
        {
            var list = unitJsonData[FilenameManager.jsonList][i];
            
            // number 는 1 부터 시작할 것.
            unit.unitNumber = int.Parse(list["number"].ToString());
            unit.conditionUnitNumber = int.Parse(list["conditionUnit"].ToString());
            unit.unitName = list["unitName"].ToString();
            unit.originalAbility.attack = int.Parse(list["attack"].ToString());
            unit.originalAbility.defence = int.Parse(list["defence"].ToString());
            unit.originalAbility.health = int.Parse(list["health"].ToString());
            unit.originalAttackAbility.attackDelay = int.Parse(list["attackDelay"].ToString());
            unit.originalExtraAbility.importance = int.Parse(list["importance"].ToString());
            unit.originalExtraAbility.logicalSize = int.Parse(list["logicalSize"].ToString());
            unit.originalExtraAbility.moveSpeed = int.Parse(list["moveSpeed"].ToString());
            unit.information = list["information"].ToString();

            // UnitType 형 임시변수에 각 enum 값을 넣어 순회의 대상으로 사용한다.
            foreach (var type in (UnitData.UnitType[])System.Enum.GetValues(typeof(UnitData.UnitType)))
            {
                if (type.ToString() == list["enumName"].ToString())
                {
                    unit.enumType = type;
                    break;
                }
            }

            unit.Init();

            GameObject prefab = PrefabUtility.CreatePrefab(
                FilenameManager.prefabPath + list["unitName"].ToString() + ".prefab",
                obj);

            Unit copy = VEasyCalculator.CopyComponent(unit);
            UnitData.unitList.Add(copy);
        }

        DestroyImmediate(obj);
    }

    public static void LoadJsonData()
    {
        if (jsonFile == null)
        {
            jsonFile = Resources.Load(FilenameManager.jsonUnit) as TextAsset;
            if (jsonFile == null)
                Debug.LogError("lost file : " + FilenameManager.jsonUnit);

            unitJsonData = LitJson.JsonMapper.ToObject(jsonFile.text);
        }
    }

    public static void UpdateTechTree()
    {
        List<List<int>> matchedTech = new List<List<int>>(1);
        List<int> conditionNumber = new List<int>();

        // loop 를 돌면서, conditionUnitNumber 가 같은 애들끼리 list 에 넣음
        for (int i = 0; i < UnitData.unitList.Count; ++i)
        {
            int cn = UnitData.unitList[i].conditionUnitNumber;
            int un = UnitData.unitList[i].unitNumber;

            if (conditionNumber.Count == 0)
            {
                Debug.Log("z" + cn + un);
                conditionNumber.Add(cn);

                List<int> unitNumber = new List<int>();
                unitNumber.Add(un);
                matchedTech.Add(unitNumber);
            }
            else
            {
                bool detectUpper = false;
                for (int j = 0; j < conditionNumber.Count; ++j)
                {
                    if (conditionNumber[j] == cn)
                    {
                        Debug.Log("f" + cn + un);
                        detectUpper = true;

                        matchedTech[j].Add(un);
                    }
                }

                // 새로운 conditionNumber 발견
                if (detectUpper == false)
                {
                    Debug.Log("d" + cn + un);
                    conditionNumber.Add(cn);

                    List<int> unitNumber = new List<int>();
                    unitNumber.Add(un);
                    matchedTech.Add(unitNumber);
                }
            }
        }

        for(int i = 0; i < conditionNumber.Count;++i)
        {
            UnitData.techTree[i] = matchedTech[i];
        }
    }

}
