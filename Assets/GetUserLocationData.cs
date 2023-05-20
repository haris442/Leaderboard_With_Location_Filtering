using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
public class GetUserLocationData : MonoBehaviour
{
    public static GetUserLocationData instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GetUserCountryRegionData(Action<string, string> callback)
    {
        StartCoroutine(GetUserLocationDataCoroutine(callback));
    }

    private IEnumerator GetUserLocationDataCoroutine(Action<string, string> callback)
    {
        string apiUrl = "https://get.geojs.io/v1/ip/geo.json";

        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);

        }
        else
        {
            string userLocationData = request.downloadHandler.text;

            //Parsing the Json Data
         
                JSONNode parsedUserLocationData = JSON.Parse(userLocationData);
            string country = parsedUserLocationData["country"].Value;
            string region = parsedUserLocationData["region"].Value;
            callback(country, region);
        }
    }
}

