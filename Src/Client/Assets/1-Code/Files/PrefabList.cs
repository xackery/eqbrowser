using UnityEngine;
using System.Collections;

public class PrefabList : MonoBehaviour 
{

    #region Singleton
    protected static PrefabList m_singleton = null;
    public static PrefabList Singleton
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

    public enum CharacterNum
    {
        Barb_F,
        Barb_M,
        DarkElf_F,
        DarkElf_M,
        Dwarf_F,
        Dwarf_M,
        Elf_F,
        Elf_M,
        Erud_F,
        Erud_M,
        Gnome_F,
        Gnome_M,
        HalfElf_F,
        HalfElf_M,
        Halfling_F,
        Halfling_M,
        HighElf_F,
        Highelf_M,
        Human_F,
        Human_M,
        Ogre_F,
        Ogre_M,
        Troll_F,
        Troll_M,
        //..
        MAX
    }

    public GameObject[] CharacterPrefabs;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
