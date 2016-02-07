using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			
			if (instance == null)
			{
				instance = new GameManager();
			}
			
			return instance;
		}
	}
	
	
	public void Awake() {
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}



	//Our array of GameObjects in the scene currently
	public GameObject BaseNPC;
	public GameObject[] NPCs;

	public void CreateInitialEntities(int count)
	{
		NPCs = new GameObject[count];
		for (int i = 0; i < count; i++)
		{
			GameObject go = Instantiate(BaseNPC, new Vector3((float)i, 1, 0), Quaternion.identity) as GameObject;
			go.transform.localScale = Vector3.one;
			NPCs[i] = go;
		}
	}

}

