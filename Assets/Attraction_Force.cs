using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction_Force : MonoBehaviour
{
    public GameObject planet;
    public GameObject star;
    private Vector2 starPos;
    private Vector2 planetPos;
    public Rigidbody2D planetRB;

    [SerializeField] private float planetMass = 10.0f;
    [SerializeField] private float starMass = 30.0f;
    [SerializeField] private Vector2 initalVelocity = new Vector2(5.0f, 0.0f);
    [SerializeField] private float G = 0.0000667f;

    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("planet");
        star = GameObject.FindGameObjectWithTag("star");
        planetRB = planet.GetComponent<Rigidbody2D>();

        planetRB.velocity = planetRB.velocity + initalVelocity;
    }

    void FixedUpdate()
    {
        planetRB.velocity = planetRB.velocity + CalculateForce();
    }

    public Vector2 CalculateForce()
    {
        starPos = star.transform.position;
        planetPos = planet.transform.position;

        float distance = Vector2.Distance(starPos, planetPos);

        float force = (G * (starMass*planetMass)) / (distance * distance);

        Vector2 forceWithDirection = (force * (starPos - planetPos));

        return (forceWithDirection);
    }

    // F = G * (M1*M2)/R^2
}
