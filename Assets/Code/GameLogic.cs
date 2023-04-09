using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameLogic : MonoBehaviour
{
	[SerializeField] private MazeGenerator mazeGenerator;
	private NodeState[][] mazeMatrix;
	private GameObject activeNode = null;
	public GameObject NodePrefub;
	public int dimentions = 5;

	private List<GameObject> _pathList;

	private float step;
	// Start is called before the first frame update
	void Start()
	{
		//Generate Maze
		mazeMatrix = mazeGenerator.Generate(dimentions, dimentions);

		//Draw Nodes to screen
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		int w = Mathf.RoundToInt(rt.rect.width);
		int h = Mathf.RoundToInt(rt.rect.height);

		int gridSize = Mathf.RoundToInt(Mathf.Min(w, h) * 0.9f);
		step = gridSize / dimentions;
		Vector2 startCord = new Vector2(-gridSize / 2 + step / 2, gridSize / 2 - step /2);
		for (int i = 0; i < dimentions; i++)
		{
			for (int j = 0; j < dimentions; j++)
			{
				Vector2 newCord = startCord + new Vector2(step * i, -step * j);
				GameObject node = Instantiate(NodePrefub, new Vector3(newCord.x, newCord.y, 0), Quaternion.identity, transform);
				if (mazeMatrix[i][j] == NodeState.Closed)
				{
					node.GetComponent<Image>().color = Color.red;
					node.GetComponent<NodeConnector>().isClosed = true;
				}
				else if (mazeMatrix[i][j] == NodeState.Start)
				{
					_pathList.Add(node);
					node.GetComponent<Image>().color = Color.yellow;
				}
				else if (mazeMatrix[i][j] == NodeState.Finish)
				{
					node.GetComponent<Image>().color = Color.yellow;
				}

				node.transform.localScale = new Vector3(step / 100, step / 100, step / 100);
				node.GetComponent<NodeConnector>().gl = this;
				node.SetActive(true);
			}
		}
	}

	public bool IsValidTarget(GameObject testNode)
	{
		if (activeNode == null) return false;
		if (testNode == activeNode) return false;
		if (testNode.GetComponent<NodeConnector>().isClosed) return false;
		if (testNode.transform.position.x != activeNode.transform.position.x && 
		testNode.transform.position.y != activeNode.transform.position.y) return false;
		if (Vector3.Distance(testNode.transform.position, activeNode.transform.position) > step * 1.1f) return false;
		if (_pathList.Contains(testNode) && _pathList[_pathList.Count - 2] != testNode) return false;

		return true;
	}

	public bool IsListHead(GameObject test)
	{
		if (_pathList.Count == 0) return false;
		return _pathList[_pathList.Count - 1] == test;
	}

	public void SetActiveNode(GameObject node)
	{
		activeNode = node;
	}
	public GameObject GetActiveNode()
	{
		return activeNode;
	}

	public void ConnectTo(GameObject newNode)
	{
		_pathList.Add(newNode);
		return ;
		if (_pathList.Count > 1 && newNode == _pathList[_pathList.Count - 2])
		{
			Debug.Log("AAAAAAAAAAAAAAa");
			//activeNode.GetComponent<LineRenderer>().enabled = false;
			//newNode.GetComponent<LineRenderer>().enabled = false;
			_pathList.Remove(activeNode);
		}
		else
		{
			_pathList.Add(newNode);
		}		
	}
	void Update()
	{
		if (Input.GetMouseButtonUp(0) && activeNode)
		{
			activeNode.GetComponent<NodeConnector>().OnPointerUp(null);
		}
	}
}
