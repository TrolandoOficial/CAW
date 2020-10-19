using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruirTempo : MonoBehaviour
{
    public float destruirObjeto;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destruirObjeto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
