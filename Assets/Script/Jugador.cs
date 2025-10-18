using System.Collections;
using UnityEngine;
using TMPro;

public class Jugador : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] private float fuerzaSalto = 43f;

    [Header("Sistema de Vidas")]
    [SerializeField] private int vidasMaximas = 3;
    [SerializeField] private float tiempoInvencibilidad = 1.5f;

    [Header("Sistema de Monedas")]
    [SerializeField] private TextMeshProUGUI textoMonedas;

    private int _monedasRecogidas = 0;

    [Header("Referencias")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI textoVidas;
    [SerializeField] private GameObject pantallaNegra;
    [SerializeField] private TextMeshProUGUI textoGameOver;

    // Componentes cacheados
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Estado
    private int _vidasActuales;
    private bool _estaEnSuelo;
    private bool _esInvencible;
    private bool _puedeSaltar = true;
    private int _contadorSuelo = 0;
    private Color _colorOriginal;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _colorOriginal = _spriteRenderer.color;
    }

    void Start()
    {
        _vidasActuales = vidasMaximas;
        ActualizarUIVidas();
        ActualizarUIMonedas();
    }

    private void ActualizarAnimacion()
    {
        if (_animator != null)
        {
            _animator.SetBool("Saltar", !_puedeSaltar);
        }
    }

    void Update()
    {
        if (gameManager.gameOver) return;

        ProcesarSalto();
        ActualizarAnimacion();
    }

    private void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _puedeSaltar)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, fuerzaSalto);
            _puedeSaltar = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar suelo para permitir salto
        if (collision.gameObject.CompareTag("Suelo"))
        {
            _contadorSuelo++;
            _puedeSaltar = true;
        }

        // Detectar daño por obstáculos
        if (_esInvencible) return;

        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            PerderVida();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            _contadorSuelo--;
            if (_contadorSuelo <= 0)
            {
                _contadorSuelo = 0;
                _puedeSaltar = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Serpiente"))
        {
            MuerteInstantanea();
        }
    }

    private void PerderVida()
    {
        _vidasActuales--;
        ActualizarUIVidas();

        if (_vidasActuales <= 0)
        {
            MuerteInstantanea();
        }
        else
        {
            StartCoroutine(ActivarInvencibilidad());
        }
    }

    private void MuerteInstantanea()
    {
        _vidasActuales = 0;
        ActualizarUIVidas();

        if (pantallaNegra != null)
        {
            pantallaNegra.SetActive(true);
        }

        if (textoGameOver != null)
        {
            textoGameOver.text = "GAME OVER\n\nMonedas Recolectadas: " + _monedasRecogidas;
        }

        gameManager.ActivarGameOver();
    }

    private IEnumerator ActivarInvencibilidad()
    {
        _esInvencible = true;

        // Parpadeo visual
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < tiempoInvencibilidad)
        {
            _spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f); // Rojo semi-transparente
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = _colorOriginal;
            yield return new WaitForSeconds(0.1f);
            tiempoTranscurrido += 0.2f;
        }

        _esInvencible = false;
        _spriteRenderer.color = _colorOriginal;
    }

    private void ActualizarUIVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + _vidasActuales;
        }
    }

    public void RecogerMoneda(int valor)
    {
        _monedasRecogidas += valor;
        ActualizarUIMonedas();
    }

    private void ActualizarUIMonedas()
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = "Monedas: " + _monedasRecogidas;
        }
    }

    public int ObtenerMonedas()
    {
        return _monedasRecogidas;
    }
}