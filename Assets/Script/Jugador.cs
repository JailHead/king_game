using System.Collections;
using UnityEngine;
using TMPro;

public class Jugador : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] private float fuerzaSalto = 43f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform puntoSuelo;
    [SerializeField] private float radioDeteccion = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

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

    void Update()
    {
        if (gameManager.gameOver) return;

        DetectarSuelo();
        ProcesarSalto();
    }

    private void DetectarSuelo()
    {
        _estaEnSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioDeteccion, capaSuelo);

        if (_animator != null)
        {
            _animator.SetBool("Saltar", !_estaEnSuelo);
        }
    }

    private void ProcesarSalto()
    {        
        if (Input.GetKeyDown(KeyCode.Space) && _estaEnSuelo)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, fuerzaSalto);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_esInvencible) return;

        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            PerderVida();
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

    void OnDrawGizmosSelected()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioDeteccion);
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