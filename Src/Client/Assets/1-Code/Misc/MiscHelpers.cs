/* MiscHelpers - Various misc utility functions
 * Created - March 24 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class MiscHelpers
{
    public static Transform GetChildTransformByTag(Transform rootTransform, string tag)
    {
        Transform[] childTrans = rootTransform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < childTrans.Length; i++)
        {
            if (childTrans[i].tag == tag)
            {
                return childTrans[i];
            }
        }

        return null;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
