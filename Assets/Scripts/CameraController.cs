using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Pega a posição do jogador
    public Transform jogador;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Pega a posição do Jogador a cada frame e faz a camera o seguir
        transform.position = new Vector3(jogador.position.x, jogador.position.y, transform.position.z);
    }
}
