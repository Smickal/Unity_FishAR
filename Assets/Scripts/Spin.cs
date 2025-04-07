using UnityEngine;

public class Spin : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 100f * Time.deltaTime, 0f));
    }
}
