using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void initMap(int numRows, int numCols)
    {
        int j = 0;
        int i = 0;
        int id = 0;
        string line;

        Vector3 position = Vector3.zero;
        Vector3 scale = Vector3.zero;

        vertices = new List<Vertex>(numRows * numCols);
        neighbors = new List<List<Vertex>>(numRows * numCols);
        costs = new List<List<float>>(numRows * numCols);
        vertexObjs = new GameObject[numRows * numCols];
        mapVertices = new bool[numRows, numCols];

        for (i = 0; i < numRows; i++)
        {
            for (j = 0; j < numCols; j++)
            {

                int rnd = 100;
                bool isGround = false;
                if (line[j] != 'T')
                {
                    isGround = true;
                    rnd = UnityEngine.Random.Range(1, 5);
                    if (line[j] == 'S')
                    {
                        vertexObjs[id].name = "Exit";
                        salida.transform.position = new Vector3(j * cellSize, 1, i * cellSize);
                    }
                }

                mapVertices[i, j] = isGround;
                position.x = j * cellSize;
                position.z = i * cellSize;

                id = GridToId(j, i);

                if (isGround)
                {
                    vertexObjs[id] = Instantiate(vertexPrefab, position, Quaternion.identity) as GameObject;
                    //vertexObjs[id].GetComponent<Renderer>().material.color = Color.cyan;
                }
                else
                    vertexObjs[id] = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
                vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
