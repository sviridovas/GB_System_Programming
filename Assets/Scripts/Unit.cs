using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int health;
    Coroutine _healingCoroutine = null;

    void RecieveHealing()
    {
        if(_healingCoroutine != null)
            StopCoroutine(_healingCoroutine);

        _healingCoroutine = StartCoroutine(HealingCoroutine());
    }

    IEnumerator HealingCoroutine()
    {
        for(int i = 6; i != 0; --i) {
            int newHealth = health + 5;
            if(newHealth >= 100) {
                health = 100;
                Debug.Log("health=" + health.ToString());
                break;
            } else 
            {
                health = newHealth;
                Debug.Log("health=" + health.ToString());
                yield return new WaitForSeconds(.5f);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        RecieveHealing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
