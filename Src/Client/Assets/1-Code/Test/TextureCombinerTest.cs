/* TextureCombinerTest - Test case for combining the source textures into one unified texture to help cut down drawcalls
 * Created - March 11 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using EQBrowser;

namespace EQBrowser
{
    public class TextureCombinerTest : MonoBehaviour
    {
        public Texture2D testOutPut;
        public GameObject testSubject;

        void OnGUI()
        {
            //if (GUI.Button(new Rect(0, 0, 100, 30), "Test"))
            //{
            //    if (testSubject == null)
            //    {
            //        Debug.LogError("Test subject is null, not proceeding");
            //        return;
            //    }

            //    testOutPut = EQBrowser.TextureCreator.CreateTexture(testSubject);
            //}

            if (GUI.Button(new Rect(10, 10, 100, 30), "Anim"))
            {
                Animation anim = testSubject.GetComponentInChildren<Animation>();

                anim.Play("Jump");
            }
        }
    }
}