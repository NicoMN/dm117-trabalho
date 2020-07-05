using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    private static MusicController musicController = null;

    //Método responsável pela música de fundo
    private void Awake()
    {
        //Verifica se musicController é diferente de null, se sim destroy o GameObject, se não chama o GameObject responsável pela música de fundo,
        // e o seta como DontDestroyOnLoad para que possa persistir entre fases
        if (musicController != null)
        {
            Destroy(gameObject);
        }
        else
        {
            musicController = this;
            GameObject.DontDestroyOnLoad(gameObject);
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
