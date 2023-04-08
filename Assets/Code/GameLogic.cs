using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
	public GameObject NodePrefub;
	public Vector2 dimentions = new Vector2(6, 6);
	// Start is called before the first frame update
	void Start()
	{
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		int w = Mathf.RoundToInt(rt.rect.width);
		int h = Mathf.RoundToInt(rt.rect.height);

		int gridSize = Mathf.RoundToInt(Mathf.Min(w, h) * 0.9f);
		float step = gridSize / dimentions.x;
		Debug.Log(step);
		Vector2 startCord = new Vector2(-gridSize / 2 + step / 2, gridSize / 2 - step /2);
		Debug.Log(startCord);
		for (int i = 0; i < dimentions.x; i++)
		{
			for (int j = 0; j < dimentions.y; j++)
			{
				Vector2 newCord = startCord + new Vector2(step * i, -step * j);
				GameObject node = Instantiate(NodePrefub, new Vector3(newCord.x, newCord.y, 0), Quaternion.identity, transform);
				//node.transform.position -= node.GetComponent<Renderer>().bounds.center;
				node.GetComponent<RectTransform>().sizeDelta = new Vector2(step, step);
				node.SetActive(true);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
