using Unity.Netcode;
using UnityEngine;


public class tragectoryLine : NetworkBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField, Min(3)] int lineSegments = 175;

    [SerializeField]float timeOfFlight = 5.0f;//length of line

    public float timeIntervalPoints = 0.01f;

    public Transform holdAreaPosition;

    


    //call server RPC to sync positions
    //make enable line in player controller be network bool
    public void drawTragectory(Vector3 startVelocity, NetworkVariable<bool> enableLine)
    {
        if (enableLine.Value == true)
        {
            Vector3 origin = holdAreaPosition.position;//current object pos which is the hold area
            lineRenderer.GetComponent<LineRenderer>().enabled = enableLine.Value;

            lineRenderer.positionCount = lineSegments;

            // line curve equation
            float timeOffset = timeOfFlight/lineSegments;
            for (int i = 0; i < lineSegments; i++)
            {
                float time = timeOffset * i;
                var x = (startVelocity.x * time) + (Physics.gravity.x/2 * time * time);
                var y = (startVelocity.y * time) + (Physics.gravity.y/2 * time * time);
                var z = (startVelocity.z * time) + (Physics.gravity.z/2 * time * time);

                Vector3 point = new Vector3(x, y, z);
                lineRenderer.SetPosition(i, origin + point);//move line to be in hold area at all times
                time+= timeIntervalPoints;
                Debug.Log("move and draw line");

            }

        }
        else if (!enableLine.Value)
        {
            Debug.Log("STOP");
            lineRenderer.GetComponent<LineRenderer>().enabled = enableLine.Value;//hide the line
        }


    }
}
