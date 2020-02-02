using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
	public AudioClip[] sounds = null;

	AudioSource audSrc = null;

	void Awake()
	{
		audSrc = GetComponent<AudioSource>();
	}
	
	public void PlayRandom()
	{
		audSrc.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
	}
}
