using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.Mathematics;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class PlayerController : NetworkBehaviour
{


    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float maxSpeed = 15f; // = GetSingleton<Ship>().MaxSpeed().
    [SerializeField] float rotationSpeed = 200f;

    [SerializeField] Texture2D cursorTexture;

    private CameraBehaviour m_camera;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        m_camera = Camera.main.GetComponent<CameraBehaviour>();
        if (m_camera != null)
        {
            m_camera.Target = transform;
        }
    }

    private Vector3 m_direction;
    private bool m_rotating;
    private Quaternion m_targetRotation;
    private void Update()
    {
        if (!this.isLocalPlayer) return;
        var axis = Input.GetAxis("Vertical");
        moveSpeed = (axis > 0f ? math.min(moveSpeed+axis, maxSpeed) : math.max(0f, moveSpeed+axis));

        // Move
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);


        if (Input.GetMouseButtonDown(0)) // TODO
        {
            StartRotationTowardsMouse();
        }

        if (m_rotating)
        {
            RotateTowardsTarget();
        }

        Camera.main.orthographicSize += Input.mouseScrollDelta.y * Time.deltaTime * m_camera.ScrollSpeed * -1;
        Camera.main.orthographicSize = Camera.main.orthographicSize < 0f? 0f : math.min(Camera.main.orthographicSize, m_camera.MaxOrthographicSize);
        
    }

    void StartRotationTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;    
        Vector3 directionToMouse = mousePosition - transform.position;

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg - 90f;

        m_targetRotation = Quaternion.Euler(0, 0, angle);

        m_rotating = true;
    }

    void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, m_targetRotation) < 0.05f)
        {
            transform.rotation = m_targetRotation;
            m_rotating = false;
        }
    }

}
