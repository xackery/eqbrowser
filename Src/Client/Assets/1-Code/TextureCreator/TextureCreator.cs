/* TextureCreator - Combines all the source bmps into one texture sheet to lessen draw calls. Creates the textures in real time for the various characters
 * Created - March 11 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using EQBrowser;

namespace EQBrowser
{
    public class TextureCreator
    {
        public static Texture2D CreateTexture(GameObject template)
        {
            Texture2D newTexture = new Texture2D(256, 256, TextureFormat.ARGB32, true);

            EQBrowser.AvatarTextureCreatorData avatarTextureData = template.GetComponentInChildren<EQBrowser.AvatarTextureCreatorData>();

            if (avatarTextureData == null)
            {
                Debug.LogError(string.Format("Cannot create texture for {0}, returning null", template.name));
                return null;
            }

            //Combine the textures in predefined positions
            for(int i = 0; i < avatarTextureData.SourceTextures.Length; i++)
            {
                if(avatarTextureData.SourceTextures[i] == null)
                    continue;

                Color[] colors = avatarTextureData.SourceTextures[i].GetPixels(0,0, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height);

                //Remember Y is inverted. 0,0 is bottom left corner of texture. See "textureatlasReference.psd"
                //TODO VELIOUS IS LARGER TEXTURES
                switch(i)
                {
                    case 0: //Chest01
                        newTexture.SetPixels(0, 128, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 1: //Chest02
                        newTexture.SetPixels(64, 240, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 2: //ForeArm01
                        newTexture.SetPixels(64, 176, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 3: //Foot01
                        newTexture.SetPixels(192, 160, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 4: //Foot02
                        newTexture.SetPixels(0, 64, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 5: //Head01
                        newTexture.SetPixels(0, 0, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 6: //Head02
                        newTexture.SetPixels(192, 64, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 7: //Head03
                        newTexture.SetPixels(128, 0, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 8: //Hand01
                        newTexture.SetPixels(64, 144, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 9: //Hand02
                        newTexture.SetPixels(64, 112, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 10: //Leg01
                        newTexture.SetPixels(128, 192, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 11: //Leg02
                        newTexture.SetPixels(128, 128, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 12: //Leg03
                        newTexture.SetPixels(128, 96, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                    case 13: //UpperArm01
                        newTexture.SetPixels(96, 192, avatarTextureData.SourceTextures[i].width, avatarTextureData.SourceTextures[i].height, colors);
                        break;
                }

                newTexture.Apply();
            }

            return newTexture;
        }
    }
}