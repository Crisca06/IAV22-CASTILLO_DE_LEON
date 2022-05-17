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
    using System.Collections.Generic;
    
    // Puntos representativos o vértice (común a todos los esquemas de división, o a la mayoría de ellos)
    [System.Serializable]
    public class Vertex : MonoBehaviour
    {
        /// <summary>
        /// Identificador del vértice 
        /// </summary>
        public int id;
        public bool bObstacle;
        public int coste;
        private Color colorPri;
        

        /// <summary>
        /// Vecinos del vértice
        /// </summary>
        public List<Edge> vecinos;

        // Vértice previo
        [HideInInspector]
        public Vertex prev;

        public void setColor(Color m)
        {
            this.GetComponent<Renderer>().material.color = m;
            colorPri = m;
        }

        public Color getColor()
        {
            return colorPri;
        }
    }
}
