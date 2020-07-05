using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{

    [Tooltip("Sistema de particulas para o evento death")]
    public GameObject dethParticle;

    //Método que verifica se o GameObject Frog foi tocado pelo Mouse
    public static void TouchFrog()
    {
        //Cria um Raycast da posição do Mouse
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //Se o Raycast é true, Manda uma mensagem para invocar o método Death()
        if (hit)
        {
            hit.transform.SendMessage("Death", SendMessageOptions.DontRequireReceiver);
        }
    }


    public void Death()
    {
        //Se a particula dethParticle não exister, Instancia a mesma e a destroi depois de 1.0f
        if (dethParticle != null)
        {
            var particulas = Instantiate(dethParticle, transform.position, Quaternion.identity);
            Destroy(particulas, 1.0f);

        }

        //Destroy o GameObject Frog
        Destroy(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Se o botão esquerdo do Mouse está pressionado, o método TouchFrog() é chamado a cada frame
        if (Input.GetMouseButton(0))
        {
            TouchFrog();
        }

    }
}
