using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawnerPrefab : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
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
        Vector3 pos = GetSpawnPos();

        GameObject child = Instantiate(cloudPrefabs[index], pos, Quaternion.identity);

        child.transform.parent = this.transform;
    }

    Vector3 GetSpawnPos()
    {
        float range = Random.Range(600f, 1000f);

        return new Vector3(this.transform.position.x, range, this.transform.position.z);
    }
}
