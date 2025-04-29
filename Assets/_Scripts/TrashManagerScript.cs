using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] trashPrefabs;
    [SerializeField] private int trashCount;
    [SerializeField] private float spawnRate;
    private Collider2D mapBounds; 

    // Start is called before the first frame update
    void Start()
    {
        // This is a test
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
