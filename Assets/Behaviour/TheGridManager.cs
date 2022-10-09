using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGridManager : MonoBehaviour {
    private Automaton automaton;
    private GridBit[,] gridBits;
    [SerializeField] private GameObject gridBitPrefab;

    [Header("grid")]
    [SerializeField] private Vector2Int dimensions = new(50, 50);
    [SerializeField] private Vector2 pivot = new(0.5f, 0.5f);
    [SerializeField] private Transform theWrapAround;

    private int upmostRow;
    private int downmostRow;
    private int leftmostColumn;
    private int rightmostColumn;

    private void Awake () {
        InitializeTheGrid();
        WorldClock.OnPreTick += EnqueueTheGrid;
        MenuManager.OnMenuExit += RecreateAutomaton;
    }

    private void RecreateAutomaton () => automaton = new Automaton(MenuManager.Rulestring, dimensions, MenuManager.FillDensity);

    private void Update () => Wrap ();

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

        upmostRow = dimensions.y - 1;
        downmostRow = 0;
        leftmostColumn = 0;
        rightmostColumn = dimensions.x - 1;
    }

    private void EnqueueTheGrid () {
        float[,] grid = automaton.GetANormalizedGrid();

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                gridBits[x, y].EnqueueState(grid[x,y]);
            }
        }
        
        automaton.SimulationStep();
    }

    private void Wrap () {
        Vector2 wrapThreshold = (Vector2)dimensions * 0.5f;

        float leftmostColDistance = theWrapAround.position.x - gridBits[leftmostColumn, 0].transform.position.x;
        float rightmostColDistance = gridBits[rightmostColumn, 0].transform.position.x - theWrapAround.position.x;

        if (leftmostColDistance  > wrapThreshold.x) WrapColumn(ref leftmostColumn, ref rightmostColumn);
        if (rightmostColDistance > wrapThreshold.x) WrapColumn(ref rightmostColumn, ref leftmostColumn);

        float upmostRowDistance =  gridBits[0, upmostRow].transform.position.y - theWrapAround.position.y;
        float downmostRowDistance = theWrapAround.position.y - gridBits[0, downmostRow].transform.position.y;

        if (upmostRowDistance > wrapThreshold.y) WrapRow(ref upmostRow, ref downmostRow);
        if (downmostRowDistance > wrapThreshold.y) WrapRow(ref downmostRow, ref upmostRow);
    }

    private void WrapRow (ref int wrapRow, ref int oppositeRow) {
        int wrapDir = (int)Mathf.Sign(gridBits[0, oppositeRow].transform.position.y - gridBits[0, wrapRow].transform.position.y);

        for (int x = 0; x < dimensions.x; x++) {
            gridBits[x, wrapRow].transform.position += new Vector3(0, dimensions.y * wrapDir);
        }

        oppositeRow = wrapRow;
        wrapRow = (dimensions.y + wrapRow + wrapDir) % dimensions.y;
    }

    private void WrapColumn (ref int wrapCol, ref int oppositeCol) {
        int wrapDir = (int)Mathf.Sign(gridBits[oppositeCol, 0].transform.position.x - gridBits[wrapCol, 0].transform.position.x);

        for (int y = 0; y < dimensions.y; y++) {
            gridBits[wrapCol, y].transform.position += new Vector3(dimensions.x * wrapDir, 0);
        }

        oppositeCol = wrapCol;
        wrapCol = (dimensions.x + wrapCol + wrapDir) % dimensions.x;
    }
}
