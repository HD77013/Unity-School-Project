using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImmediate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyImmediate());
    }

    private IEnumerator destroyImmediate()
    {
        yield return new WaitForSeconds(0.1f);
        DestroyImmediate(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
