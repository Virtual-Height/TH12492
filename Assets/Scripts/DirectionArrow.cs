using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hit, 50f))
        {
            if (hit.collider.gameObject != gameObject)
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y;
                transform.position = pos;
            }
        }
    }
}
