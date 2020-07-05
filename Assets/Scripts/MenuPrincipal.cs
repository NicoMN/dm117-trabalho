using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    void Start()
    {

        //Inicializador para o Unity Ads
        UnityAdController.InitializeAd();
    }

    public void CarregaScene(string nomeScene)
    {
        //Carrega a Scene Tutorial e seta a quantidade de gemas do jogador para 0
        SceneManager.LoadScene(nomeScene);
        ControllerJogo.gems = 0;

#if UNITY_ADS

        // Mostra um Ad quando o jogador começar o Jogo
        if (UnityAdController.showAds)
        {
            UnityAdController.ShowAd();
        }

    #endif

    }

}
