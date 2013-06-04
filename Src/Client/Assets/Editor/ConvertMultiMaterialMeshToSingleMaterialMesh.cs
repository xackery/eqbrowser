/* ConvertMultiMaterialMeshToSingleMaterialMesh - Unity Editor Script to convert one of EQ's multi materialed meshes into a single one for drawcall reduction
 * Should be used on each model in editor to create prefabs, not used at runtime
 * Created - March 11 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using EQBrowser;

public class ConvertMultiMaterialMeshToSingleMaterialMesh : EditorWindow
{
    //Window stuff
    [ExecuteInEditMode]
    [MenuItem("Window/Model Converter")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<ConvertMultiMaterialMeshToSingleMaterialMesh>().Show();
    }

    GameObject g_sourceModel;
    Object g_prefabDir;
    Object g_combinedTextureDir;
    Object g_combinedMaterialDir;

    void OnGUI()
    {
        //Here we try to match textures with the slots
        GUILayout.Label("Model Converter", EditorStyles.boldLabel);
        g_sourceModel = (GameObject)EditorGUILayout.ObjectField("Source Model", g_sourceModel, typeof(GameObject), true);
        g_prefabDir = (Object)EditorGUILayout.ObjectField("Prefab Dir", g_prefabDir, typeof(Object), true);
        g_combinedTextureDir = (Object)EditorGUILayout.ObjectField("Combined Texture Dir", g_combinedTextureDir, typeof(Object), true);
        g_combinedMaterialDir = (Object)EditorGUILayout.ObjectField("Combined Material Dir", g_combinedMaterialDir, typeof(Object), true);

        if (g_sourceModel != null && g_prefabDir != null && g_combinedTextureDir != null && g_combinedMaterialDir != null)
        {
            if (GUILayout.Button("Update"))
            {
                ConvertModel(g_sourceModel);
            }
        }
    }

    //Convert the model to use just one mesh, one material. This will alter the original imported FBX, so make sure you have a backup
    void ConvertModel(GameObject sourceModel)
    {
        //GameObject clone = (GameObject)Object.Instantiate(sourceModel);
        //clone.name = sourceModel.name;

        AvatarTextureCreatorData avData = sourceModel.GetComponentInChildren<AvatarTextureCreatorData>();

        if(avData == null)
        {
            Debug.LogError(string.Format("Missing AvatarTextureCreatorData from {0}, can't continue...", sourceModel.name));
            return;
        }

        //Vector3 pos = sourceModel.transform.position;
        //pos.x += 5f;

        //clone.transform.position = pos;

        #region Unused at this time
        ////////////////////////////////////SkinnedMeshRenderer origSkinnedMesh = sourceModel.GetComponentInChildren<SkinnedMeshRenderer>();
        ////////////////////////////////////SkinnedMeshRenderer newSkinnedMesh = clone.GetComponentInChildren<SkinnedMeshRenderer>();

        //////////////////////////////////////Debug.Log("1");

        ////////////////////////////////////if(origSkinnedMesh == null)
        ////////////////////////////////////{
        ////////////////////////////////////    Debug.LogError(string.Format("Missing SkinnedMeshRenderer on original {0}, can't continue...", sourceModel.name));
        ////////////////////////////////////    return;
        ////////////////////////////////////}

        ////////////////////////////////////if(newSkinnedMesh == null)
        ////////////////////////////////////{
        ////////////////////////////////////    Debug.LogError(string.Format("Missing SkinnedMeshRenderer on clone {0}, can't continue...", clone.name));
        ////////////////////////////////////    return;
        ////////////////////////////////////}

        ////////////////////////////////////Debug.Log(string.Format("Orig: {0} Triangle Count: {1} UV Count: {2} Vert Count: {3} Sub Mesh Count: {4} Bone Weight Count: {5} Bind Pose Count: {6} ", sourceModel.name, origSkinnedMesh.sharedMesh.triangles.Length,
        ////////////////////////////////////    origSkinnedMesh.sharedMesh.uv.Length, origSkinnedMesh.sharedMesh.vertexCount, origSkinnedMesh.sharedMesh.subMeshCount, origSkinnedMesh.sharedMesh.boneWeights.Length,
        ////////////////////////////////////    origSkinnedMesh.sharedMesh.bindposes.Length));

        ////////////////////////////////////Vector2[] backup = (Vector2[])origSkinnedMesh.sharedMesh.uv.Clone();

        //////////////////////////////////////for (int i = 0; i < origSkinnedMesh.sharedMesh.uv.Length; i++)
        //////////////////////////////////////{
        //////////////////////////////////////    Debug.Log("orig # " + i + " uv " + origSkinnedMesh.sharedMesh.uv[i]);
        //////////////////////////////////////}

        ////////////////////////////////////Mesh newMesh = newSkinnedMesh.sharedMesh;
        ////////////////////////////////////Vector3[] newMeshVerts = new Vector3[origSkinnedMesh.sharedMesh.vertexCount];
        ////////////////////////////////////Vector2[] newMeshUVs = new Vector2[newMeshVerts.Length];
        ////////////////////////////////////int[] newMeshTriangles = (int[])origSkinnedMesh.sharedMesh.triangles.Clone();
        ////////////////////////////////////Matrix4x4[] newBindPoses = (Matrix4x4[])origSkinnedMesh.sharedMesh.bindposes.Clone();
        ////////////////////////////////////BoneWeight[] newBoneWeights = (BoneWeight[])origSkinnedMesh.sharedMesh.boneWeights.Clone();
        ////////////////////////////////////Color[] newColors = new Color[newMeshVerts.Length];
        ////////////////////////////////////Vector3[] newNormals = (Vector3[])origSkinnedMesh.sharedMesh.normals.Clone();

        //////////////////////////////////////Debug.Log("2");

        //////////////////////////////////////Test to print out submesh and verts
        //////////////////////////////////////for (int i = 0; i < origSkinnedMesh.sharedMesh.subMeshCount; i++)
        //////////////////////////////////////{
        //////////////////////////////////////    int[] triangleList = origSkinnedMesh.sharedMesh.GetTriangles(i);

        //////////////////////////////////////    Debug.Log(string.Format("======== MESH {0} ========", i));

        //////////////////////////////////////    for (int j = 0; j < triangleList.Length; j++)
        //////////////////////////////////////    {
        //////////////////////////////////////        Debug.Log(string.Format("#{0} vert: {1}", j, triangleList[j]));
        //////////////////////////////////////    }
        //////////////////////////////////////}

        //////////////////////////////////////First clone orig mesh
        ////////////////////////////////////newMeshVerts = (Vector3[])origSkinnedMesh.sharedMesh.vertices.Clone();

        ////////////////////////////////////for (int i = 0; i < newColors.Length; i++)
        ////////////////////////////////////{
        ////////////////////////////////////    newColors[i] = Color.white;
        ////////////////////////////////////}

        //////////////////////////////////////Debug.Log("3");

        //////////////////////////////////////Now update UVs
        ////////////////////////////////////for (int i = 0; i < origSkinnedMesh.sharedMesh.subMeshCount; i++)
        ////////////////////////////////////{
        ////////////////////////////////////    int[] triangleList = origSkinnedMesh.sharedMesh.GetTriangles(i);

        ////////////////////////////////////    for (int j = 0; j < triangleList.Length; j++)
        ////////////////////////////////////    {
        ////////////////////////////////////        newMeshUVs[triangleList[j]] = ConvertUVToTextureAtlasUV(origSkinnedMesh.sharedMesh.uv[triangleList[j]], avData.SourceMaterialOrder[i], avData, triangleList[j]);
        ////////////////////////////////////    }
        ////////////////////////////////////}


        //////////////////////////////////////Debug.Log("4");

        ////////////////////////////////////newMesh.Clear();
        ////////////////////////////////////newMesh.bindposes = newBindPoses;
        ////////////////////////////////////newMesh.name = "CombinedMesh";
        ////////////////////////////////////newMesh.subMeshCount = 0;
        ////////////////////////////////////newMesh.vertices = newMeshVerts;
        ////////////////////////////////////newMesh.boneWeights = newBoneWeights;
        ////////////////////////////////////newMesh.uv = newMeshUVs;
        ////////////////////////////////////newMesh.uv1 = newMeshUVs;
        ////////////////////////////////////newMesh.uv2 = newMeshUVs;
        ////////////////////////////////////newMesh.colors = newColors;
        ////////////////////////////////////newMesh.triangles = newMeshTriangles;
        ////////////////////////////////////newMesh.normals = newNormals;
        //////////////////////////////////////newMesh.SetTriangles(newMeshTriangles, 0);
        ////////////////////////////////////newMesh.RecalculateBounds();
        
        //////////////////////////////////////Debug.Log("5");

        ////////////////////////////////////newSkinnedMesh.sharedMaterial = null;
        ////////////////////////////////////newSkinnedMesh.sharedMaterials = new Material[1];
        ////////////////////////////////////newSkinnedMesh.sharedMesh = newMesh;
        ////////////////////////////////////newSkinnedMesh.sharedMesh.Optimize();
        //////////////////////////////////////newSkinnedMesh.sharedMesh.subMeshCount = 1;
        //////////////////////////////////////newSkinnedMesh.sharedMesh.SetTriangles(newSkinnedMesh.sharedMesh.triangles, 1);

        #endregion
        //Now generate the singular default texture, and then material
        Texture combinedTex = GenerateCombinedTexture(sourceModel, g_combinedTextureDir);
        Material combinedMat = GenerateCombinedMaterial(sourceModel, g_combinedMaterialDir, combinedTex);

        #region Unused at this time
        //////////////////////////////newSkinnedMesh.material = combinedMat;
        
        //////////////////////////////MakePrefab(clone, g_prefabDir);

        ////////////////////////////////Debug.Log("6");

        ////////////////////////////////for (int i = 0; i < copiedMesh.vertexCount; i++)
        ////////////////////////////////{
        ////////////////////////////////    Debug.Log(string.Format("# {0} uv: {1} submeshcount: {2} materials: {3}", i, copiedMesh.uv[i], copiedMesh.subMeshCount, skinnedMesh.sharedMaterials.Length));
        ////////////////////////////////}

        //////////////////////////////Debug.Log(string.Format("Clone: {0} Triangle Count: {1} UV Count: {2} Vert Count: {3} Sub Mesh Count: {4} Bone Weight count: {5} Bind Pose Count: {6}", clone.name, newSkinnedMesh.sharedMesh.triangles.Length,
        //////////////////////////////    newSkinnedMesh.sharedMesh.uv.Length, newSkinnedMesh.sharedMesh.vertexCount, newSkinnedMesh.sharedMesh.subMeshCount, newSkinnedMesh.sharedMesh.boneWeights.Length,
        //////////////////////////////    newSkinnedMesh.sharedMesh.bindposes.Length));

        ////////////////////////////////for (int i = 0; i < newSkinnedMesh.sharedMesh.uv.Length; i++)
        ////////////////////////////////{
        ////////////////////////////////    Debug.Log("# " + i + " new uv " + newSkinnedMesh.sharedMesh.uv[i] + " orig " + backup[i]);
        ////////////////////////////////}

        //////////////////////////////newMeshVerts = null;
        //////////////////////////////newMeshUVs = null;
        //////////////////////////////newMeshTriangles = null;
        //////////////////////////////newBindPoses = null;
        //////////////////////////////newBoneWeights = null;
        //////////////////////////////newColors = null;
        //////////////////////////////newNormals = null;
        #endregion
    }

    Material GenerateCombinedMaterial(GameObject obj, Object materialSaveDir, Texture combinedTexToUse)
    {
        Material newMat = new Material(Shader.Find("Diffuse"));
        newMat.SetTexture("_MainTex", combinedTexToUse);

        AssetDatabase.CreateAsset(newMat, AssetDatabase.GetAssetPath(materialSaveDir) + "/" + obj.name + ".mat");

        return newMat;
    }

    Texture GenerateCombinedTexture(GameObject obj, Object textureSavedir)
    {
        Texture2D newTexture = EQBrowser.TextureCreator.CreateTexture(obj);

        byte[] textBytes = newTexture.EncodeToPNG();
        DestroyImmediate(newTexture);

        //Write to file
        File.WriteAllBytes(AssetDatabase.GetAssetPath(textureSavedir) + "/" + obj.name + ".png", textBytes);

        string texAssetFolderPath = AssetDatabase.GetAssetPath(textureSavedir);
        string dataPath = Application.dataPath;

        string texFolderPath = dataPath.Substring(0, dataPath.Length - 6) + texAssetFolderPath;

        //Get all the different texture file types to make this more future proof
        string[] texture = Directory.GetFiles(texFolderPath, string.Format("{0}.png", obj.name), SearchOption.AllDirectories);

        if (texture.Length < 1)
        {
            Debug.LogError("Could not find the correct texture, returning null");
            return null;
        }

        if (texture.Length > 1)
        {
            Debug.LogWarning(string.Format("Found more than one texture ({0}). Picking the first, please double check prefab {1}", texture.Length, obj.name));
        }

        string texName = texture[0].Substring(dataPath.Length - 6);
        Texture tex = (Texture)AssetDatabase.LoadMainAssetAtPath(texName);

        return tex;
    }

    //Convert the original uv for multiple submeshes and materials into one uv set using the single texture atlas
    Vector2 ConvertUVToTextureAtlasUV(Vector2 origUV, AvatarTextureCreatorData.TextureSlot materialSlot, AvatarTextureCreatorData avData, int index)
    {
        Vector2 newUV = origUV;

        //Since some of the UVs are outside 0-1 range, we're going to trim them to make it easier to convert
        int integer = (int)(origUV.x - 0.001f);
        float remainder = origUV.x - integer;
        newUV.x = remainder;

        if (materialSlot == AvatarTextureCreatorData.TextureSlot.Chest01)
        {
            Debug.Log("index " + index + " x orig UV " + origUV + " int " + integer + " remainder " + remainder);
        }

        integer = 1;//(int)(origUV.y - 0.001f);
        remainder = origUV.y - integer;
        newUV.y = remainder;

        if (materialSlot == AvatarTextureCreatorData.TextureSlot.Chest01)
        {
            Debug.Log("index " + index + " y orig UV " + origUV + " int " + integer + " remainder " + remainder);
            Debug.Log("NEW UV " + newUV);
        }
        

        //Now we want to convert that to the pixel position (warning, we might lose some accuracy here)
        int pixelX = Mathf.RoundToInt(newUV.x * avData.SourceTextures[(int)materialSlot].width);
        int pixelY = Mathf.RoundToInt(newUV.y * avData.SourceTextures[(int)materialSlot].height);

        //Now we can offset the pixelX & Y by the new bottom left position in the larger atlas
        //These are hardcoded based on the template texture atlas
        //TODO make this completely hardcoded free (perhaps figure out original texture size and new texture atlas size?)
        switch (materialSlot)
        {
            case AvatarTextureCreatorData.TextureSlot.Chest01:
                //newUV += new Vector2(0 / 256f, 128f / 256f);
                Debug.Log("chest 01 index " + index + " " + pixelX + " , " + pixelY + " source width " + avData.SourceTextures[(int)materialSlot].width
                    + " source height " + avData.SourceTextures[(int)materialSlot].height);
                pixelX += 0;
                pixelY += 128;                
                break;
            case AvatarTextureCreatorData.TextureSlot.Chest02:
                //newUV += new Vector2(64f / 256f, 240f / 256f);
                pixelX += 64;
                pixelY += 240;
                break;
            case AvatarTextureCreatorData.TextureSlot.ForeArm01:
                //newUV += new Vector2(64f / 256f, 176f / 256f);
                pixelX += 64;
                pixelY += 176;
                break;
            case AvatarTextureCreatorData.TextureSlot.Foot01:
                //newUV += new Vector2(192f / 256f, 160f / 256f);
                pixelX += 192;
                pixelY += 160;
                break;
            case AvatarTextureCreatorData.TextureSlot.Foot02:
                //newUV += new Vector2(0f / 256f, 64f / 256f);
                pixelX += 0;
                pixelY += 64;
                break;
            case AvatarTextureCreatorData.TextureSlot.Head01:
                //newUV += new Vector2(0f / 256f, 0f / 256f);
                pixelX += 0;
                pixelY += 0;
                break;
            case AvatarTextureCreatorData.TextureSlot.Head02:
                //newUV += new Vector2(192f / 256f, 64f / 256f);
                pixelX += 192;
                pixelY += 64;
                break;
            case AvatarTextureCreatorData.TextureSlot.Head03:
                //newUV += new Vector2(128f / 256f, 0f / 256f);
                pixelX += 128;
                pixelY += 0;
                break;
            case AvatarTextureCreatorData.TextureSlot.Hand01:
                //newUV += new Vector2(64f / 256f, 144f / 256f);
                pixelX += 64;
                pixelY += 144;
                break;
            case AvatarTextureCreatorData.TextureSlot.Hand02:
                //newUV += new Vector2(64f / 256f, 112f / 256f);
                pixelX += 64;
                pixelY += 112;
                break;
            case AvatarTextureCreatorData.TextureSlot.Leg01:
                //newUV += new Vector2(128f / 256f, 192f / 256f);
                pixelX += 128;
                pixelY += 192;
                break;
            case AvatarTextureCreatorData.TextureSlot.Leg02:
                //newUV += new Vector2(128f / 256f, 128f / 256f);
                pixelX += 128;
                pixelY += 128;
                break;
            case AvatarTextureCreatorData.TextureSlot.Leg03:
                //newUV += new Vector2(128f / 256f, 96f / 256f);
                pixelX += 128;
                pixelY += 96;
                break;
            case AvatarTextureCreatorData.TextureSlot.UpperArm01:
                //newUV += new Vector2(196f / 256f, 192f / 256f);
                pixelX += 196;
                pixelY += 192;
                break;
        }

        //Remember: bottom left corner as 0,0, top right is 1,1
        //Now we can convert back to relative UV float
        newUV.x = (float)pixelX / 256f;
        newUV.y = (float)pixelY / 256f;

        return newUV;
    }

    //Object CreateEmptyPrefab(string name, Object prefabDir)
    //{
    //    return PrefabUtility.CreateEmptyPrefab(AssetDatabase.GetAssetPath(prefabDir) + "/" + name + ".prefab");
    //}
    //void CreateNewPrefab(GameObject obj, string name, Object prefabDir)
    //{
    //    Object newPrefab = PrefabUtility.CreateEmptyPrefab(AssetDatabase.GetAssetPath(prefabDir) + "/" + name + ".prefab");
    //    PrefabUtility.ReplacePrefab(obj, newPrefab, ReplacePrefabOptions.ConnectToPrefab);
    //}

    //We're going to take the model and make a prefab of this model in the prefab folder
    void MakePrefab(GameObject model, Object prefabDir)
    {
        bool shouldMakePrefab = true;
        //string prefabName = string.Empty;
        string prefabName = model.name;
        //Now make a folder & prefab in the prefab dir
        //First check the model name for formatting
        //if (model.name.Length < 2)
        //{
            //Debug.LogError("Model name is too short and of unexpected format \"eq_name\". Aborting prefab creation");
            //shouldMakePrefab = false;
        //}

        if (shouldMakePrefab)
        {
            //prefabName = ObjectNames.NicifyVariableName(model.name.Substring(2, model.name.Length - 2));
            //AssetDatabase.CreateFolder(prefabDir.name, prefabName);

            PrefabUtility.CreatePrefab(AssetDatabase.GetAssetPath(prefabDir) + "/" + prefabName + ".prefab", model, ReplacePrefabOptions.ReplaceNameBased);
        }

        //Now print results
        if (shouldMakePrefab)
        {
            Debug.Log(string.Format("Prefab {0} was made in {1}.", prefabName, prefabDir.name));
        }
        else
        {
            Debug.Log(string.Format("DID NOT MAKE PREFAB"));
        }

    }
}