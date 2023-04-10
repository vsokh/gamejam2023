using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuturCloser : MonoBehaviour
{
	void Awake()
	{
		if (SoundEffects.instance.showTutor)
		{
			SoundEffects.instance.showTutor = false;
		}
		else
			gameObject.SetActive(false);
	}
}
