using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;   
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
