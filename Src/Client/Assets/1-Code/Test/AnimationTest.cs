/* Smiple animation test to let a player play an animation from GUI element for Nagafen Dragon.
 * Jan 18 2013
 */
using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour 
{
	public Animation modelAnimation;
	public AnimationList animationList;
	
	private int curAnimIndex;
		
	// Use this for initialization
	void Start () 
	{
		curAnimIndex = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void DecreaseAnimIndex()
	{
		curAnimIndex--;
		
		if(curAnimIndex < 0)
			curAnimIndex = 0;
	}
	
	void IncreaseAnimIndex()
	{
		curAnimIndex++;
		
		if(curAnimIndex > animationList.animations.Length - 1)
			curAnimIndex = animationList.animations.Length - 1;
	}
	
	void PlayCurrentAnimation()
	{
		modelAnimation.Stop();
		modelAnimation.clip = animationList.animations[curAnimIndex];
		modelAnimation.Play();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0, 10, Screen.width, 80));
		
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("<", GUILayout.MaxWidth(30), GUILayout.MinWidth(30)))
		{
			DecreaseAnimIndex();
		}
		
		GUILayout.Label(string.Format("{0}", animationList.animations[curAnimIndex].name), GUILayout.MaxWidth(200), GUILayout.MinWidth(200));
		
		if(GUILayout.Button(">", GUILayout.MaxWidth(30), GUILayout.MinWidth(30)))
		{
			IncreaseAnimIndex();
		}
		
		
		GUILayout.EndHorizontal();
		
		if(GUILayout.Button ("PLAY", GUILayout.MinWidth(100), GUILayout.MaxWidth(100)))
		{
			PlayCurrentAnimation();
		}
		
		GUILayout.EndArea();
	}
}
