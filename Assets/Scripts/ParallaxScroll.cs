using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Vector2 offset;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        offset = Vector2.zero;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
