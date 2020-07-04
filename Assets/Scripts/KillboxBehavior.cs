using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KillboxBehavior : MonoBehaviour
{

    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if(collision2D.tag == "Player")
        {
            collision2D.transform.position = GameObject.Find("SpawnPoint").transform.position;
            player = collision2D.gameObject;
            Invoke("ResetGame", 0.5f);
        }
    }

    GameObject GetGameOverMenu()
    {
        return GameObject.Find("PlayerUI").transform.Find("GameOverMenu").gameObject;
    }

    public void Continue()
    {
        var gOver = GetGameOverMenu();
        gOver.SetActive(false);
        player.SetActive(true);
    }

    /// <summary>
    /// Reinicia o jogo
    /// </summary>
    void ResetGame()
    {
        //Faz o menu game over aparecer
        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();

        Button btnContinue = null;

        foreach (var botao in botoes)
        {
            if (botao.gameObject.name.Equals("ContinueBtn"))
            {
                //Salva uma referencia para o botao continue
                btnContinue = botao;
                break;
            }
        }

        if (btnContinue)
        {
#if UNITY_ADS
            //Se o botão continue for clicado chama o metodo ShowReward
            StartCoroutine(ShowContinue(btnContinue));

#else
            //Se não existe ad não precisa mostrar o botão continue
            btnContinue.gameObject.SetActive = false;

#endif
        }

    }
    public IEnumerator ShowContinue(Button botaoContinue)
    {
        var btnText = botaoContinue.GetComponentInChildren<Text>();

        while (true)
        {
            if (UnityAdController.proxTempoReward.HasValue && (DateTime.Now < UnityAdController.proxTempoReward.Value))
            {
                botaoContinue.interactable = false;

                TimeSpan restante = UnityAdController.proxTempoReward.Value - DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);

                btnText.text = contagemRegressiva;

                yield return new WaitForSeconds(1f);
            }
            else
            {
                botaoContinue.interactable = true;
                botaoContinue.onClick.AddListener(UnityAdController.ShowRewardAd);
                UnityAdController.killbox = this;
                btnText.text = "Continue (Ver Ad)";
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
