using UnityEngine;

public class tragectoryLine : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField, Min(3)] int lineSegments = 60;

    [SerializeField, Min(1)] float timeOfFlight = 50f;


    private Vector3[] CalcTragectoryLine(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];

        lineRendererPoints[0] = startPoint;
        //tragectory formula
        for(int i = 1; i < lineSegments; i++)
        {
            float timeOffset = timeStep * i;

            Vector3 progressBeforeGravity = startVelocity * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPosition = startPoint + progressBeforeGravity + gravityOffset;

            lineRendererPoints[i] = newPosition;
        }    

        return lineRendererPoints;
    }



    public void ShowTragectoryLine(Vector3 startPoint, Vector3 startVelocity)
    {
        float timeStep = timeOfFlight/lineSegments; //more points = smoother line

        Vector3[] lineRedererPoints = CalcTragectoryLine(startPoint, startVelocity, timeStep);

        lineRenderer.positionCount = lineSegments;
        lineRenderer.SetPositions(lineRedererPoints);


    }




}
