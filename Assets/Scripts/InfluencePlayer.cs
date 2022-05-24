
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

        bool win = false;
        bool lose = false;

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

            text.text = "R = Reiniciar | Espacio = Colocar Sangre | C = Perseguir Demonio | Sangre = " + bloodAmount;

            if(win) text.text = "R = Reiniciar | Espacio = Colocar Sangre | C = Perseguir Demonio | Sangre = " + bloodAmount + "\nVICTORIA";
            if(lose) text.text = "R = Reiniciar | Espacio = Colocar Sangre | C = Perseguir Demonio | Sangre = " + bloodAmount + "\nDERROTA";
        }
        public void addBlood() { bloodAmount += 10; }

        public void Win() { win = true; }
        public void Lose() { if(!win && bloodAmount <= 0 && numOfSheeps <= 0) lose = true; }
    }
}
