using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SignRegion : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField] private Material BritSign;
    [SerializeField] private Material EUSign;
    [SerializeField] private Material EUkmSign;
    [SerializeField] private Material SwedeSign;
    [SerializeField] private Material USASign;
    private Material[] signList;
    private int pickRandom;
    private int RegionIndex = 0;
    string[] EUCountries = {"Germany", "France", "Italy", "Poland", "Netherlands", "Switzerland", "Austria", "Greece", "Moldova", "Belarus", "Belarus",
            "Belgium", "Denmark", "Romania", "Norway", "Croatia", "Hungary", "Ukrane", "Czechia", "Ireland", "Serbia", "Luxembourg", "Malta", "Lithuania", "Bulgaria",
            "Estonia", "Albania", "Cyprus", "Solvenia", "Latvia", "Solvakia", "Liechtenstein", "Bosnia and Herzegovina", "Montenegro", "Monaco", "Kosovo", "North Macedonia",
            "Andorra", "Vatican City", "San Marino", "Isle of Man", "Gibraltar"};

    // Start is called before the first frame update
    void Start()
    {
        signList = new Material[5];
        signList[0] = BritSign;
        signList[1] = EUSign;
        signList[2] = EUkmSign;
        signList[3] = SwedeSign;
        signList[4] = USASign;

        pickRandom = Random.Range(0, signList.Length);

        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (meshRenderer.material != signList[getRegionIndex()])
        {
            meshRenderer.material = signList[getRegionIndex()];
        }
    }

    private int getRegionIndex()
    {
        if (RegionInfo.CurrentRegion.DisplayName == "United Kingdom")
        {
            return 0;
        }
        else if (RegionInfo.CurrentRegion.DisplayName == "United States")
        {
            return 4;
        }
        else if (RegionInfo.CurrentRegion.DisplayName == "Sweden" || RegionInfo.CurrentRegion.DisplayName == "Finland" || RegionInfo.CurrentRegion.DisplayName == "Iceland")
        {
            return 3;
        }
        else if (RegionInfo.CurrentRegion.DisplayName == "Netherlands")
        {
            return 2;
        }
        for (int i=0; i< EUCountries.Length; i++)
        {
            if (RegionInfo.CurrentRegion.DisplayName == EUCountries[i])
            {
                return 1;
            }
        }
        return pickRandom;
    }
}
