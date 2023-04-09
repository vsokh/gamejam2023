using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
	[SerializeField] private MazeGenerator mazeGenerator;
	private NodeState[][] mazeMatrix;
	private GameObject activeNode = null;
	public GameObject NodePrefub;
	public int dimentions = 5;

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

		return true;
	}

	public void SetActiveNode(GameObject node)
	{
		activeNode = node;
	}
	public GameObject GetActiveNode()
	{
		return activeNode;
	}
	void Update()
	{
		
	}
}
