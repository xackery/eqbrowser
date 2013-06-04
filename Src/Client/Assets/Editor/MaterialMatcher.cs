using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class MaterialMatcher : EditorWindow 
{
    //Window stuff
    [ExecuteInEditMode]
    [MenuItem("Window/Material Importer Matcher")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<MaterialMatcher>().Show();
    }

    string curModelFilePath;
    public GameObject test;

    string testStr;
    Object m_selectedObj;
    float m_globalScaleFactor = 1.0f;
    bool m_importMaterials = true;

    Object m_materialsRootDir;
    Object m_texturesRootDir;

    //Material & Model Swap
    GameObject m_targetModel;
    Object m_prefabFolder;

    //ModelImporter

    void OnGUI()
    {
        //Debug.Log(Application.dataPath);
        //GUILayout.Label("Model", EditorStyles.boldLabel);
        //curModelFilePath = EditorGUILayout.TextField("Model Name", curModelFilePath);
        
        //Debug.Log(ObjectNames.GetDragAndDropTitle(Selection.gameObjects[0]));

        GUILayout.Label("Import Model", EditorStyles.boldLabel);
        m_selectedObj = (Object)EditorGUILayout.ObjectField("Model", m_selectedObj, typeof(Object), true);

        //Don't show any options unless we have something selected
        if (m_selectedObj != null)
        {
            m_globalScaleFactor = EditorGUILayout.FloatField("Scale Factor", m_globalScaleFactor);
            m_importMaterials = EditorGUILayout.Toggle("Import Materials", m_importMaterials);

            if (GUILayout.Button("Process"))
            {
                //If individual model, just do it
                if (m_selectedObj.GetType() == typeof(GameObject))
                {
                    ProcessModel(m_selectedObj as GameObject, m_globalScaleFactor, m_importMaterials);
                }
                //Otherwise try to do it and all subfolders
                else if (m_selectedObj.GetType() == typeof(Object))
                {
                    ProcessDirectory(m_selectedObj, m_globalScaleFactor, m_importMaterials);
                }

                #region test stuff
                //ProcessModel(selectedObj);


                //Debug.Log(AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(m_selectedObj)).assetPath);

                //Debug.Log("type " + m_selectedObj.GetType());

                //string assetFolderPath = AssetDatabase.GetAssetPath(m_selectedObj);
                //string dataPath = Application.dataPath;
                //string folderPath = dataPath.Substring(0, dataPath.Length - 6) + assetFolderPath;

                //string[] allFilePaths = Directory.GetFiles(folderPath, "*.fbx");

                //Debug.Log("all file paths " + allFilePaths.Length);

                //for (int i = 0; i < allFilePaths.Length; i++)
                //{
                //    Debug.Log(allFilePaths[i]);
                //}



                //Debug.Log(AssetDatabase.GetAssetPath(selectedObj));
                //Debug.Log(Application.dataPath);

                //Debug.Log(string.Format("{0}/{1}", Application.dataPath, AssetDatabase.GetAssetPath(selectedObj)));
                //Debug.Log("Type " + selectedObj.GetType().ToString() );
                #endregion
            }
        }

        //Here we try to match textures with material names
        GUILayout.Label("Material & Texture Matcher", EditorStyles.boldLabel);
        m_materialsRootDir = (Object)EditorGUILayout.ObjectField("Materials Root Dir", m_materialsRootDir, typeof(Object), true);
        m_texturesRootDir = (Object)EditorGUILayout.ObjectField("Textures Root Dir", m_texturesRootDir, typeof(Object), true);

        if (m_materialsRootDir != null && m_texturesRootDir != null)
        {
            if (GUILayout.Button("Match"))
            {
                AttemptMatchMaterialsAndTextures(m_materialsRootDir, m_texturesRootDir);
            }
        }

        //Here we swap materials based on material name.
        //We need the following - A model with orig materials intact, replacement Materials parent folder, and prefab folder to create the prefab to
        GUILayout.Label("Prefab Maker", EditorStyles.boldLabel);
        m_targetModel = (GameObject)EditorGUILayout.ObjectField("Model", m_targetModel, typeof(GameObject), true);
        m_prefabFolder = (Object)EditorGUILayout.ObjectField("Prefab Folder", m_prefabFolder, typeof(Object), true);

        if (m_targetModel != null && m_prefabFolder != null)
        {
            if (GUILayout.Button("Make Prefab"))
            {
                MakePrefab(m_targetModel, m_prefabFolder);
                //Debug.Log("prefabDir.name " + m_prefabFolder.name + " model.name " + m_targetModel.name);
            }
        }
    }

    //We're going to take the model and make a prefab of this model in the prefab folder
    void MakePrefab(GameObject model, Object prefabDir)
    {
        bool shouldMakePrefab = true;
        string prefabName = string.Empty;
        //Now make a folder & prefab in the prefab dir
        //First check the model name for formatting
        if (model.name.Length < 2)
        {
            Debug.LogError("Model name is too short and of unexpected format \"eq_name\". Aborting prefab creation");
            shouldMakePrefab = false;
        }

        if (shouldMakePrefab)
        {
            prefabName = ObjectNames.NicifyVariableName(model.name.Substring(2, model.name.Length - 2));
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

    
    void AttemptMatchMaterialsAndTextures(Object materialsDirObj, Object texturesDirObj)
    {
        string matAssetFolderPath = AssetDatabase.GetAssetPath(materialsDirObj);
        string texAssetFolderPath = AssetDatabase.GetAssetPath(texturesDirObj);

        string dataPath = Application.dataPath;

        string matFolderPath = dataPath.Substring(0, dataPath.Length - 6) + matAssetFolderPath;
        string texFolderPath = dataPath.Substring(0, dataPath.Length - 6) + texAssetFolderPath;

        string[] allMatFilePaths = Directory.GetFiles(matFolderPath, "*.mat", SearchOption.AllDirectories);
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
            for(int j = 0; j < allTexFilePathsROUGH[i].Length; j++)
            {
                allTexFilePaths[index] = allTexFilePathsROUGH[i][j];
                index++;
            }
        }

        Debug.Log(string.Format("Found {0} materials, {1} textures. Attempting to match ", allMatFilePaths.Length, allTexFilePaths.Length));

        for (int i = 0; i < allMatFilePaths.Length; i++)
        {
            //Debug.Log(allMatFilePaths[i]);
            string matName = allMatFilePaths[i].Substring(dataPath.Length - 6);
            //Begin looking through the textures
            for (int j = 0; j < allTexFilePaths.Length; j++)
            {
                string texName = allTexFilePaths[j].Substring(dataPath.Length - 6);
                Material mat = (Material)AssetDatabase.LoadMainAssetAtPath(matName);

                if (mat.GetTexture("_MainTex") != null)
                    continue;

                //Debug.Log("1: " + Path.GetFileNameWithoutExtension(matName) + " 2: " + Path.GetFileNameWithoutExtension(texName));
                if (Path.GetFileNameWithoutExtension(matName).ToLower() == Path.GetFileNameWithoutExtension(texName).ToLower())
                {
                    Debug.Log(string.Format("Found a match for material: {0}", matName));
                    Texture tex = (Texture)AssetDatabase.LoadMainAssetAtPath(texName);
                    mat.SetTexture("_MainTex", tex);
                    break;
                }
            }
        }
    }

    string StripFileExtension(string fileNameWExt)
    {
        int fileExpPos = fileNameWExt.LastIndexOf(".");

        if (fileExpPos > -1)
        {
            return fileNameWExt.Substring(0, fileExpPos);
        }

        return fileNameWExt;
    }

    //Set the scale, the import materials and the animation type
    void ProcessModel(GameObject go, float scaleFactor, bool importMaterials)
    {
        if (go != null)
        {
            //http://forum.unity3d.com/threads/50016-Editor-Script-How-to-import-animations
            string path = AssetDatabase.GetAssetPath(go);
            ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;

            Debug.Log(string.Format("Adjusting model {0}", path));

            modelImporter.globalScale = scaleFactor;
            modelImporter.importMaterials = importMaterials;
            modelImporter.animationType = ModelImporterAnimationType.Legacy;
            modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;

            AssetDatabase.ImportAsset(path);

            Debug.Log(string.Format("Adjustment Done."));

            //mi = AssetImporter.GetAtPath
        }
        else
        {
            Debug.LogWarning(string.Format("Could not adjust a model as it was null"));
        }
    }

    void ProcessDirectory(Object obj, float scaleFactor, bool importMaterials)
    {
        if (obj != null)
        {
            string assetFolderPath = AssetDatabase.GetAssetPath(obj);
            string dataPath = Application.dataPath;
            string folderPath = dataPath.Substring(0, dataPath.Length - 6) + assetFolderPath;

            string[] allFilePaths = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);

            Debug.Log(string.Format("Found {0} models in directory and subdirectories. Attempting to adjust all", allFilePaths.Length));

            for (int i = 0; i < allFilePaths.Length; i++)
            {
                string assetPath = allFilePaths[i].Substring(dataPath.Length - 6);
                Debug.Log(assetPath);
                //Debug.LogWarning(allFilePaths[i]);
                GameObject objAsset = (GameObject)AssetDatabase.LoadMainAssetAtPath(assetPath);
                ProcessModel(objAsset, scaleFactor, importMaterials);
            }
        }
    }
}
