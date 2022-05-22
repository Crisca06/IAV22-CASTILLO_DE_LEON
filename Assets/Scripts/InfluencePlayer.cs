
namespace UCM.IAV.CristianCastillo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class InfluencePlayer : MonoBehaviour
    {
        public int bloodAmount = 10;
        public GraphGrid graph;
        public InfluenceMap influenceMap;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKey(KeyCode.Space) && bloodAmount > 0)
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
        }
    }
}
