using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UCM.IAV.CristianCastillo;

public class SceneSelector : MonoBehaviour
{
    public TesterGraph testergraph;
    void Update()
    {
        if (Input.inputString != "")
        {
            string key = Input.inputString.ToLower();
            switch (key)
            {
                case "r":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case "z":
                    testergraph.smoothPath = !testergraph.smoothPath;
                    break;
                case "0":
                    SceneManager.LoadScene(0);
                    break;
                case "1":
                    SceneManager.LoadScene(1);
                    break;
                case "2":
                    SceneManager.LoadScene(2);
                    break;             
                case "3":
                    SceneManager.LoadScene(3);
                    break;
                default:
                    break;
            }
        }
    }
}
