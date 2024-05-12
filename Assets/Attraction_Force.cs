using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction_Force : MonoBehaviour
{
    public GameObject[] planets;
    public GameObject[] moons;
    public GameObject star;
    public Rigidbody2D planetRB;
    private float m1;
    const float G = 0.0667f;

    private TrailRenderer trail;
    [SerializeField] public Vector2 initialVelocity = new Vector2(0.0f,0.0f);
    [SerializeField] bool specifyInitialVelocity = false;

    private void Start()
    {
        //Find all of the planets and their rigidbodies
        planets = GameObject.FindGameObjectsWithTag("planet");
        moons = GameObject.FindGameObjectsWithTag("moon");
        trail = gameObject.GetComponent<TrailRenderer>();
        star = GameObject.FindGameObjectWithTag("star");
        planetRB = gameObject.GetComponent<Rigidbody2D>();
        m1 = planetRB.mass;

        PlanetInitialVelocity();
        MoonInitialVelocity();
        SetTrail();
    }

    private void FixedUpdate()
    {
        ApplyForce();
    }

    private void ApplyForce()
    {
        //for the star (if there is one)
        if (star)
        {
            float starMass = star.GetComponent<Rigidbody2D>().mass;
            float starDistance = Vector2.Distance(planetRB.transform.position, star.GetComponent<Rigidbody2D>().transform.position);

            float starForce = (G * (m1 * starMass) / (starDistance * starDistance));
            Vector2 starDirection = star.GetComponent<Rigidbody2D>().transform.position - planetRB.transform.position;
            Vector2 starForceWithDirection = (starForce * (starDirection / starDirection.magnitude)) / planetRB.mass;

            planetRB.velocity += starForceWithDirection * Time.deltaTime;
        }

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
                Vector2 direction = otherPlanet.transform.position - planetRB.transform.position;
                Vector2 forceWithDirection = (force * (direction/direction.magnitude)) / planetRB.mass;

                //apply force
                planetRB.velocity += forceWithDirection * Time.deltaTime;
            }

        }

        //for each moon that's affecting it
        for (int i = 0; i < moons.Length; i++)
        {
            if(moons[i] != gameObject)
            {
                Rigidbody2D otherMoon = moons[i].GetComponent<Rigidbody2D>();
                float m2 = otherMoon.mass;
                float distance = Vector2.Distance(planetRB.transform.position, otherMoon.transform.position);

                //calculate force
                float force = (G * (m1 * m2)) / (distance * distance);
                Vector2 direction = otherMoon.transform.position - planetRB.transform.position;
                Vector2 forceWithDirection = (force * (direction / direction.magnitude)) / planetRB.mass;

                //apply force
                planetRB.velocity += forceWithDirection * Time.deltaTime;
            }
        }
    }

    private void MoonInitialVelocity()
    {
        if (!specifyInitialVelocity && gameObject.tag == "moon")
        {
            //Find which planet is closest to the moon
            float leastDistance = 999999f;
            GameObject parentPlanet = gameObject;

            for(int i = 0; i < planets.Length; i++)
            {
                float distance = Vector2.Distance(planetRB.transform.position, planets[i].GetComponent<Rigidbody2D>().transform.position);
                if (distance < leastDistance)
                {
                    leastDistance = distance;
                    parentPlanet = planets[i];
                }
            }

            //Find Orbital Velocity around planet
            float planetDistance = Vector2.Distance(planetRB.transform.position, parentPlanet.GetComponent<Rigidbody2D>().transform.position);
            float planetMass = parentPlanet.GetComponent<Rigidbody2D>().mass;

            //Find tangent to star
            Vector2 direction = (planetRB.transform.position - parentPlanet.GetComponent<Rigidbody2D>().transform.position) / planetDistance;
            float temp = direction.x;
            direction.x = direction.y;
            direction.y = -1 * temp;

            //Find Orbital Velocity
            initialVelocity = new Vector2(Mathf.Sqrt(G * planetMass / planetDistance), 0);
            print("initialvelocity: " + initialVelocity);

            //Make it tangential to star
            initialVelocity.y = initialVelocity.x * direction.y;
            initialVelocity.x *= direction.x;

            //Get that planet's orbital velocity (after one frame)
            float time = 0f;
            Vector2 planetOrbitalVelocity = parentPlanet.GetComponent<Rigidbody2D>().velocity; //!!!!!!!!!!!!!!!!!!!!!!!!!!! Need better way to get parent planet's orbital velocity!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            while (time < Time.unscaledDeltaTime)
            {
                time += Time.unscaledDeltaTime;
            }

            print("parent planet's orbital velocity is: " + planetOrbitalVelocity);
            initialVelocity += planetOrbitalVelocity;
            print("moons initial velocity is: " + initialVelocity);

            //Apply Orbital Velocity
            planetRB.velocity = initialVelocity;
        }
    }

    private void PlanetInitialVelocity()
    {
        if (star && !specifyInitialVelocity && gameObject.tag == "planet")
        {
            float distance = Vector2.Distance(planetRB.transform.position, star.GetComponent<Rigidbody2D>().transform.position);
            float starMass = star.GetComponent<Rigidbody2D>().mass;

            //Find tangent to star
            Vector2 direction = (planetRB.transform.position - star.GetComponent<Rigidbody2D>().transform.position) / distance;
            float temp = direction.x;
            direction.x = direction.y;
            direction.y = -1 * temp;

            //Find Orbital Velocity
            initialVelocity = new Vector2(Mathf.Sqrt(G * starMass / distance), 0);

            //Make it tangential to star
            initialVelocity.y = initialVelocity.x * direction.y;
            initialVelocity.x *= direction.x;

            //Apply Orbital Velocity
            planetRB.velocity += initialVelocity;
        }
        else
        {
            planetRB.velocity += initialVelocity;
        }
    }

    private void SetTrail()
    {
        if (star)
        {
            float r = Vector2.Distance(planetRB.transform.position, star.GetComponent<Rigidbody2D>().transform.position);
            float orbitalPeriod = (Mathf.Sqrt((4 * Mathf.PI * (r * r * r)) / (G * star.GetComponent<Rigidbody2D>().mass)) * 2) - 0.6f;
            trail.time = orbitalPeriod;
        }
        else
        {
            trail.time = 20;
        }
        Color trailColor = gameObject.GetComponent<SpriteRenderer>().color * 0.9f;
        trail.startColor = trailColor;
        trail.endColor = trailColor * 0.05f;

        trail.startWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 0.6f;
    }
}
