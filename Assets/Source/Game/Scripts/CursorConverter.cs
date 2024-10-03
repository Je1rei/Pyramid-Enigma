using UnityEngine;

public class CursorConverter : MonoBehaviour
{
    private static Vector3 CursorWorldPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane));
        }
    }

    private static Vector3 CameraToCursor
    {
        get
        {
            return CursorWorldPosition - Camera.main.transform.position;
        }
    }

    public Vector3 CursorPositionOnTransform
    {
        get
        {
            Vector3 camToTrans = transform.position - Camera.main.transform.position;

            return Camera.main.transform.position +
                CameraToCursor *
                (Vector3.Dot(Camera.main.transform.forward, camToTrans) / Vector3.Dot(Camera.main.transform.forward, CameraToCursor));
        }
    }
}
