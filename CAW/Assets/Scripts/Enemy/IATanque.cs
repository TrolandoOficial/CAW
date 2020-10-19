using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATanque : MonoBehaviour
{
    private gameController _GameController;

    public Transform arma;
    public float velocidadeTiro;
    public float delayTiro;

    public int idBullet;
    public tagBullets tagTiro;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(gameController)) as gameController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Atirar()
    {
        if (_GameController.isAlive) 
        {
            arma.up = _GameController._PlayerController.transform.position - transform.position;
            GameObject temp = Instantiate(_GameController.prefabBullet[idBullet], arma.position, arma.localRotation);
            temp.transform.tag = _GameController.aplicarTag(tagTiro);

            temp.GetComponent<Rigidbody2D>().velocity = arma.up * velocidadeTiro;
        }
    }

    private void OnBecameVisible()
    {
        StartCoroutine("ControleTiro");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "playerShot":
                GameObject temp = Instantiate(_GameController.prefabExplosao, transform.position, _GameController.prefabExplosao.transform.localRotation);
                temp.transform.parent = _GameController.cenario;
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
