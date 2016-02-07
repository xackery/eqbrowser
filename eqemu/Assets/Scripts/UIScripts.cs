using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EQBrowser;

namespace CanvasUI
{
public class UIScripts : MonoBehaviour {

	public Text MainText;
	public Text AbilitiesText;
	public Text CombatText;
	public Text SocialText;
	public Text InventoryText;

	public GameObject Main;
	public GameObject Abilities;
	public GameObject Combat;
	public GameObject @Social;
	public GameObject MainPanel;
	public GameObject AbilitiesPanel;
	public GameObject CombatPanel;
	public GameObject SocialPanel;

	public GameObject Inventory;
	public GameObject InventoryWindow;
	public GameObject SpellGem1;
	public GameObject SpellGem2;
	public GameObject SpellGem3;
	public GameObject SpellGem4;
	public GameObject SpellGem5;
	public GameObject SpellGem6;
	public GameObject SpellGem7;
	public GameObject SpellGem8;

	public static bool CastButton = false;
	
	public void setupBtn()
	{
		string param = "clack";
		MainPanel.SetActive (true);	
		AbilitiesPanel.SetActive (false);
		CombatPanel.SetActive (false);
		SocialPanel.SetActive (false);

		Main.GetComponent<Button>().onClick.AddListener(delegate { MainClick(param); });
		Abilities.GetComponent<Button>().onClick.AddListener(delegate { AbilitiesClick(param); });
		Combat.GetComponent<Button>().onClick.AddListener(delegate { CombatClick(param); });
		@Social.GetComponent<Button>().onClick.AddListener(delegate { SocialClick (param); });

		Inventory.GetComponent<Button>().onClick.AddListener(delegate { InventoryClick (param); });
		SpellGem1.GetComponent<Button>().onClick.AddListener(delegate { SpellGem1Click (param); });
		SpellGem2.GetComponent<Button>().onClick.AddListener(delegate { SpellGem2Click (param); });
		SpellGem3.GetComponent<Button>().onClick.AddListener(delegate { SpellGem3Click (param); });
		SpellGem4.GetComponent<Button>().onClick.AddListener(delegate { SpellGem4Click (param); });
		SpellGem5.GetComponent<Button>().onClick.AddListener(delegate { SpellGem5Click (param); });
		SpellGem6.GetComponent<Button>().onClick.AddListener(delegate { SpellGem6Click (param); });
		SpellGem7.GetComponent<Button>().onClick.AddListener(delegate { SpellGem7Click (param); });
		SpellGem8.GetComponent<Button>().onClick.AddListener(delegate { SpellGem8Click (param); });

	}
	public void SpellGem1Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem2Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem3Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem4Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem5Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem6Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem7Click(string param)
	{
		UIScripts.CastButton = true;
	}
	public void SpellGem8Click(string param)
	{
		UIScripts.CastButton = true;
	}

	public void InventoryClick(string param)
	{
		if (InventoryWindow.activeSelf == false) 
		{
			InventoryWindow.SetActive (true);
			InventoryText.color = Color.green;
		} 
		else 
		{
			InventoryWindow.SetActive (false);
			InventoryText.color = Color.white;
		}
	}
	public void InventoryClick2(string param)
	{
		InventoryWindow.SetActive (false);	
	}
	public void MainClick(string param)
	{
		MainPanel.SetActive (true);	
		AbilitiesPanel.SetActive (false);
		CombatPanel.SetActive (false);
		SocialPanel.SetActive (false);

		MainText.color = Color.green;
		AbilitiesText.color = Color.black;
		CombatText.color = Color.black;
		SocialText.color = Color.black;
		
	}
	public void AbilitiesClick(string param)
	{
		MainPanel.SetActive (false);	
		AbilitiesPanel.SetActive (true);
		CombatPanel.SetActive (false);
		SocialPanel.SetActive (false);

		MainText.color = Color.black;
		AbilitiesText.color = Color.green;
		CombatText.color = Color.black;
		SocialText.color = Color.black;
		
	}
	public void CombatClick(string param)
	{
		MainPanel.SetActive (false);	
		AbilitiesPanel.SetActive (false);
		CombatPanel.SetActive (true);
		SocialPanel.SetActive (false);

		MainText.color = Color.black;
		AbilitiesText.color = Color.black;
		CombatText.color = Color.green;
		SocialText.color = Color.black;
		
	}
	public void SocialClick(string param)
	{
		MainPanel.SetActive (false);	
		AbilitiesPanel.SetActive (false);
		CombatPanel.SetActive (false);
		SocialPanel.SetActive (true);

		MainText.color = Color.black;
		AbilitiesText.color = Color.black;
		CombatText.color = Color.black;
		SocialText.color = Color.green;
	}
	// Use this for initialization
	void Start () {
		setupBtn ();
		MainText.color = Color.green;
		InventoryWindow.SetActive (false);	

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
}