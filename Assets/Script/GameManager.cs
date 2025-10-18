using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Renderer fondo;
    public GameObject Piedra1;
    public GameObject Piedra2;
    public GameObject Serpiente;

    public GameObject col;

    public float velocidad = 2;
    public List<GameObject> cols;
    public List<GameObject> obstaculos;
    public List<GameObject> serpientes;

    public bool gameOver = false;
    public int maxSerpientes = 3;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 21; i++)
        {
           cols.Add(Instantiate(col, new Vector2(-10 + i, -3), Quaternion.identity));
        }

        obstaculos.Add(Instantiate(Piedra1, new Vector2(14, -2), Quaternion.identity));
        obstaculos.Add(Instantiate(Piedra2, new Vector2(18, -2), Quaternion.identity));
        serpientes.Add(Instantiate(Serpiente, new Vector2(22, -2), Quaternion.identity));
    }

        // Update is called once per frame
        void Update()
    {
        //fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.02f, 0) * Time.deltaTime; 
        //for(int i = 0; i < cols.Count; i++)
        //{
        //    if (cols[i].transform.position.x <= -10)
        //    {
        //        cols[i].transform.position = new Vector3(10, -3, 0);
        //    }
        //    cols[i].transform.position = cols[i].transform.position + new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
        //}

        for (int i = 0; i < obstaculos.Count; i++)
        {
            if (obstaculos[i].transform.position.x <= -10)
            {
                float randomObs = Random.Range(11, 18);
                obstaculos[i].transform.position = new Vector3(randomObs, -2, 0);
            }
            obstaculos[i].transform.position = obstaculos[i].transform.position + new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
        }

        for (int i = 0; i < serpientes.Count; i++)
        {
            if (serpientes[i] != null)
            {
                if (serpientes[i].transform.position.x <= -10)
                {
                    float randomPos = Random.Range(11, 25);
                    serpientes[i].transform.position = new Vector3(randomPos, -2, 0);
                }
                serpientes[i].transform.position = serpientes[i].transform.position + new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
            }
        }

    }
}
