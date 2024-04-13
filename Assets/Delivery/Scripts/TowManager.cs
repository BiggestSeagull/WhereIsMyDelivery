using UnityEngine;

public class TowManager : MonoBehaviour
{
    [SerializeField] private Transform[] towPoints;
    [SerializeField] private Transform player;

    public void TowPlayer()
    {
        player.position = NearestPoint().position;
        player.rotation = Quaternion.Euler(0f, player.rotation.eulerAngles.y, 0f);
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private Transform NearestPoint()
    {
        Transform toReturn = null;

        float nearestDistance = float.MaxValue;
        Vector3 playerPosition = player.position;

        foreach (Transform point in towPoints)
        {
            Vector3 pointPosition = point.position;
            float distanceToPlayer = Vector3.Distance(pointPosition, playerPosition);

            // Check if current point closer than previous
            if (distanceToPlayer < nearestDistance)
            {
                nearestDistance = distanceToPlayer;
                toReturn = point;
            }
            //Debug.Log("foreach " + point.gameObject.name);
        }
        //if (toReturn != null)
        //    Debug.Log("out " + toReturn.gameObject.name);
        //else
        //    Debug.Log("out null");

        return toReturn;
    }
}
