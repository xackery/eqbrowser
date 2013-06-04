/* SceneManager - Main client scene manager. In charge of keeping track of various elements
 * Created - March 24 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EQBrowser;

public class SceneManager : MonoBehaviour 
{
    #region Singleton
    protected static SceneManager m_singleton = null;
    public static SceneManager Singleton
    {
        get
        {
            return m_singleton;
        }
    }

    void Awake()
    {
        m_singleton = this;
    }
    #endregion

    public FirstPersonCamera FirstPersonCamera;
    public ThirdPersonCamera ThirdPersonCamera;

    public enum CameraMode
    {
        FirstPerson,
        ThirdPersonLocked, // zer0sum: switched order for simplicity
        ThirdPersonFree,
        //..
        MAX
    }

    protected CameraMode m_curCameraMode;

	// Use this for initialization
	void Start () 
    {
        m_curCameraMode = CameraMode.ThirdPersonFree;
        ThirdPersonCamera.Init(m_curCameraMode);
	}

    // zer0sum: first person cam
    private static bool f_p_c = false; // ^starts in 3rd person
    private static float f_p_cZoomDelta;
	
	// Update is called once per frame
	void Update () 
    {
        // zer0sum: will zoom to 3rd person cam if 1st person scrolls out
        if (m_curCameraMode == CameraMode.FirstPerson)
        {
            if (!f_p_c)
            {
                f_p_cZoomDelta = Input.GetAxis("Mouse ScrollWheel");
                f_p_c = true;
            }
            else
            {
                if (f_p_cZoomDelta != Input.GetAxis("Mouse ScrollWheel"))
                {
                    m_curCameraMode = CameraMode.ThirdPersonLocked;
                    ChangeCamera();
                    f_p_c = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            m_curCameraMode++;

            if (m_curCameraMode >= CameraMode.MAX)
            {
                m_curCameraMode = CameraMode.FirstPerson;
            }
            //FirstPersonCamera.gameObject.SetActive(!FirstPersonCamera.gameObject.activeSelf);
            //ThirdPersonCamera.gameObject.SetActive(!ThirdPersonCamera.gameObject.activeSelf);

            ChangeCamera();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            int desktopWidth = Screen.currentResolution.width;
            int desktopHeight = Screen.currentResolution.height;
            bool curState = Screen.fullScreen;

            if (curState == false)
            {
                Screen.SetResolution(desktopWidth, desktopHeight, true);
            }
            else
            {
                Screen.fullScreen = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
        }
	}

    void ChangeCamera()
    {
        switch (m_curCameraMode)
        {
            case CameraMode.FirstPerson:
                FirstPersonCamera.gameObject.SetActive(true);
                ThirdPersonCamera.gameObject.SetActive(false);
                break;
            case CameraMode.ThirdPersonFree:
            case CameraMode.ThirdPersonLocked:
                FirstPersonCamera.gameObject.SetActive(false);
                ThirdPersonCamera.gameObject.SetActive(true);
                ThirdPersonCamera.SwitchCameraMode(m_curCameraMode);
                break;
        }
    }
}
