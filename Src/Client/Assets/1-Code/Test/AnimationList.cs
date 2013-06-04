/* Since unity doesn't give direct access to an animation components animation's list, we'll store them
 * here, and then pass them along.
 * Created Jan 18 2013
 *
 */
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]

public class AnimationList : MonoBehaviour {
	
	public AnimationClip[] animations;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
