using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    public float fuerzaSalto = 25;

    public GameManager gameManager;

    private Rigidbody2D rigidbody2D;

    public bool enSuelo = false;

    public TextMeshProUGUI textoGameOver;

    public int vidas = 3;
    public int puntos = 0;
    public Text textoVidas;
    public Image pantallaNegra;


    private Animator animator;
    void Start()
    {

        if (pantallaNegra != null)
        {
            pantallaNegra.gameObject.SetActive(false); // Asegurar que esté activo
        }

        if (textoGameOver != null)
        {
            textoGameOver.enabled = false; // Deshabilitar al inicio
        }




        

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        float mover = Input.GetAxis("Horizontal");
        Vector2 velocidadActual = rigidbody2D.velocity;
        rigidbody2D.velocity = new Vector2(mover * 5f, velocidadActual.y);




        if (Input.GetKeyDown(KeyCode.Space) && enSuelo == true)
        {
            animator.SetBool("Saltar", true);
            rigidbody2D.AddForce(new Vector2(0, (fuerzaSalto * 10)));
            enSuelo = false;
        }

        if (mover > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mira a la derecha
        }
        else if (mover < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mira a la izquierda
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Suelo")
        {
            animator.SetBool("Saltar", false);
            enSuelo = true;
        }

        if(collision.gameObject.tag == "Obstaculo")
        {
            gameManager.gameOver = true;
            perderVida();
            Debug.Log("vidas: " + vidas);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Serpiente")
        {
            puntos++;
            Debug.Log("Puntos: " + puntos);

            // Destruir la serpiente
            Destroy(collision.gameObject);
        }
    }

    void perderVida()
    {
        vidas--;

        if (vidas == 0)
        {
            GameOver();
        }
    }


    void GameOver()
    {
        pantallaNegra.gameObject.SetActive(true); // Asegurar que esté activo
        textoGameOver.enabled = true; // Deshabilitar al inicio
    }
}
