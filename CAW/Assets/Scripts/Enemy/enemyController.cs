using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    private playerController _PlayerController;

    public GameObject explosaoPrefab;
    public GameObject[] loot;

    public Transform arma;
    public GameObject tiroPrefab;

    public float[] delayEntreTiros;

    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = FindObjectOfType(typeof(playerController)) as playerController;
        StartCoroutine("Atirar");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "playerShot":
                Destroy(collider.gameObject);
                GameObject temp = Instantiate(explosaoPrefab, transform.position, transform.localRotation);
                Destroy(temp.gameObject, 1f);

                Loot();

                Destroy(this.gameObject);

                break;
        }
    }

    void Loot()
    {
        int idItem = 0;
        int rand = Random.Range(0, 400);
        if (rand < 100)
        {
            if (rand < 15)
            {
                idItem = 2;
            }
            else if (rand < 50)
            {
                idItem = 1;
            }
            else
            {
                idItem = 0;
            }
            Instantiate(loot[idItem], transform.position, transform.localRotation);
        }
    }

    void Shot()
    {
        arma.right = _PlayerController.transform.position - transform.position;
        GameObject temp = Instantiate(tiroPrefab, arma.position, arma.localRotation);
        temp.GetComponent<Rigidbody2D>().velocity = arma.right * 3;
    }

    IEnumerator Atirar() 
    {
        yield return new WaitForSeconds(Random.Range(delayEntreTiros[0], delayEntreTiros[1]));
        Shot();
        StartCoroutine("Atirar");
    }
}
