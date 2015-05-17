using UnityEngine;
using System.Collections;

public class MusicBehavior : MonoBehaviour 
{
	public AudioClip impact;
	AudioSource audio;
	GameObject SFX1;
	
	void Start() 
	{
		SFX1 = GameObject.Find("SFX1");
		audio = GetComponent<AudioSource>();
		audio.Play();
		impact = SFX1.GetComponent<AudioSource>().clip;
	}

	void Update()
	{
		if (Input.GetKeyDown("x"))
		{
			SFX1.GetComponent<AudioSource>().PlayOneShot(impact);
		}
	}

}