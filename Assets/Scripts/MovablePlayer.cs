
namespace UCM.IAV.CristianCastillo
{

    using System.Collections;
    using System.Collections.Generic;
    using UCM.IAV.CristianCastillo;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]

    public class MovablePlayer : MonoBehaviour
    {
        private Rigidbody rb;
        List<Vertex> path; // La variable con el camino calculado
        int index = 0; //num baldosa actual

        //Transform de la casilla a la que nos movemos
        private Transform dest;

        [Tooltip("Velocidad maxima de movimiento por IA")]
        public float maxIAVelocity;

        //-------------WASP--------------//
        private Vector3 velocity;
        public float maxVelocity;
        bool freeMovement = true;
        public float auxVelocity = 100;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            path = new List<Vertex>();
        }

        // Update is called once per frame
        void Update()
        {
            if (freeMovement)
            {
                //velocity.x = Input.GetAxis("Horizontal");
                //velocity.z = Input.GetAxis("Vertical");

                //velocity *= maxVelocity;

                ////transform.forward = rb.velocity;
                float x = Input.GetAxis("Horizontal");
                float z = 0;
                if (!(x > 0 || x < 0))
                    z = Input.GetAxis("Vertical");

                Vector3 dir = new Vector3(x, 0, z);
                dir.Normalize();
                dir *= maxIAVelocity;
                //Movemos
                //transform.Translate(dir * Time.deltaTime);
                rb.velocity = dir * Time.deltaTime * auxVelocity;
            }
        }

        public void SetKinematic(bool k)
        {
            rb.isKinematic = k;
        }

        public void SetFreeMovement(bool k)
        {
            freeMovement = k;
        }

        //Asigna una lista de vertices con el camino del player
        public void AddExitPath(List<Vertex> exit)
        {
            transform.forward = Vector3.forward;
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

        //Movimiento del player (control de la IA)
        public void MoveToExit()
        {
            if (!rb.isKinematic || dest == null || path.Count == 0) return;
            //Guardamos ref de la posicion con la y siempre a 0
            Vector3 playerP = transform.position;
            playerP.y = 0;
            Vector3 destP = dest.position;
            destP.y = 0;

            float distance = (destP - playerP).magnitude;
            //Si el jugador esta cerca de la siguiente baldosa pasamos a la proxima baldosa
            if (distance < 0.5f && index >= 0)
            {
                Vertex next = path[index];

                //Cambiamos color de la baldosa por la que hemos pasado(recogemos hilo)
                Renderer r = next.GetComponent<Renderer>();
                r.material.color = next.getColor();

                //Siguiente objetivo si aun queda camino por recorrer
                if (index > 0)
                {
                    index--;
                    next = path[index];
                    dest = next.GetComponent<Transform>();
                }
            }

            //Calculamos dir 
            Vector3 dir = (dest.position - transform.position);

            dir.y = 0; //y no la tenemos en cuenta 
            dir.Normalize();
            dir *= maxIAVelocity;

            //Movemos
            transform.Translate(dir * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            //if (!rb.isKinematic)
            //{
            //    rb.AddForce(velocity, ForceMode.Force);
            //}
        }
    }
}