using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour {

    public int basePoints = 3;

    public List<MonsterStats> monsterPrefabs = new List<MonsterStats>();

    public Transform canvasTransf;

    
    public List<GameObject> SpawnMonsters(int floorLevel)
    {
        List<GameObject> monstersSpawned = new List<GameObject>();

        // We have points to spawn mobs based on the floor level
        int points = basePoints + floorLevel * 2;

        while (points > 0)
        {
            List<MonsterStats> eligibleMobs = new List<MonsterStats>();
            foreach (MonsterStats ms in monsterPrefabs)
            {
                if (ms.requiredFloorLevel <= floorLevel && ms.spawnCost <= points)
                {
                    eligibleMobs.Add(ms);
                }
            }

            if(eligibleMobs.Count == 0)
            {
                Debug.LogError("Floor : " + floorLevel + ", points : " + points + " => No eligible monsters");
                break;
            }
            else
            {
                MonsterStats mob = eligibleMobs[Random.Range(0, eligibleMobs.Count)];

                // /!\ WARNING valeurs piffées plus ou moins
                float randX = Random.Range(6.0f, 23.0f);
                float randY = Random.Range(6.0f, 23.0f);
                if (Random.Range(0, 2) == 1) randX = -randX;
                if (Random.Range(0, 2) == 1) randY = -randY;

                GameObject mobSpawned = Instantiate(mob.monsterPrefab, new Vector3(randX, randY, 0f), mob.monsterPrefab.transform.rotation);
                GameObject pointerToMob = Instantiate(mob.pointerPrefab, canvasTransf);
                mobSpawned.GetComponent<MonsterStats>().pointerGO = pointerToMob;
                pointerToMob.GetComponent<PointToMob>().target = mobSpawned;

                monstersSpawned.Add(mobSpawned);
                points -= mob.spawnCost;
            }
        }

        return monstersSpawned;
    }
    
}
