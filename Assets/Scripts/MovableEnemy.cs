using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Navegacion;
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

    /// ------------------------------------------------------- ///

    List<Vertex> path; //camino calculado
    private Transform dest;
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

    private void Update()
    {
        if (stop)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 5.0f;
                stop = false;
                target.GetComponent<MovablePlayer>().auxVelocity = 100;
                particles.gameObject.SetActive(false);
            }
        }
        else if (follow)
        {
            rb.isKinematic = false;
            Vector3 dir = (target.transform.position - transform.position);
            if (dir.magnitude < 2)
            {
                stop = true;
                target.GetComponent<MovablePlayer>().auxVelocity = 50;
                particles.gameObject.SetActive(true);
            }
            else
                transform.Translate(dir.normalized * Time.deltaTime * 5);
        }
        else if (!follow)
        {
            rb.isKinematic = true;
            MoveToDest();
        }
    }

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

        if(distance < 0.5f && index >= 0)
        {
            Vertex next = path[index];

            //Recoger hilo (en caso de que el minotauro también mostrase el suyo)
            //Renderer r = next.GetComponent<Renderer>();
            //r.material.color = next.getColor();

            //Siguiente objetivo si aun queda camino por recorrer
            if (index > 0)
            {
                index--;
                next = path[index];
                dest = next.GetComponent<Transform>();
            }
            else
            {
                testerGraph.setDestinyNull();
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