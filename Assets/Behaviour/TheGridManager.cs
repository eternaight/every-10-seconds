using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGridManager : MonoBehaviour {
    private Automaton automaton;
    private GridBit[,] gridBits;
    [SerializeField] private GameObject gridBitPrefab;

    [Header("grid")]
    [SerializeField] private Automaton.FillType fillType = Automaton.FillType.BitRandom;
    [SerializeField] private Vector2Int dimensions = new(50, 100);
    [SerializeField] private Vector2 pivot = new(0.5f, 0f);
    [SerializeField] private Transform theWrapAround;
    private int leftmostColumn;
    private int rightmostColumn;

    private void Start () {
        InitializeTheGrid();
        WorldClock.OnTick += UpdateTheGrid;
        MenuManager.OnExitMenu += RecreateAutomaton;
    }

    private void RecreateAutomaton () => automaton = new Automaton(MenuManager.Rulestring, dimensions, fillType);

    private void Update () {
        float leftmostColDistance = theWrapAround.position.x - gridBits[leftmostColumn,0].transform.position.x;
        float rightmostColDistance = gridBits[rightmostColumn,0].transform.position.x - theWrapAround.position.x;
        float wrapThreshold = dimensions.x * 0.5f;

        if (leftmostColDistance  > wrapThreshold) WrapColumn(ref leftmostColumn, ref rightmostColumn);
        if (rightmostColDistance > wrapThreshold) WrapColumn(ref rightmostColumn, ref leftmostColumn);
    }

    private void WrapColumn (ref int wrapCol, ref int oppositeCol) {
        int wrapDir = (int)Mathf.Sign(gridBits[oppositeCol, 0].transform.position.x - gridBits[wrapCol, 0].transform.position.x);

        for (int y = 0; y < dimensions.y; y++) {
            gridBits[wrapCol, y].transform.position += new Vector3(dimensions.x * wrapDir, 0);
        }

        oppositeCol = wrapCol;
        wrapCol = (dimensions.x + wrapCol + wrapDir) % dimensions.x;
    }

    private void UpdateTheGrid () {
        automaton.SimulationStep();
        float[,] grid = automaton.GetANormalizedGrid();

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                gridBits[x, y].QueueState(grid[x,y]);
            }
        }
    }

    private void InitializeTheGrid () {
        if (gridBits != null) {
            for (int x = 0; x < gridBits.GetLength(0); x++) {
                for (int y = 0; y < gridBits.GetLength(1); y++) {
                    Destroy(gridBits[x, y].gameObject);
                }
            }
        }

        gridBits = new GridBit[dimensions.x, dimensions.y];

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                GameObject gridBitGO = Instantiate(gridBitPrefab, 
                    new Vector2(x, y) - dimensions * pivot, Quaternion.identity, transform);
                gridBits[x, y] = gridBitGO.GetComponent<GridBit>();
            }
        }

        leftmostColumn = 0;
        rightmostColumn = dimensions.x - 1;
    }
}
