using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction_Force : MonoBehaviour
{
    public GameObject[] planets;
    public Rigidbody2D planetRB;
    public Rigidbody2D star; 
    private float m1;

    [SerializeField] private float G = 0.00667f;

    private void Start()
    {
        //Find all of the planets and their rigidbodies
        planets = GameObject.FindGameObjectsWithTag("planet");
        planetRB = gameObject.GetComponent<Rigidbody2D>();
        star = GameObject.FindGameObjectWithTag("star").GetComponent<Rigidbody2D>();
        m1 = planetRB.mass;

        //Give any initial velocities to a planet
        InitialVelocity();
    }

    private void FixedUpdate()
    {
        ApplyForce();
    }

    private void ApplyForce()
    {
        //apply force from the star
        float starMass = star.mass;
        float starDistance = Vector2.Distance(planetRB.transform.position, star.transform.position);

        //calculate force
        float starForce = (G * (m1 * starMass)) / (starDistance * starDistance);
        Vector2 starForceWithDirection = (-1 * starForce * (planetRB.transform.position - star.transform.position)); // multiplied by -1 cause it was repelling instead of attracting

        //apply force
        planetRB.velocity += starForceWithDirection;

        //for each planet that's affecting it
        for (int i = 0; i < planets.Length; i++)
        {
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

    private void InitialVelocity() // !!!!!!!!!!!!!!!!!!!!!! Make the initial velocity based off of all surrounding objects and it will help with moons and stuff. Might fix the problem I'm having.....
    {
        float distance = Vector2.Distance(planetRB.transform.position, star.transform.position);

        //Find Orbital Velocity
        Vector2 initialVelocity = new Vector2(Mathf.Sqrt(G * star.mass * distance), 0); //If you multiply by the radius(distance), instead of dividing like you're supposed to, it makes a really cool three-leaf orbit
        gameObject.GetComponent<Rigidbody2D>().velocity += initialVelocity;
        print("Initial Velocity for " + gameObject + " is: " + initialVelocity);
    }
}
