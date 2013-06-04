/* FirstPersonCamera - Controls the camera
 * Created - April 14 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EQBrowser;

public class FirstPersonCamera : MonoBehaviour
{
    public GameObject m_curCharacterTarget;
    //public GameObject m_curHeadTarget;
    public Transform target;

    public float ySpeed = 120.0f;
    public float xSpeed = 220.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    float x = 0.0f;
    //float y = 0.0f;

    void FindHeadTarget()
    {
        target = MiscHelpers.GetChildTransformByTag(m_curCharacterTarget.transform, "EyesTag");

        //this.camera.transform.LookAt(target);
    }

    // Use this for initialization
    void Start()
    {
        FindHeadTarget();
        //Vector3 angles = transform.eulerAngles;
        //x = angles.y;
        //y = angles.x;

        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;

        UpdateCamera(0f, 0f);
    }


    // zer0sum: mouserightdown
    protected static bool m_r_d = false;

    void LateUpdate()
    {


/*        //if (Input.GetKeyDown(KeyCode.F9))
        //{
        //    m_firstPersonCamera = !m_firstPersonCamera;

        //    if (m_firstPersonCamera)
        //    {
        //        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //        UpdateCamera(distanceMax, 0f, 0f);
        //    }
        //    else
        //    {
        //        UpdateCamera(-distanceMax, 0f, 0f);
        //    }
        //}
*/
        float xDelta = 0f;
        float yDelta = 0f;

        if (Input.GetMouseButton(1))
        {
            if (!m_r_d) // zer0sum
                Screen.lockCursor = m_r_d = true;
            
            xDelta = Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDelta = Input.GetAxis("Mouse Y") * ySpeed * -0.02f;
        }
        else
        {
            Screen.lockCursor = m_r_d = false; // zer0sum
        }

        UpdateCamera(xDelta, yDelta);
    }

    void UpdateCamera(float xScreenDelta, float yScreenDelta)
    {
        if (target)
        {
            target.transform.root.Rotate(Vector3.up, 45f * xScreenDelta * Time.deltaTime);

            float Yrotation = target.transform.root.rotation.eulerAngles.y;
            Vector3 position = target.position;

            x += yScreenDelta;
            x = MiscHelpers.ClampAngle(x, yMinLimit, yMaxLimit);

            Vector3 rotation = new Vector3(x, Yrotation, 0);

            transform.rotation = Quaternion.Euler(rotation);
            transform.position = position;
        }
    }
}