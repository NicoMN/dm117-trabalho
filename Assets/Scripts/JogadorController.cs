using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorController : MonoBehaviour
{
    //Referência ao componente Rigidbody 2D
    private Rigidbody2D rb;

    private BoxCollider2D boxC;

    [SerializeField] private Animator anim;

    [SerializeField] private LayerMask platformLayerMask;

    [SerializeField]
    [Tooltip("A velocidade que a personagem se desloca")]
    [Range(0, 10)]
    private float speed = 5f;

    [SerializeField]
    [Tooltip("A altura do salto da personagem")]
    [Range(0, 50)]
    private float jumpHeight = 20f;

    [SerializeField]
    [Tooltip("A força em que a personagem é jogada ao sofrer dano")]
    [Range(0, 20)]
    private float hurtForce = 10f;

    private enum State {idle, running, jumping, falling, hurt };

    private State state = State.idle;

    private bool isGrounded()
    {
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
        return raycastHit.collider != null;
    }

    private void Movement()
    {
        float hDirecao = Input.GetAxis("Horizontal");

        //Se o jogador apertar a tecla 'A' a personagem irá se deslocar para a esquerda
        if (hDirecao < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }

        //Se o jogador apertar a tecla 'D' a personagem irá se deslocar para a direita
        else if (hDirecao > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;

        }

        //Se o jogador apertar a tecla 'Espaço' a personagem irá realizar um salto
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }

        else if (state == State.falling)
        {
            if (isGrounded())
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }

    }

    private void OnTriggerEnter2D(Collider2D trgCollision)
    {
        if(trgCollision.tag == "Collectible")
        {
            Destroy(trgCollision.gameObject);
            ControllerJogo.UpdateGems();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.gameObject.tag == "Enemy")
        {

            FrogController frog = collision2D.gameObject.GetComponent<FrogController>();

            if(state == State.falling)
            {
                frog.Death();
                Jump();
            }
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
        UnityAdController.InitializeAd();

        gameObject.transform.position = GameObject.Find("SpawnPoint").transform.position;

        //Obter acesso ao componente Rigidbody associado a esse GO (Game Object) 
        rb = GetComponent<Rigidbody2D>();

        boxC = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();      
    }

    // Update is called once per frame
    void Update()
    {
        if(state != State.hurt)
        {
        Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state);
    }


}
