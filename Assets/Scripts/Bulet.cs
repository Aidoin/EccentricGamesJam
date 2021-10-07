using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulet : Entity
{
    [HideInInspector] public List<Entity> DoingDamageToThem = new List<Entity>();
    [SerializeField] private Collider[] colliders; public Collider[] Colliders => colliders;
    [SerializeField] private Transform colliderExplosion;
    [SerializeField] private Rigidbody rigidbody;

    public float speed;
    public float Damage;


    void Start()
    {
        rigidbody.velocity = transform.forward * speed;        
    }


    public void SetTeam(Team newTeam) {
        team = newTeam;
    }

    private void OnCollisionEnter(Collision collision) {
        Entity hit = null;

        if (collision.rigidbody) {
            if (collision.rigidbody.GetComponent<Entity>()) {
                hit = collision.rigidbody.GetComponent<Entity>();
            }
        } else {
            if (collision.gameObject.GetComponent<Entity>()) {
                hit = collision.gameObject.GetComponent<Entity>();
            }
        }

        if (hit != null) {
            if(team == Team.kind) {
                if(hit.Team == Team.Environment) {
                    hit.TakeDamage(Damage, this);
                }
            } else
            if (team != hit.Team) {
                hit.TakeDamage(Damage, this);
            }
        }

        Death();
    }

    protected override void Death() {
        base.Death();
        team = Team.kind;
        colliderExplosion.gameObject.SetActive(true);
        Destroy(gameObject, Time.fixedDeltaTime + 0.1f);
    }
}
