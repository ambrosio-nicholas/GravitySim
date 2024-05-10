using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction_Force : MonoBehaviour
{
    public GameObject[] planets;
    public Rigidbody2D planetRB;
    private float m1;

    [SerializeField] private Vector2 initalVelocity = new Vector2(5.0f, 0.0f);
    [SerializeField] private float G = 0.00667f;

    private void Start()
    {
        //Find all of the planets and their rigidbodies
        planets = GameObject.FindGameObjectsWithTag("planet");
        planetRB = gameObject.GetComponent<Rigidbody2D>();
        m1 = planetRB.mass;

        //Give any initial velocities to a planet
        gameObject.GetComponent<Rigidbody2D>().velocity += initalVelocity;
        print("Applied initial velocity");
    }

    private void FixedUpdate()
    {
        ApplyForce();
    }

    public void ApplyForce()
    {
        //for each planet that's affecting it
        for (int i = 0; i < planets.Length; i++)
        {
            print(planets[i]);
            //If it's not itself
            if(planets[i] != gameObject)
            {
                Rigidbody2D otherPlanet = planets[i].GetComponent<Rigidbody2D>();
                float m2 = otherPlanet.mass;
                float distance = Vector2.Distance(planetRB.transform.position, otherPlanet.transform.position);

                //calculate force
                float force = (G * (m1 * m2)) / (distance * distance);
                Vector2 forceWithDirection = (-1 * force * (planetRB.transform.position - otherPlanet.transform.position)); // multiplied by -1 cause it was repelling instead of attracting

                //apply force
                planetRB.velocity += forceWithDirection;
            }

        }
    }
}
