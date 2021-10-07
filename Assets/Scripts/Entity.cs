using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected Team team; public Team Team => team;
    [SerializeField] protected float health = 1; public float Health => health;
    [SerializeField] protected AudioSource audioDeath; public AudioSource AudioDeath => audioDeath;
    [SerializeField] protected GameObject effectDeath;
    [SerializeField] protected bool canTakeDamage = true;
    protected Bulet attacker;

    public virtual void TakeDamage(float value, Bulet attacker) {
        this.attacker = attacker;
        if (canTakeDamage) {
            if (value == 0) {
                Debug.LogError(name + " ������� ���� � ������� " + value + ", ��� �� ����� ���� ���������");
                return;
            } else
            if (value < 0) {
                float convertValue = Mathf.Abs(value);
                Debug.LogError(name + " ������� ���� � ������� " + value + ". ��� �� ����� ���� ���������\n����� ������������� �������������� � " + convertValue);
                value = convertValue;
            }
            health -= value;
            if (health <= 0) Death();
        }
    }

    public virtual void TakeTreatment(float value) {
        if (value == 0) {
            Debug.LogError(name + " ������� ������� � ������� " + value + ", ��� �� ����� ���� ���������");
            return;
        } else
        if (value < 0) {
            float convertValue = Mathf.Abs(value);
            Debug.LogError(name + " ������� ������� � ������� " + value + ". ��� �� ����� ���� ���������\n����� ������������� �������������� � " + convertValue);
            value = convertValue;
        }
        health += value;
    }

    protected virtual void Death() {
        if (audioDeath) {
            audioDeath.Play();
            audioDeath.transform.parent = GameManager.Instance.CurrentLevel.transform;
            Destroy(audioDeath.gameObject, audioDeath.clip.length);
        }
        if (effectDeath) {
            effectDeath.transform.parent = GameManager.Instance.CurrentLevel.transform;
            effectDeath.SetActive(true);
        }
    }
}
