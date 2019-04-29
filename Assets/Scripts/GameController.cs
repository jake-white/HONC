using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Planet> planets, moons;
    public static GameController instance;
    public GameObject asteroidPrefab, cometPrefab, SFXPrefab;
    public ParticleSystem flareShot, flareSphere;
    public AudioClip alarmSFX, nightmare;
    public AudioSource music;
    public Animator sun;
    int gameState = 0;
    SpaceEvent nextEvent = null;
    float percent = 0;
    bool gamefinished = false;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }
    void Start() {
        DontDestroyOnLoad(this);
        for (int i = 0; i < planets.Count; ++i) {
            planets[i].SetName("Planet " + i);
        }
        for (int i = 0; i < moons.Count; ++i) {
            moons[i].SetName("Moon " + i);
        }
    }

    // Update is called once per frame
    void Update() {
        CheckGameFinished();
        if(!gamefinished) {
            UIController.instance.UpdateFinishedText(AmountPlanetsFinished(), AmountPlanets());
            if (AmountPlanetsOnceFinished() > gameState) {
                gameState = AmountPlanetsOnceFinished();
                GameAdvance();
            }
        }
    }

    public void GameAdvance() {
        if (AmountUntriggeredPlanets() > 0) {
            List<Planet> untriggered = GetFinishedUntriggeredPlanets();
            int randomIndex = Random.Range(0, untriggered.Count - 1);
            if(untriggered.Count > 1) {
                while (untriggered[randomIndex] == PlayerController.instance.GetCurrentPlanet()) {
                    randomIndex = Random.Range(0, untriggered.Count);
                }
            }
            if (gameState == 2) {
                nextEvent = new SpaceEvent(SpaceEvent.SpaceEventType.Asteroid, untriggered[randomIndex], new Vector3(2 * untriggered[randomIndex].transform.localScale.x, 20, 0));
                untriggered[randomIndex].TriggerEvent();
            }
            else if (gameState == 3) {
                nextEvent = new SpaceEvent(SpaceEvent.SpaceEventType.SolarFlare, untriggered[randomIndex], untriggered[randomIndex].transform.position);
                untriggered[randomIndex].TriggerEvent();
            }
            else if(gameState == 1) {
                nextEvent = new SpaceEvent(SpaceEvent.SpaceEventType.Comet, untriggered[randomIndex], new Vector3(2 * untriggered[randomIndex].transform.localScale.x, 20, 0));
                untriggered[randomIndex].TriggerEvent();
            }
        }
    }

    public void CheckGameFinished() {
        if (AmountPlanetsFinished() >= AmountPlanets()) {
            nextEvent = new SpaceEvent(SpaceEvent.SpaceEventType.Supernova, planets[0], Vector3.zero);
            gamefinished = true;
        }
    }

    public void CheckEvents() {
        if (nextEvent == null) {

        }
        else if (nextEvent.type == SpaceEvent.SpaceEventType.Asteroid) {
            Vector3 asteroidLocation = nextEvent.target.transform.position + nextEvent.relevantVector;
            GameObject newAsteroid = Instantiate(asteroidPrefab);
            newAsteroid.transform.position = asteroidLocation;
            newAsteroid.GetComponent<Asteroid>().SetTarget(nextEvent.target.gameObject);
            UIController.instance.SetScreen(nextEvent.type, nextEvent.target);
            newAsteroid.GetComponent<Asteroid>().isEvent = true;
            PlayerController.instance.SetMarker(newAsteroid);
            PlaySFX(alarmSFX);
        }
        else if (nextEvent.type == SpaceEvent.SpaceEventType.SolarFlare) {
            flareShot.GetComponent<FlareShot>().SetTarget(nextEvent.target);
            flareShot.transform.LookAt(nextEvent.relevantVector);
            flareShot.Play();
            flareSphere.Play();
            flareShot.GetComponent<Animator>().Play("FlareShot");
            UIController.instance.SetScreen(nextEvent.type, nextEvent.target);
            PlaySFX(alarmSFX);
        }
        else if (nextEvent.type == SpaceEvent.SpaceEventType.Comet) {
            Vector3 asteroidLocation = nextEvent.target.transform.position + nextEvent.relevantVector;
            GameObject newAsteroid = Instantiate(cometPrefab);
            newAsteroid.transform.position = asteroidLocation;
            newAsteroid.GetComponent<Asteroid>().SetTarget(nextEvent.target.gameObject);
            UIController.instance.SetScreen(nextEvent.type, nextEvent.target);
            newAsteroid.GetComponent<Asteroid>().isEvent = true;
            PlayerController.instance.SetMarker(newAsteroid);
            PlaySFX(alarmSFX);
        }
        else if (nextEvent.type == SpaceEvent.SpaceEventType.Supernova) {
            music.Stop();
            music.clip = nightmare;
            music.Play();
            percent = 100 * ((float) AmountPlanetsFinished() + AmountMoonsFinished()) / ((float)AmountPlanets() + AmountMoons());
            PlayerController.instance.FinalSequence();
            UIController.instance.FinalSequence();
            sun.Play("Supernova");
            flareSphere.Play();
            flareShot.Play();
            gamefinished = true;
        }
        nextEvent = null;
    }

    public void PlaySFX(AudioClip clip) {
        GameObject newSFX = Instantiate(SFXPrefab);
        newSFX.GetComponent<SFX>().SetClip(clip);
    }

    int AmountPlanets() {
        return planets.Count;
    }
    int AmountMoons() {
        return moons.Count;
    }

    int AmountPlanetsFinished() {
        int total = 0;
        foreach (Planet p in planets) {
            if (p.IsFinished()) {
                ++total;
            }
        }
        return total;
    }

    int AmountMoonsFinished() {
        int total = 0;
        foreach (Planet p in moons) {
            if (p.IsFinished()) {
                ++total;
            }
        }
        return total;
    }

    int AmountPlanetsOnceFinished() {
        int total = 0;
        foreach (Planet p in planets) {
            if (p.WasOnceFinished()) {
                ++total;
            }
        }
        return total;
    }

    int AmountTriggeredPlanets() {
        int total = 0;
        foreach (Planet p in planets) {
            if (p.IsEventTriggered()) {
                ++total;
            }
        }
        return total;
    }

    int AmountUntriggeredPlanets() {
        int total = 0;
        foreach (Planet p in planets) {
            if (!p.IsEventTriggered()) {
                ++total;
            }
        }
        return total;
    }

    List<Planet> GetFinishedPlanets() {
        List<Planet> finished = new List<Planet>();
        foreach (Planet p in planets) {
            if (p.IsFinished()) {
                finished.Add(p);
            }
        }
        return finished;
    }

    List<Planet> GetOnceFinishedPlanets() {
        List<Planet> finished = new List<Planet>();
        foreach (Planet p in planets) {
            if (p.WasOnceFinished()) {
                finished.Add(p);
            }
        }
        return finished;
    }

    List<Planet> GetTriggeredPlanets() {
        List<Planet> finished = new List<Planet>();
        foreach (Planet p in planets) {
            if (p.IsEventTriggered()) {
                finished.Add(p);
            }
        }
        return finished;

    }

    List<Planet> GetFinishedUntriggeredPlanets() {
        List<Planet> finished = new List<Planet>();
        foreach (Planet p in planets) {
            if (p.IsFinished() && !p.IsEventTriggered()) {
                finished.Add(p);
            }
        }
        return finished;
    }

    public float GetPercent() {
        return percent;
    }
}
