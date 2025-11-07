using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public int sortingOffset = 0;
    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        _sr.sortingOrder = (int)(-transform.position.y * 100) + sortingOffset;
    }
}
