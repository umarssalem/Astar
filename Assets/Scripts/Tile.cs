using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour, IAStarNode
{
    const float maxDis = 1.04f;
    public float cost { get; set; }

    public void setTerrain(TerrainAlt t)
    {
        GetComponent<MeshRenderer>().material = t.TerrainMaterial;
        cost = t.Cost;
    }
    public Tile()
    {
      
    }
    public bool IsCloser(Tile tile)
    {        
        return Vector3.Distance(transform.position, tile.transform.position) <= maxDis;    
    }

   public List<Tile> closedTiles = new List<Tile>();


    public IEnumerable<IAStarNode> Neighbours => closedTiles;

    public float CostTo(IAStarNode neighbour)
    {
        Tile t = neighbour as Tile;
        return t.cost;
    }

    public float EstimatedCostTo(IAStarNode goal)
    {
        Tile t = goal as Tile;
        return Vector3.Distance(transform.position, t.transform.position);
    }
    
}
