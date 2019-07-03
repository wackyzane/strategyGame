﻿using System.Collections;
using UnityEngine;

public class unitMovement : MonoBehaviour 
{
    public int health = 100;
    public int attack = 32;
    public float speed = 5f;
    public Coroutine moveCoroutine = null;
    private mouseClick mouseClick;
    private Vector3 movePoint;
    private arrowShoot arrowShoot;
    private GameObject hitObject;
    private GameObject crossbow;
    private Coroutine arrow = null;
    private Transform[] enemies;
    
    
    private void Start() {
        GameObject MouseManager = GameObject.Find("MouseManager");
        mouseClick = MouseManager.GetComponent<mouseClick>();
        if (gameObject.tag == "unit") {
            mouseClick.selectableObjects.Add(this.gameObject);
        }
    }

    private void Update() {
        
    }

    public void findAction() {
        movePoint = mouseClick.mouseMovePoint();
        hitObject = mouseClick.isObjectSelected();
        if (hitObject.tag == "Enemy") {
            //|| gameObject.name == "Bowman" || gameObject.name == "Bowman(Clone)"
            if (gameObject.name == "Crossbowman" || gameObject.name == "Crossbowman(Clone)") {
                crossbow = gameObject.transform.GetChild(0).gameObject;
                arrowShoot = crossbow.GetComponent<arrowShoot>();
                gameObject.transform.LookAt(hitObject.transform);
                StopAllCoroutines();
                arrow = arrowShoot.StartCoroutine(arrowShoot.arrowAttack(hitObject));
            }
        } else {
            StopAllCoroutines();
            if (arrow != null) {
                StopCoroutine(arrow);
            }
            moveCoroutine = StartCoroutine(moveOverSpeed(gameObject, movePoint, speed));
        }
    }

    public IEnumerator moveOverSpeed(GameObject unit, Vector3 movePoint, float speed) {
        unit.transform.LookAt(movePoint);
        while(unit.transform.position != movePoint) {
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, movePoint, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
}
