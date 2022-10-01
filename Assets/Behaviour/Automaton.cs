using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automaton {
    private readonly List<int> birthconditions = new();
    private readonly List<int> survivalConditions = new();
    private readonly int maxState = 1;
    private readonly int neighbourhoodRadius;
    private readonly bool wrap;
    
    private int[,] internalGrid;

    private Vector2Int Dimensions => new(internalGrid.GetLength(0), internalGrid.GetLength(1));

    public enum FillType {None, BitRandom, Random, CenterDot}

    public Automaton (string rulestring, Vector2Int dimensions, FillType fillType = FillType.BitRandom, int neighbourhoodRadius = 1, bool wrap = true) {
        string[] rules = rulestring.Split('/');

        foreach (char c in rules[0]) birthconditions.Add(c - '0');
        foreach (char c in rules[1]) survivalConditions.Add(c - '0');
        maxState = int.Parse(rules[2]) - 1;

        // initializing the grid
        internalGrid = new int[dimensions.x, dimensions.y];
        switch (fillType) {
            case FillType.None:
                break;

            case FillType.BitRandom:
                for (int x = 0; x < Dimensions.x; x++) {
                    for (int y = 0; y < Dimensions.y; y++) {
                        internalGrid[x, y] = Random.value > 0.5f ? maxState : 0;
                    }
                }
                break;
            case FillType.Random:
                for (int x = 0; x < Dimensions.x; x++) {
                    for (int y = 0; y < Dimensions.y; y++) {
                        internalGrid[x, y] = Random.Range(0, maxState + 1);
                    }
                }
                break;

            case FillType.CenterDot:
                internalGrid[Dimensions.x / 2, Dimensions.y / 2] = maxState;
                break;
        }

        this.neighbourhoodRadius = neighbourhoodRadius;
        this.wrap = wrap;
    }

    public float[,] GetANormalizedGrid () {
        float[,] normalizedGrid = new float[Dimensions.x, Dimensions.y];

        for (int x = 0; x < Dimensions.x; x++) {
            for (int y = 0; y < Dimensions.y; y++) {
                normalizedGrid[x, y] = (float)internalGrid[x, y] / maxState;
            }
        }

        return normalizedGrid;
    }

    public void SimulationStep() {
        int[,] newGrid = new int[Dimensions.x, Dimensions.y];

        for (int x = 0; x < Dimensions.x; x++) {
            for (int y = 0; y < Dimensions.y; y++) {
                newGrid[x, y] = GetNewCellState(x, y);
            }
        }

        internalGrid = newGrid;
    }

    private int GetNewCellState (int x, int y) {
        int numNeighbours = CountNeighbours(x, y);
        int cellState = internalGrid[x, y];

        if (cellState == 0) { // if the cell is dead
            if(birthconditions.Contains(numNeighbours)) { // if enough neighbours for birth
                // commit birth
                return maxState;   
            }
        } else { // if the cell is alive or dying
            if (!survivalConditions.Contains(numNeighbours)) { // if not enough neighbours for survival
                // progress in dying
                return cellState - 1;
            }
        }

        // remain unchaged otherwise
        return cellState;
    }

    private int CountNeighbours(int x, int y) {
        int neighbourCount = 0;

        for (int xOffset = -neighbourhoodRadius; xOffset <= neighbourhoodRadius; xOffset++) {
            for (int yOffset = -neighbourhoodRadius; yOffset <= neighbourhoodRadius; yOffset++) {
                if (xOffset == 0 && yOffset == 0) continue;

                if (SafeSample(x + xOffset, y + yOffset) == maxState) neighbourCount++;
            }
        }

        return neighbourCount;
    }

    private int SafeSample(int x, int y) {
        int lastCellIdX = Dimensions.x - 1;
        int lastCellIdY = Dimensions.y - 1;

        int safeCellIdX = wrap ? (lastCellIdX + x) % lastCellIdX : Mathf.Clamp(x, 0, lastCellIdX);
        int safeCellIdY = wrap ? (lastCellIdY + y) % lastCellIdY : Mathf.Clamp(y, 0, lastCellIdY);

        return internalGrid[safeCellIdX, safeCellIdY];
    }
}
