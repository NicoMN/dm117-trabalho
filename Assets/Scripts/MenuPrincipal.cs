using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    void Start()
    {
        UnityAdController.InitializeAd();
    }

    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
        ControllerJogo.gems = 0;

#if UNITY_ADS

        if (UnityAdController.showAds)
        {
            UnityAdController.ShowAd();
        }

    #endif

    }

}
