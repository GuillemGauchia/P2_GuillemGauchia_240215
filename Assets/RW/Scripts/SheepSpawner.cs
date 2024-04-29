using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    public bool canSpawn = true;
    public GameObject sheepPrefab;
    public List<Transform> sheepSpawnPositions = new List<Transform>();
    private float timeBetweenSpawns = 2.0f;
    private List<GameObject> sheepList = new List<GameObject>();
    private int spawnReductionCount = 0;

    void Start()
    {
        //timeBetweenSpawns = Random.Range(1.0f, 3.0f);
        StartCoroutine(SpawnRoutine());
        StartCoroutine(ReduceSpawnTime());
    }

    private void SpawnSheep()
    {
        Vector3 randomPosition = sheepSpawnPositions[Random.Range(0, sheepSpawnPositions.Count)].position;
        GameObject sheep = Instantiate(sheepPrefab, randomPosition, sheepPrefab.transform.rotation);
        sheepList.Add(sheep);
        sheep.GetComponent<Sheep>().SetSpawner(this);
    }

    private IEnumerator SpawnRoutine()
    {
        while (canSpawn)
        {
            SpawnSheep();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ReduceSpawnTime()
    {
        while (spawnReductionCount < 15)
        {
            yield return new WaitForSeconds(5.0f);
            timeBetweenSpawns -= 0.1f;
            spawnReductionCount++;
        }
    }

    public void RemoveSheepFromList(GameObject sheep)
    {
        sheepList.Remove(sheep);
    }

    public void DestroyAllSheep()
    {
        foreach (GameObject sheep in sheepList)
        {
            Destroy(sheep);
        }

        sheepList.Clear();
    }

    void Update()
    {

    }
}
