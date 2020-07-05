using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorController : MonoBehaviour
{
    //Referência ao componente Rigidbody 2D
    private Rigidbody2D rb;

    //Referência ao componente BoxCollider2D
    private BoxCollider2D boxC;

    //Referência ao componente Anim que é encarregado das animações do jogador
    [SerializeField] private Animator anim;

    //Referência a um LayerMask especifico que será utilizado no método IsGrounded()
    [SerializeField] private LayerMask platformLayerMask;

    [SerializeField]
    [Tooltip("A velocidade que o jogador se desloca")]
    [Range(0, 10)]
    private float speed = 5f;

    [SerializeField]
    [Tooltip("A altura do salto do jogador")]
    [Range(0, 50)]
    private float jumpHeight = 20f;

    [SerializeField]
    [Tooltip("A força em que o jogador é jogada ao sofrer dano")]
    [Range(0, 20)]
    private float hurtForce = 10f;

    //Enumerator que contém todos os estados do jogador
    private enum State {idle, running, jumping, falling, hurt };

    //Seta o estado do jogador como o estado idle
    private State state = State.idle;

    //Método que verifica se o jogador está no chão
    private bool IsGrounded()
    {
        //Cria um Raycast2D que começa no centro do jogador e vai para o chão e verifica se o mesmo está colidindo com o platformLayerMask
        float extraHeightText = .05f;
        RaycastHit2D raycastHit = Physics2D.Raycast(boxC.bounds.center, Vector2.down, boxC.bounds.extents.y + extraHeightText, platformLayerMask);
        Color rayColor;

        if (raycastHit.collider != null) {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxC.bounds.center, Vector2.down * (boxC.bounds.extents.y + extraHeightText));

        //Retorna true ou false se a colisão do RayCast aconteceu
        return raycastHit.collider != null;
    }

    private void Movement()
    {
        float hDirecao = Input.GetAxis("Horizontal");

        //Se o jogador apertar a tecla 'A' a personagem irá se deslocar para a esquerda
        if (hDirecao < 0)
        {
            //Desloca o jogador e muda a direção do sprite
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }

        //Se o jogador apertar a tecla 'D' a personagem irá se deslocar para a direita
        else if (hDirecao > 0)
        {
            //Desloca o jogador e muda a direção do sprite
            rb.velocity = new Vector2(speed, rb.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }

        //Se o jogador apertar a tecla 'Espaço' irá pular
        //Quando o jogador aperta espaço também é verificado se IsGrounded() é true, se sim o jogador está no chão e pode pular
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    //Realiza a ação de pular e muda o state para o State jumping
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        state = State.jumping;
    }

    //Método resposável por verificar e setar os estados do jogador para que sejam usados pelas animações dos Sprites
    private void AnimationState()
    {
        if (state == State.jumping)
        {
            // Quando a velocidade de Y do jogador for menor do que .1f, trocar o seu State para o State falling (caindo)
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }

        //Se o State for de falling checa se ele está em contato com o chão, se sim troca o State para idle
        else if (state == State.falling)
        {
            if (IsGrounded())
            {
                state = State.idle;
            }
        }


        //Se o jogador estiver no State de hurt (machucado) ele checa se sua velocidade X é menor que .1f, se sim volta o State para idle
        else if (state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        //Se a velocidade de X so jogador for superior a 2f troca seu State para running
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }

        //Se nada acima aconteceu, muda o State do jogador para idle
        else
        {
            state = State.idle;
        }

    }

    //Método que toca um efeito sonoro quando o jogador obtém uma gema, e depois a destroi.
    public IEnumerator PlaySound(GameObject gem)
    {
        gem.GetComponent<AudioSource>().Play();
        yield return new WaitWhile(() => gem.GetComponent<AudioSource>().isPlaying);
        Destroy(gem.gameObject);
    }

    //Método que verifica se o jogador colidiu com Colliders 2D denifidos como IsTrigger
    private void OnTriggerEnter2D(Collider2D trgCollision)
    {
        //Se o jogador colidiu com o collider com um GameObject de tag "Collectible", atualiza sua quantidade de gemas e chama a co-Rotina PlaySound()
        if(trgCollision.tag == "Collectible")
        {
            StartCoroutine(PlaySound(trgCollision.gameObject));
            ControllerJogo.UpdateGems();     
        }

        //Se o jogador colidiu com um GameObject com tag "Checkpoint", chama o método SetCheckpoint() com as coordenadas do Checkpoint tocado
        if (trgCollision.tag == "Checkpoint")
        {
            ControllerJogo.SetCheckpoint(trgCollision.transform.position);
        }
    }

    //Método que verifica se o jogador colidiu com Colliders 2D
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Verifica se o jogador colidiu com o GameObject com tag "Enemy"
        if(collision2D.gameObject.tag == "Enemy")
        {

            //Pega o Script do inimigo Frog
            FrogController frog = collision2D.gameObject.GetComponent<FrogController>();

            //Se o jogador estava em State falling quando colidiu com o inimigo, chama o método Death() de FrogController e faz o jogador pular do inimigo
            if(state == State.falling)
            {
                frog.Death();
                Jump();
            }

            //Se não, troca o State do jogador para hurt e o joga para trás
            else
            {
                state = State.hurt;

                if (collision2D.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Inicializar o Unity Ads
        UnityAdController.InitializeAd();

        //Seta a posição do jogador nas coordenadas do GameObject com tag "Spawn"
        gameObject.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;

        //Obter acesso ao componente Rigidbody associado a esse GO (Game Object) 
        rb = GetComponent<Rigidbody2D>();

        //Obter acesso ao componente BoxCollider2D associado a esse GO (Game Object) 
        boxC = GetComponent<BoxCollider2D>();

        //Obter acesso ao componente Animator associado a esse GO (Game Object) 
        anim = GetComponent<Animator>();      
    }

    // Update is called once per frame
    void Update()
    {
        //Se o jogador estiver em State hurt restringe seus movimentos, senão chama o método responsável pelos movimentos
        if(state != State.hurt)
        {
        Movement();
        }

        //Chama o método responsável pelos estados de animação do jogador
        AnimationState();
        anim.SetInteger("state", (int)state);
    }


}
