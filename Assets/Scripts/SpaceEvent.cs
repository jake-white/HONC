using UnityEngine;

public class SpaceEvent {
    public enum SpaceEventType { Asteroid, SolarFlare, Comet, Supernova};
    public SpaceEventType type;
    public Planet target;
    public Vector3 relevantVector;
    public SpaceEvent(SpaceEventType type, Planet target, Vector3 relevantVector) {
        this.type = type;
        this.target = target;
        this.relevantVector = relevantVector;
    }
}