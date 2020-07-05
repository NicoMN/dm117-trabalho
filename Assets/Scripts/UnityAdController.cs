using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class UnityAdController : MonoBehaviour
{

    public static bool showAds = true;

    //Tipo DateTime que pode ser null
    public static DateTime? proxTempoReward = null;

    public static string gameId = "3692831";

    public static KillboxBehavior killbox;

    /// <summary>
    /// Metodo para mostrar ad com recompensa
    /// </summary>
    public static void ShowRewardAd()
    {
#if UNITY_ADS

        proxTempoReward = DateTime.Now.AddSeconds(15);
        if(Advertisement.IsReady())
        {
            //Pausar o jogo
            MenuPauseComp.pausado = true;
            Time.timeScale = 0f;

            //Outra forma de criar o ShowOptions e setar o callback
            var opcoes = new ShowOptions
            {
                resultCallback = TratarMostrarResultado
            };

        Advertisement.Show(opcoes);
        }
#endif
    }

/// <summary>
/// Metodo para tratar o resultado com reward (recompensa)
/// </summary>


#if UNITY_ADS
    public static void TratarMostrarResultado(ShowResult result)
    {
        switch(result)
        {

        case ShowResult.Finished:
            //Anuncio mostrado, continue o jogo
            killbox.Continue();
            break;

        case ShowResult.Skipped:
            Debug.Log("Ad pulado. Nada feito");
            break;

        case ShowResult.Failed:
            Debug.LogError("Erro no Ad. Nada feito");
            break;

        }

        //Resume o Jogo
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;

    }
#endif

public static void InitializeAd()
    {
        Advertisement.Initialize(gameId, false);
    }

    public static void ShowAd()
    {

#if UNITY_ADS


        //Opcoes para o Ad
        ShowOptions opcoes = new ShowOptions();
        opcoes.resultCallback = Unpause;

        if(Advertisement.IsReady())
        {
            //Mostra o anuncio
            Advertisement.Show(opcoes);
        }

#endif
    }

    public static void Unpause(ShowResult result)
    {
        //Quando o anuncio acabar, saia do modo pausado
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }


    // Start is called before the first frame update
    void Start()
    {
        showAds = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
