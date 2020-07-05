using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerJogo : MonoBehaviour
{
    //Variável responsável por contar a quantidade de gemas que o jogador tem
    public static int gems = 0;

    //Variavel responsavel por salvar o ultimo checkpoint do jogador na fase atual
    public static Vector2 checkpoint;

    //Método que atualiza a quantidade de gemas do jogador e lhe mostra esta quantidade na PlayerUI
    public static void UpdateGems()
    {
        gems++;
        Text gemCount = GameObject.Find("GemTxt").gameObject.GetComponent<Text>();
        var gemCurrent = string.Format("Gems: {0:D1}", gems);
        gemCount.text = gemCurrent;
    }

    //Método que seta o checkpoint do jogador
    public static void SetCheckpoint(Vector2 pos)
    {
        //Se o X da pos passado for maior que o X do checkpoint então este é um checkpoint novo
        if(pos.x > checkpoint.x)
        {
            //Atribui as coordenadas de pos a checkpoint
            checkpoint = pos;
        }
    }

    //Método que retorna o jogador ao seu ultimo checkpoint
    public static void LastCheckpoint()
    {
        //Se o checkpoint conter um valor válido o jogador é retornado a estas coordenadas, caso ao contrário ele é retornado ao ponto inicial da fase (Spawn)
        if(checkpoint != null)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = checkpoint;
        } else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
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
