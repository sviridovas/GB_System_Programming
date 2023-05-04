using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
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

    async Task Task1(CancellationToken token)
    {
        await Task.Delay(10, token);
        Debug.Log("Task1 end");
    }

    async Task Task2(CancellationToken token)
    {
        for(int i = 60; i != 0; --i) {
            if(token.IsCancellationRequested) return;
            await Task.Yield();    
        }
        Debug.Log("Task2 end");
    }

    async Task<bool> WhatTaskFasterAsync (CancellationToken token, Task task1, Task task2) {
        var i = await Task.WhenAny(task1, task2);
        // как остановить задачи отсюда, если нельзя влиять на token
        if(i == task1) {
            return true;
        } 
        else {
            return false;
        }
    }

    // Start is called before the first frame update
    async void Start()
    {
        // RecieveHealing();

        var token = new CancellationTokenSource().Token;
        await WhatTaskFasterAsync(token, Task1(token), Task2(token));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
