using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSight : MonoBehaviour
{
    public float raycastInterval = 0.2f;

    private float startWait = 0.5f, lastRayLength = -1;
    private Vector3 direction, origin;
    void Start()
    {
        StartCoroutine(raycastTestRoutine());
    }

    private IEnumerator raycastTestRoutine()
    {
        bool nohit = true;
        yield return new WaitForSeconds(startWait);
        while (nohit)
        {
            yield return new WaitForSeconds(raycastInterval);

            GameObject player = GameController.getInstance().GetPlayerInControl();
            Vector3 distance = player.transform.position - transform.position;
            direction = distance.normalized;
            origin = transform.position;
            Ray ray = new Ray(origin, direction);

            Debug.Log("Cast Ray origin: " + origin + " direction: " + direction);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                lastRayLength = hitInfo.distance;
                if (hitInfo.transform.gameObject.CompareTag("Player"))
                    nohit = false; // Player was hit by raycast
            }
        }
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        if (lastRayLength >= 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction * lastRayLength);
        }
    }
}
