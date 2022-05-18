namespace UCM.IAV.CristianCastillo {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class InfluenceMap : MonoBehaviour
    {
        float cellSize;

        int fils, cols;

        InfluenceTile[] matriz;
        InfluenceTile leastInfluencedTile;
        List<Vertex> vertices;
        public GraphGrid graph;

        bool influenceVisible;

        [SerializeField]
        GameObject tilePrefab;


        public static InfluenceMap instance;
        private void Awake()
        {
            if (!instance) instance = this;
            else Destroy(this.gameObject);
        }
        public static InfluenceMap GetInstance()
        {
            return instance;
        }

        // Start is called before the first frame update
        void Start()
        {
            cellSize = graph.cellSize;
        }

        public void initMap(int numRows, int numCols)
        {
            int j = 0;
            int i = 0;
            int id = 0;

            Vector3 position = Vector3.zero;
            matriz = new InfluenceTile[numRows * numCols];
            vertices = graph.GetVertices();

            for (i = 0; i < numRows; i++)
            {
                for (j = 0; j < numCols; j++)
                {
                    bool isWall = false;

                    position.x = j * cellSize;
                    position.z = i * cellSize;
                    position.y = 1.25f;

                    id = graph.GridToId(j, i);
                    isWall = graph.isWall(i, j);

                    if (isWall) continue;

                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, this.transform) as GameObject;
                    matriz[id] = tile.AddComponent<InfluenceTile>();
                    matriz[id].setPosition(i, j);

                    if(i == numRows - 2 || j == numCols - 2 || i == 1 || j == 1)
                        matriz[id].influence = 10;
                    else matriz[id].influence = graph.getVertexObj(id).GetComponent<Vertex>().coste;

                    matriz[id].name = matriz[id].name.Replace("(Clone)", id.ToString());
                }
            }

            UpdateInfluence();
        }

        // Update is called once per frame
        void UpdateInfluence()
        {
            foreach (Vertex v in graph.GetVertices()) {
                if(v.bObstacle)
                v.coste = matriz[v.id].influence;
            }
        }
    }

}