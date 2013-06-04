/* TexturePrefabMatcher - Unity Editor Script to match textures with slot
 * Created - March 11 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using EQBrowser;

public class TexturePrefabMatcher : EditorWindow
{
    //Window stuff
    [ExecuteInEditMode]
    [MenuItem("Window/Texture Prefab Matcher")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<TexturePrefabMatcher>().Show();
    }

    GameObject m_modelPrefab;
    Object m_texturesDir;
    bool m_overWriteRectsWhenTextureSearching = false;

    void OnGUI()
    {
        //Here we try to match textures with the slots
        GUILayout.Label("Match texture to slot", EditorStyles.boldLabel);
        m_modelPrefab = (GameObject)EditorGUILayout.ObjectField("Model", m_modelPrefab, typeof(Object), true);
        GUILayout.Label("Textures Dir (should be model specific)");
        m_texturesDir = (Object)EditorGUILayout.ObjectField( m_texturesDir, typeof(Object), true);
        m_overWriteRectsWhenTextureSearching = EditorGUILayout.Toggle("Reset UV Rects", m_overWriteRectsWhenTextureSearching);

        if (m_texturesDir != null && m_texturesDir != null)
        {
            if (GUILayout.Button("Match"))
            {
                AttemptMatchTextureToSlot(m_modelPrefab, m_texturesDir, m_overWriteRectsWhenTextureSearching);
            }
        }

        //Here we convert all textures to read and write
        GUILayout.Label("Convert all textures in folder to readable", EditorStyles.boldLabel);
        m_texturesDir = (Object)EditorGUILayout.ObjectField(m_texturesDir, typeof(Object), true);
        
        if (m_texturesDir != null)
        {
            if (GUILayout.Button("Process"))
            {
                MakeAllTexturesInDirReadable(m_texturesDir);
            }
        }
    }

    void MakeAllTexturesInDirReadable(Object texturesDirObj)
    {
        string texAssetFolderPath = AssetDatabase.GetAssetPath(texturesDirObj);

        string dataPath = Application.dataPath;
        string texFolderPath = dataPath.Substring(0, dataPath.Length - 6) + texAssetFolderPath;
        //Get all the different texture file types to make this more future proof
        string[][] allTexFilePathsROUGH = new string[4][];
        allTexFilePathsROUGH[0] = Directory.GetFiles(texFolderPath, "*.bmp", SearchOption.AllDirectories);
        allTexFilePathsROUGH[1] = Directory.GetFiles(texFolderPath, "*.tga", SearchOption.AllDirectories);
        allTexFilePathsROUGH[2] = Directory.GetFiles(texFolderPath, "*.psd", SearchOption.AllDirectories);
        allTexFilePathsROUGH[3] = Directory.GetFiles(texFolderPath, "*.png", SearchOption.AllDirectories);

        //Combine them all into one nice list for easier processing
        string[] allTexFilePaths = new string[allTexFilePathsROUGH[0].Length + allTexFilePathsROUGH[1].Length + allTexFilePathsROUGH[2].Length + allTexFilePathsROUGH[3].Length];

        int index = 0;
        for (int i = 0; i < allTexFilePathsROUGH.Length; i++)
        {
            for (int j = 0; j < allTexFilePathsROUGH[i].Length; j++)
            {
                allTexFilePaths[index] = allTexFilePathsROUGH[i][j];
                index++;
            }
        }

        for (int i = 0; i < allTexFilePaths.Length; i++)
        {
            string texName = allTexFilePaths[i].Substring(dataPath.Length - 6);
            Object tex = AssetDatabase.LoadMainAssetAtPath(texName);

            string path = AssetDatabase.GetAssetPath(tex);
            TextureImporter texImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            //Debug.Log(string.Format("Adjusting texture {0}", path));

            texImporter.textureType = TextureImporterType.Advanced;
            texImporter.isReadable = true;

            AssetDatabase.ImportAsset(path);
        }        
    }

    void AttemptMatchTextureToSlot(GameObject m_modelPrefab, Object texturesDirObj, bool overWriteRects)
    {
        //First find the AvatarTextureCreatorData
        AvatarTextureCreatorData avatarData = m_modelPrefab.GetComponentInChildren<AvatarTextureCreatorData>();

        //If we can't find the component
        if (avatarData == null)
        {
            //Look for the avatar component
            EQBrowser.Avatar avatar = m_modelPrefab.GetComponentInChildren<EQBrowser.Avatar>();

            //If avatar isn't null, then use it's gameobject to add it, otherwise eject with an error
            if (avatar != null)
            {
                avatarData = avatar.gameObject.AddComponent<EQBrowser.AvatarTextureCreatorData>();
            }
            else
            {
                Debug.LogError(string.Format("Can't find an AvatarTextureCreatorData OR Avatar, not finishing on {0}", m_modelPrefab.name));
                return;
            }
        }

        string texAssetFolderPath = AssetDatabase.GetAssetPath(texturesDirObj);

        string dataPath = Application.dataPath;
        string texFolderPath = dataPath.Substring(0, dataPath.Length - 6) + texAssetFolderPath;

        //Create the source textures slots
        avatarData.SourceTextures = new Texture2D[14];
        if (avatarData.SourceTextureRects.Length != avatarData.SourceTextures.Length)
        {
            avatarData.SourceTextureRects = new Rect[avatarData.SourceTextures.Length];
        }

        //string[] allMatFilePaths = Directory.GetFiles(matFolderPath, "*.mat", SearchOption.AllDirectories);
        //Get all the different texture file types to make this more future proof
        string[][] allTexFilePathsROUGH = new string[4][];
        allTexFilePathsROUGH[0] = Directory.GetFiles(texFolderPath, "*.bmp", SearchOption.AllDirectories);
        allTexFilePathsROUGH[1] = Directory.GetFiles(texFolderPath, "*.tga", SearchOption.AllDirectories);
        allTexFilePathsROUGH[2] = Directory.GetFiles(texFolderPath, "*.psd", SearchOption.AllDirectories);
        allTexFilePathsROUGH[3] = Directory.GetFiles(texFolderPath, "*.png", SearchOption.AllDirectories);

        //Combine them all into one nice list for easier processing
        string[] allTexFilePaths = new string[allTexFilePathsROUGH[0].Length + allTexFilePathsROUGH[1].Length + allTexFilePathsROUGH[2].Length + allTexFilePathsROUGH[3].Length];

        int index = 0;
        for (int i = 0; i < allTexFilePathsROUGH.Length; i++)
        {
            for (int j = 0; j < allTexFilePathsROUGH[i].Length; j++)
            {
                allTexFilePaths[index] = allTexFilePathsROUGH[i][j];
                index++;
            }
        }

        if (allTexFilePaths.Length < 1)
        {
            Debug.LogError(string.Format("Can't find any textures! Aborting on model: {0}", m_modelPrefab.name));
            return;
        }

        //Now go through and look for matches
        int count = 0;
        bool foundItem = true;
        System.Collections.Generic.List<string> missingItems = new System.Collections.Generic.List<string>();
        string searchName = "";
        int texCount = 0;
        for (int i = 0; i < avatarData.SourceTextures.Length; i++)
        {
            if(foundItem == false)
            {
                missingItems.Add(searchName);
            }

            //Hopefully get the correct file name
            searchName = ConvertTextureIndexToSearchString(i, allTexFilePaths[0]);
                        
            //Now search through all the textures for that file to assign it to the slot
            for (texCount = 0; texCount < allTexFilePaths.Length; texCount++)
            {

                //We're going to set up the default rects for the uvs here too
                if (avatarData.SourceTextureRects[i] != null && overWriteRects)
                {
                    avatarData.SourceTextureRects[i] = new Rect(0, 0, 1f, 1f);
                }

                string texName = allTexFilePaths[texCount].Substring(dataPath.Length - 6);
                if (searchName.ToLower() == Path.GetFileNameWithoutExtension(texName).ToLower())
                {
                    //We should have the correct texture, so assign it
                    avatarData.SourceTextures[i] = (Texture2D)AssetDatabase.LoadMainAssetAtPath(texName);
                    count++;
                    foundItem = true; 
                    break;
                }
            }

            if (texCount >= allTexFilePaths.Length)
            {
                foundItem = false;
            }
        }

        Debug.Log(string.Format("Finished with model: {0} and found {1} textures. Missing the following:", m_modelPrefab.name, count));
        for (int i = 0; i < missingItems.Count; i++)
        {
            Debug.Log(missingItems[i]);
        }
    }

    //Based on the index, find the (hopefully) correct file name
    string ConvertTextureIndexToSearchString(int index, string firstFileName)
    {
        string firstNameWOExt = Path.GetFileNameWithoutExtension(firstFileName).ToLower();
        //Debug.Log("firstFileName " + firstFileName + " firstNameWOExt " + firstNameWOExt);
        string partialFileName = string.Empty;
        switch (index)
        {
            case 0: //Chest01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "ch", 1);
                break;
            case 1: //Chest02
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "ch", 2);
                break;
            case 2: //ForeArm01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "fa", 1);
                break;
            case 3: //Foot01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "ft", 1);
                break;
            case 4: //Foot02
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "ft", 2);
                break;
            case 5: //Head01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "he", 1);
                break;
            case 6: //Head02
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "he", 2);
                break;
            case 7: //Head03
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "he", 3);
                break;
            case 8: //Hand01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "hn", 1);
                break;
            case 9: //Hand02
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "hn", 2);
                break;
            case 10: //Leg01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "lg", 1);
                break;
            case 11: //Leg02
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "lg", 2);
                break;
            case 12: //Leg03
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "lg", 3);
                break;
            case 13: //UpperArm01
                partialFileName = string.Format("{0}{1}000{2}", firstNameWOExt.Substring(0, 3), "ua", 1);
                break;
        }

        return partialFileName;
    }
}