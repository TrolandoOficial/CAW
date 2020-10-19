using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class IAInimigoA : MonoBehaviour
{
    private gameController _GameController;

    private bool isCurva;

    public float velocidadeMovimento;
    public float pontoInicialCurva;

    public float grausCurva;
    private float incrementado;

    private float rotacaoZ;
    public float incrementar;

    public Transform arma;
    public float velocidadeTiro;
    public float delayTiro;

    public int idBullet;
    public tagBullets tagTiro;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(gameController)) as gameController;
        rotacaoZ = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) 
        {
            Atirar();
        }

        if (transform.position.y <= pontoInicialCurva && !isCurva) 
        {
            isCurva = true;
        }

        if (isCurva && incrementado < grausCurva)
        {
            rotacaoZ += incrementar;
            transform.rotation = Quaternion.Euler(0, 0, rotacaoZ);

            if (incrementar < 0)
            {
                incrementado += (incrementar * -1);
            }
            else 
            {
                incrementado += incrementar;
            }
        }
        transform.Translate(Vector3.down * velocidadeMovimento * Time.deltaTime);    
    }

    void Atirar() 
    {
        GameObject temp = Instantiate(_GameController.prefabBullet[idBullet], arma.position, transform.localRotation);

        temp.transform.tag = _GameController.aplicarTag(tagTiro);

        temp.GetComponent<Rigidbody2D>().velocity = -transform.up * velocidadeTiro;
    }

    private void OnBecameVisible()
    {
        StartCoroutine("ControleTiro");
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag) 
        {
            case "playerShot":
                GameObject temp = Instantiate(_GameController.prefabExplosao, transform.position, _GameController.prefabExplosao.transform.localRotation);
                Destroy(collider.gameObject);
                Destroy(this.gameObject);
                break;
        }
    }

    IEnumerator ControleTiro() 
    {
        yield return new WaitForSeconds(delayTiro);
        Atirar();
        StartCoroutine("ControleTiro");
    }
}
