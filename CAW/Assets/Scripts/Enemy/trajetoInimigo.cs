using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajetoInimigo : MonoBehaviour
{
    public Transform naveInimiga;
    public Transform[] checkpoints;

    public float velocidadeDeMovimento;
    public float delayParado;

    private int idCheckpoint;
    private bool movimentar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("IniciarMovimento");
    }

    // Update is called once per frame
    void Update()
    {
        if (movimentar == true)
        {
            naveInimiga.localPosition = Vector3.MoveTowards(naveInimiga.localPosition, checkpoints[idCheckpoint].position, velocidadeDeMovimento * Time.deltaTime);
            if (naveInimiga.localPosition == checkpoints[idCheckpoint].position) 
            {
                movimentar = false;
                StartCoroutine("IniciarMovimento");
            }
        }
    }

    IEnumerator IniciarMovimento()
    {
        idCheckpoint++;
        if (idCheckpoint >= checkpoints.Length) idCheckpoint = 0;
        yield return new WaitForSeconds(delayParado);
        movimentar = true;
    }
}
