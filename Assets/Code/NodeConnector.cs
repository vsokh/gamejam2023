using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeConnector : MonoBehaviour, 
IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler//, IPointerMoveHandler
{
	public float width = 5f;
	public Color color = Color.cyan;
	public bool isClosed = false;
	public GameLogic gl;
	private LineRenderer lr;
	private Vector3[] linePoints = new Vector3[2];
	private bool trackMouse = false;
	private bool isSnapping = false;
	private GameObject snappingNode = null;
	private Color _hlColor;

	public ParticleSystem ripples;
	public ParticleSystem connect;
	void Start()
	{
		GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
		lr = GetComponent<LineRenderer> ();
		//gl = GameObject.FindObjectOfType<GameLogic>();
		if (!lr) lr = gameObject.AddComponent<LineRenderer> ();
		lr.material.color = color;
		lr.widthMultiplier = width;
		lr.positionCount = 2;
		lr.enabled = false;
		linePoints[0] = transform.position;
		linePoints[0].z = 0;
		_hlColor = GetComponent<Button>().colors.highlightedColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		gl.SetActiveNode(gameObject);
		trackMouse = true;
		lr.enabled = true;
	}
 
	public void OnPointerUp(PointerEventData eventData)
	{
		if (snappingNode && gl.IsValidTarget(snappingNode))
		{
			linePoints[1] = snappingNode.transform.position;
			lr.SetPositions (linePoints);
			snappingNode.GetComponent<NodeConnector>().connect.Play();
		}
		else
			lr.enabled = false;
		trackMouse = false;
		isSnapping = false;
		snappingNode = null;
		gl.SetActiveNode(null);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (gl.GetActiveNode())
		{
			if (gl.IsValidTarget(gameObject))
			{
				GameObject activeNode = gl.GetActiveNode();
				activeNode.GetComponent<NodeConnector>().LineSnap(gameObject);
				ripples.Play();
			}
			else
			{
				ColorBlock colorVar = GetComponent<Button>().colors;
				colorVar.highlightedColor = Color.magenta;
				GetComponent<Button>().colors = colorVar;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameObject activeNode = gl.GetActiveNode();
		if (activeNode && gameObject != activeNode)
		{
			activeNode.GetComponent<NodeConnector>().LineRelease();
		}
		ColorBlock colorVar = GetComponent<Button>().colors;
		colorVar.highlightedColor = _hlColor;
		GetComponent<Button>().colors = colorVar;
	}

	// public void OnPointerMove(PointerEventData eventData)
	// {
	// 	GameObject activeNode = gl.GettActiveNode();
	// 	if (activeNode && gameObject != activeNode)
	// 	{
	// 		activeNode.GetComponent<NodeConnector>().LineRelease();
	// 	}
	// }

	public void LineSnap(GameObject candicate)
	{
		isSnapping = true;
		snappingNode = candicate;
		// Camera c = Camera.main;
		// Vector3 mousePos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));

		// linePoints[1] = (mousePos + pos) / 2;
		// linePoints[1].z = 0;
		// lr.SetPositions (linePoints);
		// trackMouse = false;
	}

	public void LineRelease()
	{
		isSnapping = false;
		snappingNode = null;
	}
	void Update()
	{
		if (trackMouse)
		{
			Camera c = Camera.main;
			Vector3 mousePos = c.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
			if (isSnapping)
			{
				linePoints[1] = (snappingNode.transform.position + mousePos) / 2;
			}
			else
			{
				linePoints[1] = mousePos;
			}
			
			linePoints[1].z = 0;
			lr.SetPositions (linePoints);
		}
	}
}
