using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    public Renderer fondo;
    public GameObject Piedra1;
    public GameObject Piedra2;
    public GameObject Serpiente;
    public GameObject col;
    public GameObject Moneda;

    [Header("Configuración")]
    public float velocidad = 2f;
    public int maxSerpientes = 3;

    [Header("Estado del Juego")]
    public bool gameOver = false;

    public List<GameObject> cols;
    public List<GameObject> obstaculos;
    public List<GameObject> serpientes;
    public List<GameObject> monedas;

    private Vector3 _movimientoIzquierda = Vector3.left;
    private Vector2 _scrollTextura = new Vector2(0.02f, 0);
    private float _tiempoSiguienteMoneda = 0f;
    private float _intervaloSpawnMonedas = 3f;

    [Header("Control de Velocidad")]
    public float velocidadMinima = 1f;
    public float velocidadMaxima = 5f;
    public float aceleracion = 2f;

    void Start()
    {
        InicializarColumnas();
        InicializarObstaculos();
        InicializarSerpientes();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SalirJuego();
            return;
        }

        if (gameOver) return;

        ControlarVelocidad();
        MoverFondo();
        MoverColumnas();
        MoverObstaculos();
        MoverSerpientes();
        SpawnearMonedas();
        MoverMonedas();
    }

    private void InicializarColumnas()
    {
        for (int i = 0; i < 21; i++)
        {
            cols.Add(Instantiate(col, new Vector2(-10 + i, -3), Quaternion.identity));
        }
    }

    private void InicializarObstaculos()
    {
        obstaculos.Add(Instantiate(Piedra1, new Vector2(14, -2), Quaternion.identity));
        obstaculos.Add(Instantiate(Piedra2, new Vector2(18, -2), Quaternion.identity));
    }

    private void InicializarSerpientes()
    {
        serpientes.Add(Instantiate(Serpiente, new Vector2(22, -2), Quaternion.identity));
    }

    private void MoverFondo()
    {
        fondo.material.mainTextureOffset += _scrollTextura * Time.deltaTime;
    }

    private void MoverColumnas()
    {
        for (int i = 0; i < cols.Count; i++)
        {
            if (cols[i].transform.position.x <= -10)
            {
                cols[i].transform.position = new Vector3(10, -3, 0);
            }
            cols[i].transform.position += _movimientoIzquierda * Time.deltaTime * velocidad;
        }
    }

    private void MoverObstaculos()
    {
        for (int i = 0; i < obstaculos.Count; i++)
        {
            if (obstaculos[i].transform.position.x <= -10)
            {
                float randomX = Random.Range(11f, 18f);
                obstaculos[i].transform.position = new Vector3(randomX, -2, 0);
            }
            obstaculos[i].transform.position += _movimientoIzquierda * Time.deltaTime * velocidad;
        }
    }

    private void MoverSerpientes()
    {
        for (int i = 0; i < serpientes.Count; i++)
        {
            if (serpientes[i] != null)
            {
                if (serpientes[i].transform.position.x <= -10)
                {
                    float randomX = Random.Range(11f, 25f);
                    serpientes[i].transform.position = new Vector3(randomX, -2, 0);
                }
                serpientes[i].transform.position += _movimientoIzquierda * Time.deltaTime * velocidad;
            }
        }
    }

    public void ActivarGameOver()
    {
        gameOver = true;
        Time.timeScale = 0f;
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    private void ControlarVelocidad()
    {
        // Acelerar con Flecha Arriba o W
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            velocidad += aceleracion * Time.deltaTime;
        }
        // Desacelerar con Flecha Abajo o S
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            velocidad -= aceleracion * Time.deltaTime;
        }

        // Limitar velocidad entre min y max
        velocidad = Mathf.Clamp(velocidad, velocidadMinima, velocidadMaxima);
    }

    private void SpawnearMonedas()
    {
        _tiempoSiguienteMoneda -= Time.deltaTime;

        if (_tiempoSiguienteMoneda <= 0f)
        {
            float randomX = Random.Range(12f, 20f);
            float randomY = Random.Range(-1.8f, -0.8f);
            GameObject nuevaMoneda = Instantiate(Moneda, new Vector2(randomX, randomY), Quaternion.identity);
            monedas.Add(nuevaMoneda);

            _tiempoSiguienteMoneda = _intervaloSpawnMonedas;
        }
    }

    private void MoverMonedas()
    {
        for (int i = monedas.Count - 1; i >= 0; i--)
        {
            if (monedas[i] == null)
            {
                monedas.RemoveAt(i);
                continue;
            }

            if (monedas[i].transform.position.x <= -10)
            {
                Destroy(monedas[i]);
                monedas.RemoveAt(i);
                continue;
            }

            monedas[i].transform.position += _movimientoIzquierda * Time.deltaTime * velocidad;
        }
    }

    public void SalirJuego()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}