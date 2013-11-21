using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// A simple NPC helper to do cool animation effects and sounds
/// </summary>
/// 
public class NPCHelper : MonoBehaviour {
	
	//Current animation being used
	private int mAnimationIndex;
	//ref to animation component
	private Animation mAnimation;
	//ref to collider
	private BoxCollider mCollider;
	//list of animation names
	private List<string> animList = new List<string>();
	public List<AudioClip> soundList = new List<AudioClip>();
	
	private AudioSource mAudioSource;
	private AudioSource mAudioSourceIdle;
	
	public void PlayRandomAnimation() {
		mAnimationIndex++;
		if (mAnimationIndex >= animList.Count) mAnimationIndex = 0;
		mAnimation.Play(animList[mAnimationIndex]);
		//Attack sound
		switch (animList[mAnimationIndex]) {
		case "Melee1H":
			foreach (AudioClip curClip in soundList) {
				if (curClip.name.Contains("_att")) {
					mAudioSource.clip = curClip;
					mAudioSource.Play();
					break;
				}
			}
			break;
		case "Hurt_01":
			foreach (AudioClip curClip in soundList) {
				if (curClip.name.Contains("_hit")) {
					mAudioSource.clip = curClip;
					mAudioSource.Play();
					break;
				}
			}
			break;
		case "Idle":
		case "Idle_01":			
			foreach (AudioClip curClip in soundList) {
				if (curClip.name.Contains("_idl")) {
					mAudioSource.clip = curClip;
					mAudioSource.Play();
					break;
				}
			}
			break;
		case "Dead":
			foreach (AudioClip curClip in soundList) {
				if (curClip.name.Contains("_die")) {
					mAudioSource.clip = curClip;
					mAudioSource.Play();
					break;
				}
			}
			break;
		}
	}
	
	private void Start() {
		mAnimation = GetComponent<Animation>();
		if (mAnimation == null) {
			Debug.Log (name+" does not have any animations, destroying NPC Helper");
			Destroy(gameObject);
		}
		foreach (AnimationState curAnim in mAnimation ) {
			animList.Add(curAnim.name);
		}
		mAudioSource = gameObject.AddComponent<AudioSource>();
		mAudioSourceIdle = gameObject.AddComponent<AudioSource>();
		foreach (AudioClip curClip in soundList) {
				if (curClip.name.Contains("loop")) { //Special hack really only used for spectre :(
					mAudioSourceIdle.clip = curClip;
					mAudioSourceIdle.loop = true;
					mAudioSourceIdle.Play();
				}
			}	
	}
}
