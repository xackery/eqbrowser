/* ThirdPersonCamera - Controls the camera
 * Derived from http://wiki.unity3d.com/index.php?title=MouseOrbitImproved
 * Created - March 24 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EQBrowser;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject m_curCharacterTarget;
    public LocalPlayerController m_playerController;
    //public GameObject m_curHeadTarget;
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 50f;

    float x = 0.0f;
    float y = 0.0f;

    float targetLastRot;

    bool m_isLocked;

    void FindHeadTarget()
    {
        target = MiscHelpers.GetChildTransformByTag(m_curCharacterTarget.transform, "EyesTag");

        //this.camera.transform.LookAt(target);
    }

    void FindPlayerController()
    {
        m_playerController = m_curCharacterTarget.GetComponentInChildren<LocalPlayerController>();
    }

    // Use this for initialization
	void Start () 
    {
        FindHeadTarget();
        FindPlayerController();
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;

        UpdateCameraFree(0f, 0f, 0f);
	}

    public void Init(SceneManager.CameraMode camMode)
    {
        SwitchCameraMode(camMode);
        
        Start();
    }

    public void SwitchCameraMode(SceneManager.CameraMode camMode)
    {
        if (camMode == SceneManager.CameraMode.ThirdPersonLocked)
        {
            m_isLocked = true;
            this.camera.transform.LookAt(target);
            //Confusing, but x refers to the movement of the mouse relative to the screen, and that generally refers to the y rotation of the character
            targetLastRot = m_curCharacterTarget.transform.rotation.eulerAngles.y;

            ForceOrientation();
        }
        else
        {
            m_isLocked = false;
        }
    }

    void Update()
    {
        
    }

    // zer0sum: mouserightdown
    protected static bool m_r_d = false;

    void LateUpdate()
    {
        float xDelta = 0f;
        float yDelta = 0f;
        float zoomDelta = 0f;

        if (Input.GetMouseButton(1))
        {
            if (!m_r_d) // zer0sum
                Screen.lockCursor = m_r_d = true;
            xDelta = Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDelta = Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }
        else
        {
            Screen.lockCursor = m_r_d = false; // zer0sum
        }

        zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        if (m_isLocked)
        {
            UpdateCameraLocked(zoomDelta, xDelta, yDelta);
        }
        else
        {
            UpdateCameraFree(zoomDelta, xDelta, yDelta);
        }
        
    }

    void UpdateCameraLocked(float distDelta, float xDelta, float yDelta)
    {
        //First rotate the player's character
        if (!Mathf.Approximately(xDelta, 0f))
        {
            float sign = Mathf.Sign(xDelta);
            m_playerController.Turn(sign * 55f + (xDelta * 100f), (sign < 0) ? false : true);
        }

        //Now update the camera position
        y -= yDelta;

        y = MiscHelpers.ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, m_curCharacterTarget.transform.rotation.eulerAngles.y, 0f);

        distance = Mathf.Clamp(distance - distDelta * 5, distanceMin, distanceMax);

        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit, 1 << LayerMask.NameToLayer("Terrain")))
        {
            distance -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    void UpdateCameraFree(float distDelta, float xDelta, float yDelta)
    {
        x += xDelta;//Input.GetAxis("Mouse X") * xSpeed /** distance*/ * 0.02f;
        y -= yDelta;//Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        y = MiscHelpers.ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.Clamp(distance - distDelta * 5, distanceMin, distanceMax);

        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit, 1 << LayerMask.NameToLayer("Terrain")))
        {
            distance -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    //Force it to look in the same direction as the character, regardless of previous orientation
    //Could be done more elegantly
    void ForceOrientation()
    {
        Quaternion rotation = Quaternion.Euler(3f, targetLastRot, 0);

        //distance = Mathf.Clamp(distance - distDelta * 5, distanceMin, distanceMax);

        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit, 1 << LayerMask.NameToLayer("Terrain")))
        {
            distance -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;

        x = m_curCharacterTarget.transform.rotation.eulerAngles.y;
        y = m_curCharacterTarget.transform.rotation.eulerAngles.x;
    }
}