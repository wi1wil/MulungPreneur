using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPrefab : MonoBehaviour
{
    private float _speed;

    public void Start()
    {
        _speed = Random.Range(0.2f, 1f);
    }

    public void Update()
    {
        this.transform.position -= new Vector3(_speed, 0f, 0f);

        CheckForOutOfBounds(gameObject);
    }

    void CheckForOutOfBounds(GameObject obj)
    {
        if(this.transform.position.x < -600f)
        {
            Destroy(obj);
        }
    }
}
