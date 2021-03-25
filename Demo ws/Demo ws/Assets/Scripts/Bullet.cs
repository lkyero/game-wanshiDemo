using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;//速度
    public float range;//距离
    public float volume;//体积
    public string enemyTag;//敌方

    public float attack;//攻击力
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<SphereCollider>() != null)
            gameObject.GetComponent<SphereCollider>().radius = volume;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.magnitude > range)
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(enemyTag))//碰撞了敌方
        {
            var hit = other.gameObject;
            if (hit.GetComponent<Pawn1>() != null)
            {
                Pawn1 pawn = hit.GetComponent<Pawn1>();
                if (pawn != null)
                {
                    pawn.TakeDamage(attack);
                }
            }
            else if (hit.GetComponent<Pawn2>() != null)
            {
                Pawn2 pawn = hit.GetComponent<Pawn2>();
                if (pawn != null)
                {
                    pawn.TakeDamage(attack);
                }
            }
            Destroy(gameObject);
        }
    }
}
