using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 0.3f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        foreach (ContactPoint contact in collision.contacts)
    //        {
    //            if (contact.normal.y > 0.5f)
    //            {
    //                collision.transform.SetParent(transform);
    //                break;
    //            }
    //        }
    //    }
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //        collision.transform.SetParent(null);
    //}
}
