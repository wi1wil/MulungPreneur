using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D insertedBounds;
    [SerializeField] private BoxCollider2D G_WaypointObj;
    [SerializeField] private Direction direction;
    [SerializeField] private float offset;
    CinemachineConfiner confiner;
    enum Direction { Up, Down, Left, Right } 

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = insertedBounds;
            // G_WaypointObj.isTrigger = false;

            UpdatePlayerPosition(collision.gameObject);
            Debug.Log("Player exited the Tutorial bounds.");
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPosition = player.transform.position;
        
        switch (direction)
        {
            case Direction.Up:
                newPosition.y += offset;
                break;
            case Direction.Down:
                newPosition.y -= offset;
                break;
            case Direction.Left:
                newPosition.x -= offset;
                break;
            case Direction.Right:
                newPosition.x += offset;
                break;
        }

        player.transform.position = newPosition;
    }
}
