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
        [SerializeField]
        GameObject wallTilePrefab;


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
            string line;

            Vector3 position = Vector3.zero;
            Vector3 scale = Vector3.zero;

            vertices = graph.GetVertices();

            for (i = 0; i < numRows; i++)
            {
                for (j = 0; j < numCols; j++)
                {
                    bool isWall = false;

                    position.x = j * cellSize;
                    position.z = i * cellSize;

                    id = graph.GridToId(j, i);
                    isWall = graph.isWall(i, j);

                    if (!isWall)
                    {
                        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity) as GameObject;
                        matriz[id] = tile.GetComponent<InfluenceTile>();
                        matriz[id].setPosition(i, j);
                    }
                    else
                    {
                        GameObject tile = Instantiate(wallTilePrefab, position, Quaternion.identity) as GameObject;
                        matriz[id] = tile.GetComponent<InfluenceTile>();
                        matriz[id].setPosition(i, j);
                    }

                    matriz[id].name = matriz[id].name.Replace("(Clone)", id.ToString());
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}