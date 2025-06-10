using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPrefabScript : MonoBehaviour
{
    private float speed;

    public void Start()
    {
        speed = Random.Range(0.2f, 0.5f);
    }

    public void Update()
    {
        this.transform.position -= new Vector3(speed, 0f, 0f);

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
