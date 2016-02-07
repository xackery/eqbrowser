using UnityEngine;
using System.Collections;

public class LOC2UI : MonoBehaviour
{
    public Rect OutputHere = new Rect(0, 0, 500, 50);
    private Vector2 scrollViewVector = Vector2.zero;
    public Rect dropDownRect = new Rect(125, 50, 125, 300);
	public static string[] list = { "Select a Zone", "Char Select", "North Qeynos", "South Qeynos", "Qeynos Catacombs", "Qeynos Hills", "Surefall Glade", "West Karana" };

    int indexNumber;
    bool show = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
//        GUI.Label(OutputHere, string.Format("Server: X({0}) Y({1}) Z({2})\r\nClient: X({3}) Y({2}) Z({1})", -this.transform.position.x, this.transform.position.z, this.transform.position.y, this.transform.position.x));



        if (GUI.Button(new Rect((dropDownRect.x - 50), (dropDownRect.y - 50), dropDownRect.width, 25), ""))
        {
            if (!show)
            {
                show = true;
            }
            else
            {
                show = false;
            }
        }

        if (show)
        {
 //           scrollViewVector = GUI.BeginScrollView(new Rect((dropDownRect.x - 100), (dropDownRect.y + 25), dropDownRect.width, dropDownRect.height), scrollViewVector, new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (list.Length * 25))));
			scrollViewVector = GUI.BeginScrollView(new Rect((dropDownRect.x - 50), (dropDownRect.y - 50), dropDownRect.width, dropDownRect.height), scrollViewVector, new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (list.Length * 25))));
            GUI.Box(new Rect(0, 0, dropDownRect.width, Mathf.Max(dropDownRect.height, (list.Length * 25))), "");

            for (int index = 0; index < list.Length; index++)
            {

                if (GUI.Button(new Rect(0, (index * 25), dropDownRect.height, 25), ""))
                {
                    show = false;
                    indexNumber = index;
                }

                GUI.Label(new Rect(5, (index * 25), dropDownRect.height, 25), list[index]);

            }
            //			print(indexNumber);

            if (indexNumber == 1)
            {
                Application.LoadLevel("1 Character creation");
            }
            if (indexNumber == 2)
            {
                Application.LoadLevel("2 North Qeynos");
            }
            if (indexNumber == 3)
            {
                Application.LoadLevel("3 South Qeynos");
            }
            if (indexNumber == 4)
            {
                Application.LoadLevel("4 Qeynos Catacombs");
            }
            if (indexNumber == 5)
            {
                Application.LoadLevel("5 Qeynos Hills");
            }
            if (indexNumber == 6)
            {
                Application.LoadLevel("6 Surefall Glade");
            }
            if (indexNumber == 7)
            {
                Application.LoadLevel("7 West Karana");
            }

            GUI.EndScrollView();
        }
        else
        {
//            GUI.Label(new Rect((dropDownRect.x - 95), dropDownRect.y, 300, 25), list[indexNumber]);
			GUI.Label(new Rect((dropDownRect.x - 45), (dropDownRect.y - 50), 300, 25), list[indexNumber]);
        }
    }
}