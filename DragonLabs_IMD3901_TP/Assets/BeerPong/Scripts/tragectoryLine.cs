using UnityEngine;

public class tragectoryLine : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField, Min(3)] int lineSegments = 175;

    [SerializeField]float timeOfFlight = 5.0f;//length of line

    public float timeIntervalPoints = 0.01f;

    public void drawTragectory(Vector3 startVelocity, bool enableLine)
    {
        if (enableLine)
        {
            Vector3 origin = transform.position;//current object pos which is the camera
            lineRenderer.GetComponent<LineRenderer>().enabled = enableLine;

            lineRenderer.positionCount = lineSegments;
            lineRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);//stay rotated 90 degrees

            // line curve equation
            float timeOffset = timeOfFlight/lineSegments;
            for (int i = 0; i < lineSegments; i++)
            {
                float time = timeOffset * i;
                var x = (startVelocity.x * time) + (Physics.gravity.x/2 * time * time);
                var y = (startVelocity.y * time) + (Physics.gravity.y/2 * time * time);
                var z = (startVelocity.z * time) + (Physics.gravity.z/2 * time * time);

                Vector3 point = new Vector3(x, y, z);
                lineRenderer.SetPosition(i, origin + point);//move line to be infront of the camera at all times
                time+= timeIntervalPoints;

            }

        }
        else if (!enableLine)
        {
            //Debug.Log("STOP");
            lineRenderer.GetComponent<LineRenderer>().enabled = enableLine;//hide the line
        }


    }
}
