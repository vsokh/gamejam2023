using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class TestLine : MonoBehaviour {
 
	public float width = 5f;
	public Color color = Color.cyan;
	private LineRenderer lr;
	public ArrayList linePoints;
	private int pointsNum = 0;
	

 
	 void Start ()
	 {
		lr = GetComponent<LineRenderer> ();
		if (!lr) lr = gameObject.AddComponent<LineRenderer> ();
		lr.material.color = color;
		lr.widthMultiplier = width;
		lr.enabled = true;
		linePoints = new ArrayList();
	 }
 
	 void Update ()
	 {
		Camera c = Camera.main;
		if (Input.GetMouseButtonDown(0) && pointsNum == 0)
		{
			Vector3 currentPos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
			currentPos.z = 0;
			linePoints.Add(currentPos);
			pointsNum++;
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 currentPos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
			currentPos.z = 0;
			if (linePoints.Count <= pointsNum) linePoints.Add(currentPos);
			else linePoints[pointsNum] = currentPos;	
			SetPositionsToLineRenderer();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			Vector3 currentPos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
			currentPos.z = 0;
			linePoints[pointsNum] = currentPos;
			pointsNum++;
			SetPositionsToLineRenderer();
		}

	}

	void SetPositionsToLineRenderer()
	{
		lr.positionCount = linePoints.Count;
		Vector3[] positions = new Vector3[linePoints.Count];
		for (int i = 0; i < linePoints.Count; i++)
		{
			positions[i] = (Vector3)linePoints[i];
		}
		Debug.Log(positions.Length);
		lr.SetPositions (positions);

	}
 
 }
