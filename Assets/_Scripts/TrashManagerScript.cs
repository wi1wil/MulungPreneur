using Unity.Profiling;
using Unity.VisualScripting;
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
        int index = Random.Range(0, trashPrefabs.Length);
        GameObject spawned = Instantiate(trashPrefabs[index], randomPos, Quaternion.identity);
        if (trashObjects != null)
        {
            spawned.transform.SetParent(trashObjects.transform);
        }
    }
}
