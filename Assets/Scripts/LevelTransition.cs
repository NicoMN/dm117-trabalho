using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script responsável pelas transições de fases
public class LevelTransition : MonoBehaviour
{

    //Método que verifica as colisões de tipo IsTrigger
    public void OnTriggerEnter2D(Collider2D collision2D)
    {
        //Se o jogador colidiu com o GameObject LevelTransition, carrega a cena da próxima fase
        if (collision2D.tag == "Player")
        {
            SceneManager.LoadScene("LevelOne");
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
