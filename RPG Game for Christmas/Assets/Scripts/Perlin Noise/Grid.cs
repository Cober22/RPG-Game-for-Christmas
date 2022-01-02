using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;
    public Nodo[,] grid;
    public int gridSizeX, gridSizeY;
    private GameObject tile;
    //Vector3 test;

    private float nodeDiameter;

    void Awake()
    {
        GameObject grid = new GameObject();
        grid.name = "Grid";

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        if (distance < nodeDiameter)
            distance = nodeDiameter;
        
        int maxNodeRadius = gridWorldSize.x < gridWorldSize.y ? Mathf.RoundToInt(gridWorldSize.x) : Mathf.RoundToInt(gridWorldSize.y);
        if (nodeRadius > maxNodeRadius)
            distance = maxNodeRadius;

        grid = new Nodo[gridSizeX, gridSizeY];
        //Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Vector3 worldPoint = Vector3.zero - Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                Vector3 worldPoint = Vector3.zero - Vector3.right * (x * distance) + Vector3.forward * (y * distance);

                //-----------   CREACION DE GRID DE TILES   -----------//
                //GameObject newTile = Instantiate(tile);
                GameObject newTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newTile.transform.SetParent(GameObject.Find("/Grid").transform);
                //newTile.transform.position = new Vector3(x, y, 0f);
                newTile.transform.localScale *= nodeDiameter;
                Vector3 positionRounded = new Vector3(Mathf.RoundToInt(worldPoint.x), Mathf.RoundToInt(worldPoint.y), Mathf.RoundToInt(worldPoint.z));
                newTile.transform.position = positionRounded;
                Nodo nodo = new Nodo(false, positionRounded, x, y, newTile);
                nodo.terrainObject = newTile;
                grid[x, y] = nodo;
            }
        }
    }

    public List<Nodo> GetNeighbouringNodes(Nodo a_Node)
    {
        List<Nodo> NeighbouringNodes = new List<Nodo>();
        int xCheck;
        int yCheck;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) //if we are on the node tha was passed in, skip this iteration.
                    continue;
                else if (x == -1 && (y == -1 || y == 1) || x == 1 && (y == -1 || y == 1))
                    continue;

                xCheck = a_Node.gridX + x;
                yCheck = a_Node.gridY + y;

                //Make sure the node is within the grid.
                if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
                {
                    NeighbouringNodes.Add(grid[xCheck, yCheck]); //Adds to the neighbours list.
                }
            }
        }

        return NeighbouringNodes;
    }

    public List<Nodo> GetAllNeighbouringNodes(Nodo a_Node)
    {
        List<Nodo> NeighbouringNodes = new List<Nodo>();
        int xCheck;
        int yCheck;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) //if we are on the node tha was passed in, skip this iteration.
                    continue;

                xCheck = a_Node.gridX + x;
                yCheck = a_Node.gridY + y;

                //Make sure the node is within the grid.
                if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
                {
                    NeighbouringNodes.Add(grid[xCheck, yCheck]); //Adds to the neighbours list.
                }
            }
        }

        return NeighbouringNodes;
    }

    public Nodo NodeFromWorldPosition(Vector3 a_WorldPosition)
    {
        float percentX = (a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (a_WorldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public Vector2 Vec2FromWorldPosition(Vector3 a_WorldPosition)
    {
        float percentX = (a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (a_WorldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
       
        if (grid != null)
            foreach (Nodo nodo in grid)
                Gizmos.DrawCube(nodo.position, Vector3.one * (nodeDiameter - distance));
    }
}