﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowShoot : MonoBehaviour
{
    public GameObject unit;
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public float accuracy = 1f;
    public float range = 20f;
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    // Time since last attack
    private float attackDelay = 0f;
    private unitMovement unitMovement;
    
    private void Awake() {
        attackCooldown = Time.time + 1;
    }

    private void Update() {
    }

    public IEnumerator arrowAttack(GameObject enemy) {
        while (enemy != null) {
            if (Vector3.Distance(unit.transform.position, enemy.transform.position) > Mathf.Abs(range)) {
                unitMovement = unit.GetComponent<unitMovement>();
                while (enemy != null && Vector3.Distance(unit.transform.position, enemy.transform.position) > Mathf.Abs(range)) {
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, enemy.transform.position, unitMovement.speed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
            attackCooldown = Time.time - attackDelay;
            if (attackCooldown >= 1f / attackSpeed && enemy != null) {
                attackDelay = Time.time;
                Vector3 Vo = calculateVelocity(enemy.transform.position, unit.transform.GetChild(1).position, 1f);
                GameObject spawn = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity);
                Rigidbody rb = spawn.GetComponent<Rigidbody>();
                rb.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
                rb.velocity = Vo;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    Vector3 calculateVelocity(Vector3 target, Vector3 origin, float time) {
        // Define the distence x and y first
        target.x += Random.Range(-accuracy, accuracy);
        target.y += Random.Range(-accuracy, accuracy);
        target.z += Random.Range(-accuracy, accuracy);
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        // Create a float the represent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;
        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
