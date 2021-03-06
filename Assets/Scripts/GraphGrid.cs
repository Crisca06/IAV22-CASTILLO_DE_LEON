/*    
    Obra original:
        Copyright (c) 2018 Packt
        Unity 2018 Artificial Intelligence Cookbook - Second Edition, by Jorge Palacios
        https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition
        MIT License

    Modificaciones:
        Copyright (C) 2020-2022 Federico Peinado
        http://www.federicopeinado.com

        Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
        Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
        Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.CristianCastillo
{

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class GraphGrid : Graph
    {
        public GameObject obstaclePrefab;
        
        public string mapsDir = "Maps"; // Directorio por defecto
        public string mapName = "arena.map"; // Fichero por defecto
        public bool get8Vicinity = false;
        public float cellSize = 1f;
        public int numOfSheeps = 1;
        [Range(0, Mathf.Infinity)]
        public float defaultCost = 1f;
        [Range(0, Mathf.Infinity)]
        public float maximumCost = Mathf.Infinity;

        int numCols;
        int numRows;
        GameObject[] vertexObjs;
        bool[,] mapVertices;
        public GameObject salida;
        public InfluenceMap influenceMap;
        public Camera mainCam;
        public GameObject sheep;

        public int GridToId(int x, int y)
        {
            return Math.Max(numRows, numCols) * y + x;
        }

        public Vector2 IdToGrid(int id)
        {
            Vector2 location = Vector2.zero;
            location.y = Mathf.Floor(id / numCols);
            location.x = Mathf.Floor(id % numCols);
            return location;
        }

        public bool isWall(int x, int y) { return !mapVertices[x, y]; }

        public bool posIsValid(int x, int y)
        {
            return x >= 0 && x < numCols && y >= 0 && y < numRows;
        }

        private void LoadMap(string filename)
        {
            string path = Application.dataPath + "/" + mapsDir + "/" + filename;
            try
            {
                StreamReader strmRdr = new StreamReader(path);
                using (strmRdr)
                {
                    int j = 0;
                    int i = 0;
                    int id = 0;
                    string line;

                    Vector3 position = Vector3.zero;
                    Vector3 scale = Vector3.zero;
                    line = strmRdr.ReadLine();// non-important line
                    line = strmRdr.ReadLine();// height
                    numRows = int.Parse(line.Split(' ')[1]);
                    line = strmRdr.ReadLine();// width
                    numCols = int.Parse(line.Split(' ')[1]);
                    line = strmRdr.ReadLine();// "map" line in file

                    vertices = new List<Vertex>(numRows * numCols);
                    neighbors = new List<List<Vertex>>(numRows * numCols);
                    costs = new List<List<float>>(numRows * numCols);
                    vertexObjs = new GameObject[numRows * numCols];
                    mapVertices = new bool[numRows, numCols];

                    for (i = 0; i < numRows; i++)
                    {
                        line = strmRdr.ReadLine();
                        for (j = 0; j < numCols; j++)
                        {

                            int rnd = 10000;
                            bool isGround = false;
                            if (line[j] != 'T')
                            {
                                isGround = true;
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
                            }
                            else vertexObjs[id] = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
                            
                            vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
                            Vertex v = vertexObjs[id].AddComponent<Vertex>();
                            
                            v.id = id;
                            v.bObstacle = isGround;

                            if (isGround) {
                                v.coste = 1;
                                v.setColor(Color.gray);
                            }
                            else {
                                v.coste = rnd;
                            }

                            vertices.Add(v);
                            neighbors.Add(new List<Vertex>());
                            costs.Add(new List<float>());
                            float y = vertexObjs[id].transform.localScale.y;
                            scale = new Vector3(cellSize, y, cellSize);
                            vertexObjs[id].transform.localScale = scale;
                            vertexObjs[id].transform.parent = gameObject.transform;
                        }
                    }

                    // now onto the neighbours
                    for (i = 0; i < numRows; i++)
                    {
                        for (j = 0; j < numCols; j++)
                        {
                            SetNeighbours(j, i);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void Load()
        {
            LoadMap(mapName);
            Vertex n = GetNearestVertex(numCols / 2, numRows / 2);
            demon.transform.position = new Vector3(n.transform.position.x, 0.7f, n.transform.position.z);
            Vertex m = GetNearestVertex(1, 1);
            player.transform.position = new Vector3(m.transform.position.x, 1, m.transform.position.z);

            for(int i = 0; i < numOfSheeps; i++)
            {
                int randomX = UnityEngine.Random.Range(0, numCols);
                int randomZ = UnityEngine.Random.Range(0, numRows);

                Vertex v = GetNearestVertex(randomX, randomZ);

                Vector3 pos = v.transform.position;
                pos.y = 1;

                Instantiate(sheep, pos, Quaternion.identity, this.transform);
            }

            influenceMap.initMap(numRows, numCols, cellSize);
            moveCamera();
        }

        protected void SetNeighbours(int x, int y, bool get8 = false)
        {
            int col = x;
            int row = y;
            int i, j;
            int vertexId = GridToId(x, y);
            neighbors[vertexId] = new List<Vertex>();
            costs[vertexId] = new List<float>();
            Vector2[] pos = new Vector2[0];
            if (get8)
            {
                pos = new Vector2[8];
                int c = 0;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col; j++)
                    {
                        pos[c] = new Vector2(j, i);
                        c++;
                    }
                }
            }
            else
            {
                pos = new Vector2[4];
                pos[0] = new Vector2(col, row - 1);
                pos[1] = new Vector2(col - 1, row);
                pos[2] = new Vector2(col + 1, row);
                pos[3] = new Vector2(col, row + 1);
            }
            foreach (Vector2 p in pos)
            {
                i = (int)p.y;
                j = (int)p.x;
                if (i < 0 || j < 0)
                    continue;
                if (i >= numRows || j >= numCols)
                    continue;
                if (i == row && j == col)
                    continue;
                if (!mapVertices[i, j])
                    continue;
                int id = GridToId(j, i);
                neighbors[vertexId].Add(vertices[id]);
                costs[vertexId].Add(defaultCost);
            }
        }
        public int roundFloat(float i)
        {
            if (i <= (int)i + 0.5)
                return (int)i;
            else return (int)i + 1;
        }

        public override Vertex GetNearestVertex(Vector3 position)
        {
            int col = roundFloat(position.x / cellSize);
            int row = roundFloat(position.z / cellSize);
            return GetNearestVertex(col, row);
        }

        public Vertex GetNearestVertex(int col, int row)
        {
            
            Vector2 p = new Vector2(col, row);
            List<Vector2> explored = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(p);
            do
            {
                p = queue.Dequeue();
                col = (int)p.x;
                row = (int)p.y;
                int id = GridToId(col, row);
                if (mapVertices[row, col])
                    return vertices[id];

                if (!explored.Contains(p))
                {
                    explored.Add(p);
                    int i, j;
                    for (i = row - 1; i <= row + 1; i++)
                    {
                        for (j = col - 1; j <= col + 1; j++)
                        {
                            if (i < 0 || j < 0)
                                continue;
                            if (j >= numCols || i >= numRows)
                                continue;
                            if (i == row && j == col)
                                continue;
                            queue.Enqueue(new Vector2(j, i));
                        }
                    }
                }
            } while (queue.Count != 0);
            return null;
        }

        public List<Vertex> Smooth(List<Vertex> path)
        {
            // AQUÍ HAY QUE PONER LA IMPLEMENTACIÓN DEL ALGORITMO DE SUAVIZADO
            // ...
            if (path.Count <= 1) return new List<Vertex>();
            List<Vertex> aux = new List<Vertex>();
            //aux.Add(path[path.Count - 1]);

            Vector2 vi = IdToGrid(path[1].id);
            Vector2 viPrev = IdToGrid(path[0].id);
            Vector2 dir = vi - viPrev;
            aux.Add(path[0]);
            if(path.Count < 2)
            {
                return aux;
            }
            for (int i = 2; i < path.Count; i++)
            {
                vi = IdToGrid(path[i].id);
                viPrev = IdToGrid(path[i - 1].id);
                Vector2 diraux = vi - viPrev;
                if (dir != diraux)
                {
                    aux.Add(path[i - 1]);
                }
                dir = diraux;
            }

            aux.Add(path[path.Count - 1]);
            return aux; //newPath;
        }

        public void pintaCamino(List<Vertex> path, Color color)
        {

            for (int i = 0; i < path.Count; i++)
            {   
                Renderer r = vertexObjs[path[i].id].GetComponent<Renderer>();
                r.material.color = color;
            }
            
        }

        public int getNumRows()
        {
            return numRows;
        }

        public int getNumColumn()
        {
            return numCols;
        }

        public int randomNoObstacle()
        {
            int x = UnityEngine.Random.Range(0, mapVertices.GetLength(1) - 1);
            int y = UnityEngine.Random.Range(0, mapVertices.GetLength(0) - 1);


            Vertex vertex = GetNearestVertex(new Vector3(x, 0, y));
            return vertex.id;
        }

        public int randomVertexWithoutBlood(int zone)
        {
            bool manyIterations = false;
            int i = 0;
            int fil; int col; int filimit; int colimit;
            fil = col = filimit = colimit = 0;

            int mapVerticesLenght0 = mapVertices.GetLength(0);
            int mapVerticesLenght1 = mapVertices.GetLength(1);

            switch (zone)
            {
                case 0:
                    fil = 0; col = 0; filimit = mapVerticesLenght0 / 2; colimit = mapVerticesLenght1 / 2;
                    break;
                case 1:
                    fil = 0; col = mapVerticesLenght1 / 2; filimit = mapVertices.GetLength(0) / 2; colimit = mapVertices.GetLength(1) - 1;
                    break;
                case 2:
                    fil = mapVerticesLenght0 / 2; col = 0; filimit = mapVertices.GetLength(0) - 1; colimit = mapVertices.GetLength(1) / 2;
                    break;
                case 3:
                    fil = mapVerticesLenght0 / 2; col = mapVerticesLenght1 / 2; filimit = mapVertices.GetLength(0) - 1; colimit = mapVertices.GetLength(1) - 1;
                    break;
                default:
                    break;
            }

            while (!manyIterations)
            {
                int x = UnityEngine.Random.Range(col, colimit);
                int y = UnityEngine.Random.Range(fil, filimit);

                Vertex v = GetNearestVertex(new Vector3(x, 0, y));
                if (v.coste < 1000) return v.id;

                i++;
                
                if (i > vertices.Count) manyIterations = true;
            }

            foreach (Vertex v in vertices)
                if (v.coste < 1000) return v.id;

            return -1;
        }

        public int randomVertexInArea(float fil, float col, int radius, bool blood)
        {
            int fil_ = roundFloat(fil);
            int col_ = roundFloat(col);

            List<Vertex> v = new List<Vertex>();

            for (int i = fil_ - radius; i < fil_ + radius; i++)
            {
                for (int j = col_ - radius; j < col + radius; j++)
                {
                    int id = GridToId(j, i);
                    if (id < 0) continue;
                    if (i == fil_ && j == col_) continue;

                    if ((!blood && vertices[id].bObstacle) || (vertices[id].coste < 1000 && vertices[id].bObstacle)) v.Add(vertices[id]);
                }
            }

            if(v.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, v.Count);
                return v[random].id;
            }

            return -1;
        }

        public GameObject getVertexObj(int id)
        {
            return vertexObjs[id];
        }

        public void moveCamera()
        {
            mainCam.transform.position = new Vector3(numCols / 2, (numCols + numRows) / 5 + 24f, numRows / 2);
        }
    }
}
