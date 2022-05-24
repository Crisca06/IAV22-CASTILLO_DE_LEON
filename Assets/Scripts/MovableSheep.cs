namespace UCM.IAV.CristianCastillo
{
    using System.Collections;
    using System.Collections.Generic;
    using UCM.IAV.CristianCastillo;
    using Unity.VisualScripting;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class MovableSheep : MonoBehaviour
    {
        public float speed = 10;
        public ParticleSystem particles;

        private Rigidbody rb;
        private Renderer rend;
        List<Vertex> pathSheep;

        private GraphGrid graphGrid;

        [Tooltip("Radio del merodeo de la IA")]
        public int radius = 5;
        int fil; int col;

        bool picked = false;

        /// ------------------------------------------------------- ///

        List<Vertex> path; //camino calculado
        public Transform dest;
        int index = 0;

        [Tooltip("Velocidad maxima de movimiento por IA")]
        public float maxIAVelocity;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rend = gameObject.GetComponent<Renderer>();
            graphGrid = GameObject.FindGameObjectWithTag("GraphGrid").GetComponent<GraphGrid>();
            path = new List<Vertex>();
            particles = gameObject.GetComponent<ParticleSystem>();
        }

        public void setPosition(int f, int c) { fil = f; col = c; }

        public int randomVertexInZone() { return graphGrid.randomVertexInArea(fil, col, radius, false); }

        public bool destinyIsNull() { return dest == null; }
        public void moveToVertex(int id)
        {

            GameObject dstSheep = graphGrid.getVertexObj(id);
            pathSheep = graphGrid.GetPathAstar(this.gameObject, dstSheep, null);

            AddExitPath(pathSheep);
        }

        private void Update()
        { }

        public void MoveToDest()
        {
            //Si el cuerpo es cinematico y no hay destino salimos de la funcion
            if (!rb.isKinematic || dest == null) return;

            //Guardamos ref de la posicion con la y siempre a 0 del destio y
            //de la pos inicial del enemigo
            Vector3 enemyP = transform.position;
            enemyP.y = 0;
            Vector3 destP = dest.position;
            destP.y = 0;

            //calculamos distancia recorrido desde la pos actual
            float distance = (destP - enemyP).magnitude;

            if (distance < 0.5f && index >= 0)
            {
                Vertex next = path[index];

                //Siguiente objetivo si aun queda camino por recorrer
                if (index > 0)
                {
                    index--;
                    next = path[index];
                    dest = next.GetComponent<Transform>();
                }
                else
                {
                    index = 0;
                    path.Clear();
                }
            }
            //Calculamos dir 
            Vector3 dir = (dest.position - transform.position);

            dir.y = 0; //y no la tenemos en cuenta 
            dir.Normalize();
            dir *= maxIAVelocity;

            //Movemos
            transform.Translate(dir * Time.deltaTime);

            if (index == 0) dest = null;
        }

        public void AddExitPath(List<Vertex> exit)
        {
            path = exit;
            if (exit.Count > 0)
            {
                index = path.Count - 1;
                dest = path[index].GetComponent<Transform>();
            }
            else
            {
                dest = null;
                index = 0;
            }
        }

        public void DestNull() { dest = null; }

        public bool isPicked() { return picked; }

        public void playParticles() { particles.Play(); }

        public void DestroyThis() { Destroy(this.gameObject); }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !picked) {
                picked = true;
                other.GetComponent<InfluencePlayer>().addBlood();
                graphGrid.numOfSheeps--;
            }
        }
    }
}