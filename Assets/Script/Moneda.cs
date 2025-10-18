using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    [SerializeField] private int valor = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Jugador jugador = other.GetComponent<Jugador>();
            if (jugador != null)
            {
                jugador.RecogerMoneda(valor);
                Destroy(gameObject);
            }
        }
    }
}