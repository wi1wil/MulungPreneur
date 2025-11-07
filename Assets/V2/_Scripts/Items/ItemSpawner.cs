using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Items to Spawn")]
    public ItemsSO[] itemsSO;          
    public GameObject trashPrefab;      
    public GameObject trashContainer; 
    public Collider2D mapBounds;

    [Header("Spawn Settings")]
    public int maxTrash;
    public float spawnRate;
    public int deleteTime;

    private float timer;
    private int trashCount;

    void Update()
    {
        trashCount = 0;
        foreach (var trash in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (mapBounds.OverlapPoint(trash.transform.position))
                trashCount++;
        }

        if (trashCount < maxTrash)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpawnTrash();
                timer = spawnRate;
            }
        }
    }

    void SpawnTrash()
    {
        if (itemsSO.Length == 0 || trashPrefab == null) return;

        // Pick a random item
        int index = Random.Range(0, itemsSO.Length);
        ItemsSO itemToSpawn = itemsSO[index];

        // Random position within bounds
        Vector2 randomPos = new Vector2(
            Random.Range(mapBounds.bounds.min.x, mapBounds.bounds.max.x),
            Random.Range(mapBounds.bounds.min.y, mapBounds.bounds.max.y)
        );

        // Check for spawnable area
        Collider2D[] hits = Physics2D.OverlapPointAll(randomPos);
        bool foundGround = false;
        foreach (var hit in hits)
        {
            if (hit.CompareTag("SpawnableArea"))
            {
                foundGround = true;
                break;
            }
        }
        if (!foundGround) return;

        // Instantiate item prefab
        GameObject spawned = Instantiate(trashPrefab, randomPos, Quaternion.identity);

        // Initialize it with the ItemsSO
        ItemPrefab itemComponent = spawned.GetComponent<ItemPrefab>();
        if (itemComponent != null)
        {
            itemComponent.Initialize(itemToSpawn, 1);
        }

        // Set parent
        if (trashContainer != null)
            spawned.transform.SetParent(trashContainer.transform);

        // Destroy after time
        StartCoroutine(FlickerBeforeDestroy(spawned, deleteTime));
    }

    private IEnumerator FlickerBeforeDestroy(GameObject obj, float totalLifetime)
    {
        float flickerDuration = 3f;
        float flickerSpeed = 0.15f;

        yield return new WaitForSeconds(totalLifetime - flickerDuration);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        ItemPrefab item = obj.GetComponent<ItemPrefab>();
        if (sr == null) yield break;

        float elapsed = 0f;
        while (elapsed < flickerDuration)
        {
            // If being interacted with, pause flickering
            if (item != null && item.IsBeingInteractedWith)
            {
                sr.enabled = true;
                yield return null;
                continue;
            }

            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        sr.enabled = true;

        // Wait for interaction to end before actually destroying
        if (item != null)
        {
            while (item.IsBeingInteractedWith)
                yield return null;
        }

        Destroy(obj);
    }
}
