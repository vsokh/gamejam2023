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
	[SerializeField] private GameObject EndMenu;
	[SerializeField] private GameObject WinMenu;
	private NodeState[][] mazeMatrix;
	private GameObject activeNode = null;
	private GameObject finishNode = null;
	private Vector2Int finishCoords;
	private GameObject startNode = null;
	private Vector2Int startCoords;
	public bool attemptFeiled = false;
	public GameObject NodePrefub;
	public GameObject StartPrefub;
	public GameObject FinishPrefub;
	public int dimentions = 8;
	public int roadsNumber = 4;
	public bool pathCreated = false;

	private List<GameObject> _nodeList = new List<GameObject>();
	private GameObject[,] _nodeMatrix;
	private List<MoveSet> _commandsList = new List<MoveSet>();

	private float step;
	void Start()
	{
		dimentions = SoundEffects.instance.scale;
		roadsNumber = (int)Mathf.Ceil((float)dimentions / 2);
		//Generate Maze
		mazeMatrix = mazeCreator.Generate(dimentions, roadsNumber);
		_nodeMatrix = new GameObject[dimentions, dimentions];
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
					node.GetComponent<Image>().color = new Color(1, 0.5f, 0.5f);
					node.GetComponent<NodeConnector>().willClose = true;
				}
				else if (mazeMatrix[i][j] == NodeState.Start)
				{
					_nodeList.Add(node);
					node.GetComponent<Image>().color = new Color(0.8f, 1, 0.35f);
					startNode = node;
					startCoords = new Vector2Int(i, j);
					GameObject temp = Instantiate(StartPrefub, new Vector3(newCord.x - step, newCord.y, 0), Quaternion.identity, transform);
					temp.transform.localScale = new Vector3(3 * step / temp.transform.localScale.x, 3 * step / temp.transform.localScale.y, 1);
					LineRenderer lr = temp.GetComponent<LineRenderer>();
					lr.widthMultiplier = 8;
					Vector3[] linePoints = new Vector3[2];
					linePoints[0] = temp.transform.position;
					linePoints[0].z = 1;
					linePoints[1] = node.transform.position;
					linePoints[1].z = 1;
					lr.SetPositions(linePoints);
				}
				else if (mazeMatrix[i][j] == NodeState.Finish)
				{
					node.GetComponent<Image>().color = new Color(0.35f, 1, 0.35f);
					finishNode = node;
					finishCoords = new Vector2Int(i, j);
					node.GetComponent<NodeConnector>().isFinish = true;
					GameObject temp = Instantiate(FinishPrefub, new Vector3(newCord.x + step, newCord.y, 0), Quaternion.identity, transform);
					temp.transform.localScale = new Vector3(0.7f * step / temp.transform.localScale.x, 0.7f * step / temp.transform.localScale.y, 1);
					LineRenderer lr = temp.GetComponent<LineRenderer>();
					lr.widthMultiplier = 8;
					Vector3[] linePoints = new Vector3[2];
					linePoints[0] = temp.transform.position;
					linePoints[0].z = 1;
					linePoints[1] = node.transform.position;
					linePoints[1].z = 1;
					lr.SetPositions(linePoints);
				}

				node.transform.localScale = new Vector3(step / 100, step / 100, step / 100);
				node.GetComponent<NodeConnector>().gl = this;
				node.SetActive(true);

				_nodeMatrix[i, j] = node;
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

		if (_nodeList.Contains(newNode))
		{
			_commandsList.RemoveAt(_commandsList.Count - 1);
			_nodeList.RemoveAt(_nodeList.Count - 1);
			SoundEffects.instance.DisconnectPlay();
		}
		else 
		{
			SoundEffects.instance.Play();
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
			StartCoroutine(ReversePing());
		}	
	}

	public bool IsPrevNode(GameObject testNode)
	{
		return _nodeList.Count > 1 && _nodeList[_nodeList.Count - 2] == testNode;
	}

	private IEnumerator ReversePing()
	{
		StartCoroutine(ConnectionAnim(true, 0.3f));
		yield return new WaitForSeconds((_commandsList.Count + 1) * 0.3f);
		for (int i = 0; i < _commandsList.Count; i++)
		{
			int val = (int) _commandsList[i];
			_commandsList[i] = (MoveSet)((val + 2) % 4);
		}
		for (int i = 0; i < dimentions; i++)
		{	
			for (int j = 0; j < dimentions; j++)
			{
				NodeConnector node = _nodeMatrix[i, j].GetComponent<NodeConnector>();
				if(node.willClose)
				{
					node.isClosed = true;
					node.GetComponent<Image>().color = Color.red;
					yield return new WaitForSeconds(0.1f);
				}
			}
		}
		StartCoroutine(ConnectionAnim(false, 0.5f));
	}

	private IEnumerator ConnectionAnim(bool isDisconnecting, float deley)
	{
		GameObject currNode = isDisconnecting ? startNode : finishNode;
		GameObject nextNode = null;
		Vector2Int currCoords = isDisconnecting ? startCoords : finishCoords;
		foreach (MoveSet command in _commandsList)
		{
			if (command == MoveSet.Up) currCoords.y -= 1;
			else if (command == MoveSet.Down) currCoords.y += 1;
			else if (command == MoveSet.Left) currCoords.x -= 1;
			else currCoords.x += 1;

			if (currCoords.x < 0 || currCoords.x >= dimentions || currCoords.y < 0 || currCoords.y >= dimentions)
				nextNode = null;
			else
				nextNode = _nodeMatrix[currCoords.x, currCoords.y];
			if (isDisconnecting) 
			{
				currNode.GetComponent<LineRenderer>().enabled = false;
				nextNode.GetComponent<NodeConnector>().connect.Play();
				SoundEffects.instance.DisconnectPlay();
			}
			else
			{
				if (nextNode == null)
				{
					currNode.GetComponent<NodeConnector>().ripples.startColor = Color.red;
					currNode.GetComponent<NodeConnector>().ripples.Play();
					SoundEffects.instance.DisconnectPlay();
					yield return new WaitForSeconds(deley);
					EndMenu.SetActive(true);
					EndMenu.GetComponentInChildren<typewriterUI>().StartWriting();
					attemptFeiled = true;
					break;
				}
				else if (nextNode.GetComponent<NodeConnector>().isClosed)
				{
					currNode.GetComponent<NodeConnector>().ReverseConnection(nextNode, true);
					SoundEffects.instance.DisconnectPlay();
					yield return new WaitForSeconds(deley);
					EndMenu.SetActive(true);
					EndMenu.GetComponentInChildren<typewriterUI>().StartWriting();
					attemptFeiled = true;
					break;
				}
				currNode.GetComponent<NodeConnector>().ReverseConnection(nextNode);
				SoundEffects.instance.Play();
			}
			currNode = nextNode;
			yield return new WaitForSeconds(deley);
		}
		if (!isDisconnecting && !attemptFeiled)
		{
			WinMenu.SetActive(true);
			WinMenu.GetComponentInChildren<typewriterUI>().StartWriting();
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
