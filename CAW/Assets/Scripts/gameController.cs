using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum tagBullets 
{
    player,
    inimigo
}

public enum gameState
{
    intro,
    gameplay
}

public class gameController : MonoBehaviour
{
    public playerController _PlayerController;

    public GameObject prefabPlayer;
    public int vidaExtra;
    public float delaySpawnPlayer;
    public float tempoInvencivel;
    public float velocidadeFase;
    public gameState currentState;

    [Header("Limites de Movimento")]
    public Transform limiteSuperior;
    public Transform limiteInferior;
    public Transform limiteEsquerdo;
    public Transform limiteDireito;
    public Transform spawnPlayer;
    public Transform posicaoFinalFase;
    public Transform cenario;


    [Header("Prefabs")]
    public GameObject[] prefabBullet;
    public GameObject prefabExplosao;

    [Header("Config Intro")]
    public float tamanhoInicialNave;
    public float tamanhoOriginal;
    public Transform posicaoInicial;
    public Transform posicaoDecolagem;

    public float velocidadeDecolagem;
    private float velocidadeAtual;
    private bool isDecolar;

    public Color corInicialFumaca;
    public Color corFinalFumaca;

    public bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("IntroFase");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) 
        {
            LimitarMovimentoPlayer();
        }

        if (isDecolar && currentState == gameState.intro) 
        {
            _PlayerController.transform.position = Vector3.MoveTowards(_PlayerController.transform.position, posicaoDecolagem.position, velocidadeAtual * Time.deltaTime);

            if (_PlayerController.transform.position == posicaoDecolagem.position) 
            {
                StartCoroutine("Subir");
                currentState = gameState.gameplay;
            }
            _PlayerController.srFumaca.color = Color.Lerp(corInicialFumaca, corFinalFumaca, 0.1f);
        }
    }

    private void LateUpdate()
    {
        if (currentState == gameState.gameplay) 
        {
            cenario.position = Vector3.MoveTowards(cenario.position, new Vector3(cenario.position.x, posicaoFinalFase.position.y, 0), velocidadeFase * Time.deltaTime);
        }
    }

    void LimitarMovimentoPlayer() 
    {
        float posX = _PlayerController.transform.position.x;
        float posY = _PlayerController.transform.position.y;

        if (posY > limiteSuperior.position.y)
        {
            _PlayerController.transform.position = new Vector3(posX, limiteSuperior.position.y, 0);
        }
        else if (posY < limiteInferior.position.y)
        {
            _PlayerController.transform.position = new Vector3(posX, limiteInferior.position.y, 0);
        }

        if (posX > limiteDireito.position.x)
        {
            _PlayerController.transform.position = new Vector3(limiteDireito.position.x, posY, 0);
        }
        else if (posX < limiteEsquerdo.position.x)
        {
            _PlayerController.transform.position = new Vector3(limiteEsquerdo.position.x, posY, 0);
        }
    }

    public string aplicarTag(tagBullets tag) 
    {
        string retorno = null;

        switch (tag) 
        {
            case tagBullets.player:
                retorno = "playerShot";
                break;
            case tagBullets.inimigo:
                retorno = "inimigoShot";
                break;
        }
        return retorno;
    }

    public void HitPlayer() 
    {
        isAlive = false;
        Destroy(_PlayerController.gameObject);
        GameObject temp = Instantiate(prefabExplosao, _PlayerController.transform.position, prefabExplosao.transform.localRotation);
        vidaExtra--;

        if (vidaExtra >= 0)
        {
            StartCoroutine("InstanciarPlayer");
        }
        else 
        {
            print("GameOver!");
        }
    }

    IEnumerator InstanciarPlayer() 
    {
        yield return new WaitForSeconds(delaySpawnPlayer);
        GameObject temp =  Instantiate(prefabPlayer, spawnPlayer.position, spawnPlayer.localRotation);
        yield return new WaitForEndOfFrame();
        _PlayerController.StartCoroutine("Invencivel");
    }

    IEnumerator IntroFase()
    {
        _PlayerController.srFumaca.color = corInicialFumaca;
        _PlayerController.sombra.gameObject.SetActive(false);
        _PlayerController.transform.localScale = new Vector3(tamanhoInicialNave, tamanhoInicialNave, tamanhoInicialNave);
        _PlayerController.transform.position = posicaoInicial.position;
        yield return new WaitForSeconds(1);
        isDecolar = true;

        for (velocidadeAtual = 0; velocidadeAtual < velocidadeDecolagem; velocidadeAtual+= 0.2f)
        {
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Subir()
    {
        _PlayerController.sombra.gameObject.SetActive(true);
        for (float s = tamanhoInicialNave; s < tamanhoOriginal; s += 0.025f) 
        {
            _PlayerController.transform.localScale = new Vector3(s, s, s);
            _PlayerController.sombra.gameObject.transform.localScale = new Vector3(s, s, s);
            _PlayerController.srFumaca.color = Color.Lerp(_PlayerController.srFumaca.color, corFinalFumaca, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
