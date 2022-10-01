using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGridManager : MonoBehaviour {
    private Automaton automaton;

    [SerializeField] private GameObject gridBitPrefab;
    private GridBit[,] gridBits;

    [Header("birth/survival/generations")]
    [SerializeField] private string rulestring = "3/23/2";

    [Header("etc")]
    [SerializeField] private int neighbourhoodRadius = 1;
    [SerializeField] private bool wrap = true;

    [Header("grid")]
    [SerializeField] private Automaton.FillType fillType = Automaton.FillType.BitRandom;
    [SerializeField] private Vector2Int dimensions = new(50, 100);
    [SerializeField] private Vector2 pivot = new(0.5f, 0f);

    private void Start () {
        InitializeTheGrid();
        automaton = new Automaton(rulestring, dimensions, fillType, neighbourhoodRadius, wrap);
        WorldClock.OnTick += UpdateTheGrid;
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("refreshing...");

            InitializeTheGrid();
            automaton = new Automaton(rulestring, dimensions, fillType, neighbourhoodRadius, wrap);
        }
    }

    private void UpdateTheGrid () {
        automaton.SimulationStep();
        float[,] grid = automaton.GetANormalizedGrid();

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                gridBits[x, y].UpdateState(grid[x,y]);
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
    }
}
