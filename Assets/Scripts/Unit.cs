using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
                yield break;
            } else 
            {
                health = newHealth;
                Debug.Log("health=" + health.ToString());
                yield return new WaitForSeconds(.5f);
            }
        }
    }

    async Task Task1()
    {
        await Task.Delay(1000);
        Debug.Log("Task1 end");
    }

    async Task Task2()
    {
        for(int i = 60; i != 0; --i) {
            await Task.Yield();    
        }
        Debug.Log("Task2 end");
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
