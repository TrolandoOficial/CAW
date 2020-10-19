using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private gameController _GameController;
    private Rigidbody2D rbPlayer;
    private SpriteRenderer srPlayer;
    public SpriteRenderer srFumaca;
    public GameObject sombra;

    public float velocidade;

    public Transform armaPosition;

    public float velocidadeTiro;

    public int idBullet;
    public tagBullets tagTiro;
    public Color corInvecivel;
    public float delayPiscar;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(gameController)) as gameController;

        _GameController._PlayerController = this;
        _GameController.isAlive = true;

        rbPlayer = GetComponent<Rigidbody2D>();
        srPlayer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameController.currentState == gameState.gameplay) 
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            rbPlayer.velocity = new Vector2(horizontal * velocidade, vertical * velocidade);

            if (Input.GetButtonDown("Fire1")) Shot();
        }
    }

    void Shot()
    {
        GameObject temp = Instantiate(_GameController.prefabBullet[idBullet]);

        temp.transform.tag = _GameController.aplicarTag(tagTiro);

        temp.transform.position = armaPosition.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "inimigoShot":
                _GameController.HitPlayer();
                Destroy(collider.gameObject);
                break;
        }
    }

    IEnumerator Invencivel() 
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        srPlayer.color = corInvecivel;
        StartCoroutine("PiscarPlayer");

        yield return new WaitForSeconds(_GameController.tempoInvencivel);
        col.enabled = true;
        srPlayer.color = Color.white;
        srPlayer.enabled = true;
        StopCoroutine("PiscarPlayer");
    }

    IEnumerator PiscarPlayer() 
    {
        yield return new WaitForSeconds(delayPiscar);
        srPlayer.enabled = !srPlayer.enabled;
        StartCoroutine("PiscarPlayer");
    }
}
