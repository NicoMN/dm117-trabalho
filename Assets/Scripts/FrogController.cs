using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{

    [Tooltip("Particle system for the death event")]
    public GameObject dethParticle;

    public static void TouchFrog()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit)
        {
            hit.transform.SendMessage("Death", SendMessageOptions.DontRequireReceiver);
        }
    }


    public void Death()
    {
        if (dethParticle != null)
        {
            var particulas = Instantiate(dethParticle, transform.position, Quaternion.identity);
            Destroy(particulas, 1.0f);

        }

        Destroy(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TouchFrog();
        }

    }
}
