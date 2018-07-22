using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour {
    [Tooltip("The cost to spawn one copy of the monster")]
    public int spawnCost = 1;
    [Tooltip("The minimum floor level where this monster is allowed to spawn")]
    public int requiredFloorLevel = 0;

    public GameObject monsterPrefab;

    public GameObject pointerPrefab;

    [HideInInspector]
    public GameObject pointerGO; // The actual pointer of the specific mob
}
