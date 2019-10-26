using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public int damage;
    public Transform target;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (!target)
        {
            Debug.LogWarning("no target");
            transform.Translate(this.transform.forward * Time.deltaTime * speed);
            return;
        }
        //transform.Translate(Vector3.Normalize(target.transform.position - this.transform.position) * Time.deltaTime * speed);
        this.transform.position += Vector3.Normalize(target.transform.position - this.transform.position) * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent) return;
        if (!other.transform.parent.parent) return;
        if (other.transform.parent.parent != target) return;
        Health h;
        if (h= other.transform.parent.parent.GetComponent<Health>())
        {
            h.health -= damage;
        }
        Destroy(this.gameObject);
    }
}
