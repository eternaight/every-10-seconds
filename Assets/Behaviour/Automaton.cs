using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automaton {
    private readonly List<int> birthconditions = new();
    private readonly List<int> survivalConditions = new();
    private readonly int maxState = 1;
    
    private int[,] internalGrid;
    private Vector2Int Dimensions => new(internalGrid.GetLength(0), internalGrid.GetLength(1));

    public Automaton (string rulestring, Vector2Int dimensions, float fillDensity = 0.5f) {
        string[] rules = rulestring.Split('/');

        foreach (char c in rules[0]) birthconditions.Add(c - '0');
        foreach (char c in rules[1]) survivalConditions.Add(c - '0');
        maxState = (rules.Length > 2) ? int.Parse(rules[2]) - 1 : 1;

        CreateTheGrid(dimensions, fillDensity);
    }

    private void CreateTheGrid (Vector2Int dimensions, float fillDensity = 0.5f) {
        internalGrid = new int[dimensions.x, dimensions.y];
        
        for (int x = 0; x < Dimensions.x; x++) {
            for (int y = 0; y < Dimensions.y; y++) {
                internalGrid[x, y] = Random.value < fillDensity ? maxState : 0;
            }
        }
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

        for (int xOffset = -1; xOffset <= 1; xOffset++) {
            for (int yOffset = -1; yOffset <= 1; yOffset++) {
                if (xOffset == 0 && yOffset == 0) continue;

                if (WrappedSample(x + xOffset, y + yOffset) == maxState) neighbourCount++;
            }
        }

        return neighbourCount;
    }

    private int WrappedSample(int x, int y) {
        int wrappedCellX = (Dimensions.x + x) % Dimensions.x;
        int wrappedCellY = (Dimensions.y + y) % Dimensions.y;

        return internalGrid[wrappedCellX, wrappedCellY];
    }
}
