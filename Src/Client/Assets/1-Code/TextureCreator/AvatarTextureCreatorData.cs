/* AvatarTextureCreatorData - Stores the various relevant data for each character to be used by Texture Creator
 * Created - March 11 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using EQBrowser;

namespace EQBrowser
{
    public class AvatarTextureCreatorData : MonoBehaviour
    {
        public enum TextureSlot
        {
            Chest01,
            Chest02,
            ForeArm01,
            Foot01,
            Foot02,
            Head01, //Face
            Head02, //Hair
            Head03, //Mostly ears for elves
            Hand01,
            Hand02,
            Leg01,
            Leg02,
            Leg03,
            UpperArm01
        }

        public Texture2D CurBodyTexture;

        public Texture2D[] SourceTextures;

        //public Texture2D Chest01;
        //public Texture2D Chest02;
        //public Texture2D ForeArm01;
        //public Texture2D Foot01;
        //public Texture2D Foot02;
        //public Texture2D Head01;
        //public Texture2D Head02;
        //public Texture2D Hand01;
        //public Texture2D Hand02;
        //public Texture2D Leg01;
        //public Texture2D Leg02;
        //public Texture2D Leg03;
        //public Texture2D UpperArm01;

        //Most will use 0,0,1,1 but some, like erudite female, won't due to texture structure
        public Rect[] SourceTextureRects = new Rect[14];

        public TextureSlot[] SourceMaterialOrder;

        //[SerializeField]
        // Rect src_chest01_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_chest02_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_foreArm01_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_foot01_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_foot02_rect;
        //[SerializeField]
        //Rect src_head01_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_head02_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_hand01_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_hand02_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_leg01_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_leg02_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_leg03_rect = new Rect(0, 0, 1f, 1f);
        //[SerializeField]
        //Rect src_upperArm01_rect = new Rect(0, 0, 1f, 1f);

        //public Rect Chest01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Chest02_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect ForeArm01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Foot01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Foot02_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Head01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Head02_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Hand01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Hand02_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Leg01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Leg02_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect Leg03_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        //public Rect UpperArm01_Src_Rect
        //{
        //    get { return src_chest01_rect; }
        //}

        void Start()
        {
            //UpdateBodyTexture();
        }

        public void UpdateBodyTexture()
        {
            CurBodyTexture = TextureCreator.CreateTexture(this.gameObject);
        }
    }
}