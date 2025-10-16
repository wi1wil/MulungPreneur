using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    public GameObject[] cloudPrefabs;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public GameObject cloudParent;
    public float spawnInterval = 3f;
    public float offsetRange = 10f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > spawnInterval)
        {
            _timer = 0f;

            int count = Random.Range(1, 3);
            for (int i = 0; i < count; i++)
            {
                int prefabIndex = Random.Range(0, cloudPrefabs.Length);
                int spawnIndex = Random.Range(0, spawnPoints.Length);
                SpawnCloud(prefabIndex, spawnIndex);
            }
        }
    }

    private void SpawnCloud(int prefabIndex, int spawnIndex)
    {
        Transform spawn = spawnPoints[spawnIndex];

        Vector3 spawnPos = spawn.position + new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), 0f);

        GameObject newCloud = Instantiate(cloudPrefabs[prefabIndex], spawnPos, Quaternion.identity, cloudParent.transform);
    }
}
