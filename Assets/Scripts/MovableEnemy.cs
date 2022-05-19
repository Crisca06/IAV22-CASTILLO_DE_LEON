namespace UCM.IAV.CristianCastillo
{
    using System.Collections;
    using System.Collections.Generic;
    using UCM.IAV.CristianCastillo;
    using Unity.VisualScripting;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class MovableEnemy : MonoBehaviour
    {
        public Transform playerDetector;
        public float speed = 10;
        public ParticleSystem particles;

        private bool follow;
        private Rigidbody rb;
        private GameObject target;
        private float timer = 5.0f;
        private bool stop = false;
        private Renderer rend;

        public GraphGrid graphGrid;
        public TesterGraph testerGraph;
        public bool trapped = false;

        /// ------------------------------------------------------- ///

        List<Vertex> path; //camino calculado
        public Transform dest;
        int index = 0;

        [Tooltip("Velocidad maxima de movimiento por IA")]
        public float maxIAVelocity;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = GameObject.FindGameObjectWithTag("Player");
            rend = gameObject.GetComponent<Renderer>();

            path = new List<Vertex>();
        }

        public int randomVertex() { return graphGrid.randomVertexWithoutBlood(); }
        public bool destinyIsNull() { return dest == null; }
        public void moveToVertex(int id)
        {
            if (id == -1) {
                trapped = true;
                return;
            }
         
            GameObject dstEnemy = graphGrid.getVertexObj(id);
            List<Vertex> pathEnemy = graphGrid.GetPathAstar(this.gameObject, dstEnemy, null);
            AddExitPath(pathEnemy);
        }

        private void Update()
        { }

        public void setFollow(bool fol)
        {
            follow = fol;
        }

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

                //Despintar camino
                Renderer r = next.GetComponent<Renderer>();
                r.material.color = next.getColor();

                //Siguiente objetivo si aun queda camino por recorrer
                if (index > 0)
                {
                    index--;
                    next = path[index];
                    dest = next.GetComponent<Transform>();
                }
                else {
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
            playerDetector.transform.rotation = Quaternion.LookRotation(dir);

            if(index == 0) dest = null;
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
    }
}