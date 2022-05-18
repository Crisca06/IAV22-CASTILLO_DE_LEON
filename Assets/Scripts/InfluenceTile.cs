namespace UCM.IAV.CristianCastillo
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class InfluenceTile : MonoBehaviour
    {
        public int fil, col;

        public int influence = 1;
        public bool influenceActive;

        Color iniColor;

        public void setPosition(int i, int j) { fil = i; col = j; }

        // Start is called before the first frame update
        void Start()
        {
            iniColor = GetComponent<MeshRenderer>().material.color;
        }

        // Update is called once per frame
        private void Update()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            renderer.material.color = iniColor;
            renderer.material.color = ColorInfluence();
        }

        public Color ColorInfluence()
        {
            Color color;

            if (!influenceActive) return iniColor;

            if (influence > 1)
            {
                color = Color.red;
                color.a = 0.4f;
                return color;
            }
            else return iniColor;
        }
    }

}