using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Rigidbody rb;
    float hydrogen, oxygen, nitrogen, carbon;
    public GameObject ocean, atmosphere, continent;
    public enum PlanetLikeness { Earth, Mars, Neptune}

    public PlanetLikeness type;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        AddElement(0.001f, 0.001f, 0.001f, 0.001f);
        UpdateAppearance();
    }

    public void AddElement(float h, float o, float n, float c) {
        this.hydrogen += h;
        this.oxygen += o;
        this.nitrogen += n;
        this.carbon += c;

        this.hydrogen = Mathf.Clamp(this.hydrogen, 0, 1);
        this.oxygen = Mathf.Clamp(this.oxygen, 0, 1);
        this.nitrogen = Mathf.Clamp(this.nitrogen, 0, 1);
        this.carbon = Mathf.Clamp(this.carbon, 0, 1);
        UpdateAppearance();
    }

    void UpdateAppearance()
    {
        float atmosphereValue = ((this.oxygen + this.nitrogen) / 2) / 10;
        float oceanValue = 0.95f + (0.05f * (this.hydrogen * this.oxygen));
        float plantValue = this.hydrogen * this.oxygen * this.nitrogen * this.carbon;
        float oxidationValue = this.oxygen;

        ocean.transform.localScale = new Vector3(oceanValue, oceanValue, oceanValue);
        Material atmosphereMat = atmosphere.GetComponent<MeshRenderer>().material;
        atmosphereMat.color = new Color(atmosphereMat.color.r, atmosphereMat.color.g, atmosphereMat.color.b, atmosphereValue);

        Material continentMat = continent.GetComponent<MeshRenderer>().material;
        Color continentColor = continentMat.color;

        if (type == PlanetLikeness.Earth)
        {
            continentColor = new Color(continentMat.color.r, plantValue, continentMat.color.b, continentMat.color.a);
        }
        else if(type == PlanetLikeness.Mars)
        {
            continentColor = new Color(oxidationValue, continentMat.color.g, continentMat.color.b, continentMat.color.a);
        }
        else if (type == PlanetLikeness.Neptune)
        {
            continentColor = new Color(continentMat.color.r, continentMat.color.g, plantValue, continentMat.color.a);
        }

        continentMat.color = continentColor;
    }
}
