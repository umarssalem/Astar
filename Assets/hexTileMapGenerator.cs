using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Unity.Collections;

public class hexTileMapGenerator : MonoBehaviour
{
    private GameObject startWayPoint;
    private GameObject endWayPoint;
    private List<GameObject> path;
    public Material waypointMaterial;
    public GameObject IndicatorPrefab;
    public Tile hexTilePerfab;
    public TerrainAlt[] Terrains;
    [SerializeField] int width = 8;
    [SerializeField] int height = 8;
    [SerializeField] float tileXOffset = 1.04f;
    [SerializeField] float tileZOffset = 0.8f;
 

    private GameObject InstantiateWayPoint()
    {
        GameObject temp = Instantiate(IndicatorPrefab);
        temp.SetActive(false);
        temp.GetComponent<MeshRenderer>().material = waypointMaterial;
        return temp;

    }
    GameObject getNode()
    {
        Vector3 position = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit))
        {
            
            return hit.collider.gameObject;
        }
        return null;
    }
    void insertArt()
    {

    }
    void checkNeighbor(Tile[,] tiles)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var t = tiles[i, j];
                for (int a = i - 1; a <= i + 1; a++)
                {
                    for (int b = j - 1; b <= j + 1; b++)
                    {
                        if (a < 0 || a >= width || b < 0 || b >= height)
                        {
                            continue;
                        }
                        if (t == tiles[a, b] || !t.IsCloser(tiles[a, b]))
                        {
                            continue;
                        }
                        t.closedTiles.Add(tiles[a, b]);
                    }
                }
            }
        }
    }
    void createHexTileMap()
    {
        Tile[,] tiles = new Tile[width, height];
        for(int x = 0; x< width; x++)
        {
            for(int z = 0; z< height; z++)
            {
                Tile tempGo = Instantiate(hexTilePerfab);
                tempGo.name = "tile " + x + " " + z;
                tiles[x, z] = tempGo;
                tempGo.setTerrain(getRandomTerrain());
                if (z % 2 == 1)
                {
                    tempGo.transform.position = new Vector3(x * tileXOffset, 0, z * tileZOffset);
                }
                else
                {
                    tempGo.transform.position = new Vector3(x * tileXOffset + tileXOffset / 2, 0, z * tileZOffset);
                }
            }

        }
        checkNeighbor(tiles);
    }
    private TerrainAlt getRandomTerrain()
    {
        return Terrains[UnityEngine.Random.Range(0, Terrains.Length)];
    }
    List<GameObject> nodes = new List<GameObject>();
    void Start()
    {
        path = new List<GameObject>();
        startWayPoint = InstantiateWayPoint();
        endWayPoint = InstantiateWayPoint();
        createHexTileMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject goj = getNode();
            if(!nodes.Contains(goj))
            {
                nodes.Add(goj);
            }
            
        }
        if (nodes.Count == 2)
        {
            Tile t1 = nodes[0].GetComponent<Tile>();
            startWayPoint.transform.position = t1.transform.position;
            startWayPoint.SetActive(true);
            Tile t2 = nodes[1].GetComponent<Tile>();
            endWayPoint.transform.position = t2.transform.position;
            endWayPoint.SetActive(true);
            foreach(var node in path)
            {
                Destroy(node);
            }
            path.Clear();
            List<IAStarNode> tiles = new List <IAStarNode>(AStar.GetPath(t1, t2));
            foreach (var node in tiles)
            {
                Tile t = node as Tile;
                if (t ==  t1 || t == t2)
                {
                    continue;
                }
                GameObject temp = Instantiate(IndicatorPrefab);
                temp.transform.position = t.transform.position;
                path.Add(temp);
                Debug.Log((node as Tile).name);
            }
            nodes.Clear();
                

        }

    }
}
