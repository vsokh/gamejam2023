using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCheker : MonoBehaviour
{
	void Start()
	{
		transform.localScale *= transform.parent.localScale.x;
	}
}
