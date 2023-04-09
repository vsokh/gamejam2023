using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameLogic : MonoBehaviour
{
	public enum MoveSet
	{
		Up,
		Right,
		Down,
		Left
	}
	[SerializeField] private MazeGenerator mazeGenerator;
	[SerializeField] private MazeCreator mazeCreator;
	private NodeState[][] mazeMatrix;
	private GameObject activeNode = null;
	public GameObject NodePrefub;
	public int dimentions = 5;
	public bool pathCreated = false;

	private List<GameObject> _nodeList = new List<GameObject>();
	[SerializeField] private List<MoveSet> _commandsList = new List<MoveSet>();

	private float step;
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
					_nodeList.Add(node);
					node.GetComponent<Image>().color = Color.yellow;
				}
				else if (mazeMatrix[i][j] == NodeState.Finish)
				{
					node.GetComponent<Image>().color = Color.cyan;
					node.GetComponent<NodeConnector>().isFinish = true;
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
		if (_nodeList.Contains(testNode) && _nodeList[_nodeList.Count - 2] != testNode) return false;

		return true;
	}

	public bool IsListHead(GameObject test)
	{
		if (_nodeList.Count == 0) return false;
		return _nodeList[_nodeList.Count - 1] == test;
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
		SoundEffects.instance.Play();

		if (_nodeList.Contains(newNode))
		{
			_commandsList.RemoveAt(_commandsList.Count - 1);
			_nodeList.RemoveAt(_nodeList.Count - 1);
		}
		else 
		{
			GameObject currentNode = _nodeList[_nodeList.Count - 1];
			if (newNode.transform.position.x == currentNode.transform.position.x)
			{
				if (newNode.transform.position.y > currentNode.transform.position.y) _commandsList.Add(MoveSet.Up);
				else _commandsList.Add(MoveSet.Down);
			}
			else
			{
				if (newNode.transform.position.x > currentNode.transform.position.x) _commandsList.Add(MoveSet.Right);
				else _commandsList.Add(MoveSet.Left);
			}
			_nodeList.Add(newNode);
		}

		if (newNode.GetComponent<NodeConnector>().isFinish == true)
		{
			pathCreated = true;
			ReversePing();
		}	
	}

	public bool IsPrevNode(GameObject testNode)
	{
		return _nodeList.Count > 1 && _nodeList[_nodeList.Count - 2] == testNode;
	}

	private void ReversePing()
	{
		for (int i = 0; i < _commandsList.Count; i++)
		{
			int val = (int) _commandsList[i];
			_commandsList[i] = (MoveSet)((val + 2) % 4);
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
