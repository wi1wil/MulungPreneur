using JetBrains.Annotations;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class TrashManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] trashPrefabs;
    [SerializeField] private Collider2D mapBounds;
    [SerializeField] private GameObject trashObjects;
    public int trashCount;
    public int maxTrash;
    public float spawnRate;
    public float timer;
    public int deleteTime;

    void Update()
    {
        trashCount = 0;
        foreach (var trash in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (mapBounds.OverlapPoint(trash.transform.position))
            {
                trashCount++;
            }
        }

        if (trashCount < maxTrash)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                spawnTrash();
                timer = spawnRate;
            }
        }
    }

    public void spawnTrash()
    {
        Vector2 randomPos = new Vector2(Random.Range(mapBounds.bounds.min.x, mapBounds.bounds.max.x), Random.Range(mapBounds.bounds.min.y, mapBounds.bounds.max.y));

        Collider2D[] hits = Physics2D.OverlapPointAll(randomPos);
        bool foundGround = false;
        foreach (var hit in hits)
        {
            if (hit != null && hit.CompareTag("SpawnableArea"))
            {
                // Debug.Log("Spawned trash at: " + randomPos);
                foundGround = true;
                break;
            }
        }
        if (!foundGround)
        {
            return;
        }

        int index = Random.Range(0, trashPrefabs.Length);
        GameObject spawned = Instantiate(trashPrefabs[index], randomPos, Quaternion.identity);
        // Debug.Log("Spawned trash: " + spawned.name);
        if (trashObjects != null)
        {
            spawned.transform.SetParent(trashObjects.transform);
        }
        
        Destroy(spawned, deleteTime);
    }
}
