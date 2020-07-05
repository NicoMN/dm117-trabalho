using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseComp : MonoBehaviour
{

    public static bool pausado;

    public GameObject menuPausePanel;

    /// <summary>
    /// Metodo para reiniciar a scene
    /// </summary>
    public void Restart()
    {

        Pause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ControllerJogo.gems = 0;
        ControllerJogo.checkpoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;
    }

    /// <summary>
    /// Metodo para pausar o jogo
    /// </summary>
    public void Pause(bool isPausado)
    {
        pausado = isPausado;

        //Se o jogo estiver pausado, timescale recebe 0
        Time.timeScale = (pausado) ? 0 : 1;

        //Habilita/Desabilita o menu pause
        menuPausePanel.SetActive(pausado);

    }

    /// <summary>
    /// Metodo para carregar uma scene
    /// </summary>
    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        //UnityAdController.InitializeAd();
        //pausado = false;
        //Pause(false);

        //Pause() não é necessário para Mobile
#if !UNITY_ADS
        Pause(false);
#endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
