using UnityEngine;

public class TowManager : MonoBehaviour
{
    [SerializeField] private Transform[] towPoints;
    [SerializeField] private Transform player;

    public void TowPlayer()
    {
        player.position = NearestPoint().position;
    }

    private Transform NearestPoint()
    {
        // = null needs to remove return error
        Transform toReturn = null;

        Vector3 nearestPoint = new(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 playerPosition = player.position;

        // Iterating points
        foreach (Transform point in towPoints)
        {
            Vector3 pointPosition = point.position;
            float distanceToNearest = Vector3.Distance(pointPosition, playerPosition);

            // Check if current point closer than previous
            if (distanceToNearest < nearestPoint.magnitude)
            {
                nearestPoint = pointPosition;
                toReturn = point;
            }
        }

        return toReturn;
    }
}
