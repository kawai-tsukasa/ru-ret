using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime,0f);
    }
}
