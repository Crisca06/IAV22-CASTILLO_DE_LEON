
namespace UCM.IAV.CristianCastillo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class InfluencePlayer : MonoBehaviour
    {
        public int bloodAmount = 10;
        public GraphGrid graph;
        public InfluenceMap influenceMap;
        public Text text;
        public int numOfSheeps;

        private ParticleSystem pSys;
        bool win = false;
        bool lose = false;
        bool played = false;

        // Update is called once per frame
        void Update()
        {
            numOfSheeps = graph.numOfSheeps;

            if (Input.GetKey(KeyCode.Space) && bloodAmount > 0)
            {
                int fil_ = graph.roundFloat(transform.position.z);
                int col_ = graph.roundFloat(transform.position.x);
                int id = graph.GridToId(col_, fil_);

                InfluenceTile tile = influenceMap.GetInfluenceTile(id);
                if (tile.influence < 1000) {
                    tile.influence = 1000;
                    bloodAmount--;
                    influenceMap.UpdateInfluence();
                }
            }

            Lose();

            if(win) text.text = "R = Reiniciar | Espacio = Colocar Sangre | C = Perseguir Demonio | Sangre = " + bloodAmount + "\nVICTORIA";
            else text.text = "R = Reiniciar | Espacio = Colocar Sangre | C = Perseguir Demonio | Sangre = " + bloodAmount + "\nTeclas para Cambio de Escena: 0 1 2 3";
            if (lose) text.text = "R = Reiniciar | Espacio = Colocar Sangre | C = Perseguir Demonio | Sangre = " + bloodAmount + "\nDERROTA";
            if(lose && !played) { pSys.Play(); played = true; }
        }
        public void addBlood() { bloodAmount += 10; }
        private void Start()
        {
            pSys = gameObject.GetComponent<ParticleSystem>();
        }

        public void Win() { win = true; }
        public void Lose() { if(!win && bloodAmount <= 0 && numOfSheeps <= 0) lose = true; }
    }
}
