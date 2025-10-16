using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawnerPrefab : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    public GameObject cloudParent;
    private float timer = 4f;

    public void Update()
    {
        timer += Time.deltaTime;
        
        if(timer > 3f)
        {
            timer = 0f;

            int count = Random.Range(1,3);
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, cloudPrefabs.Length);
                SpawnPrefab(index);
            }
        }
    }

    void SpawnPrefab(int index)
    {
        Debug.Log("Spawn Cloud Prefab");
        Vector3 pos = GetSpawnPos();

        GameObject newCloud = Instantiate(cloudPrefabs[index], pos, Quaternion.identity);
        newCloud.transform.SetParent(cloudParent.transform, false);
        newCloud.transform.localPosition = pos;

    }

    Vector3 GetSpawnPos()
    {
        float range = Random.Range(50f, 450f);

        return new Vector3(transform.position.x, range, transform.position.z);
    }
}
