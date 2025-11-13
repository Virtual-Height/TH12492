using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.Universal;

public class CityNameSetup : MonoBehaviour
{
    [Header("CITY NAME TEXT OBJECTS")]
    public Text[] flightCityName;
    public Text[] trainCityName;
    public Text[] basCityName;

    [Header("PRICE TEXT OBJECTS")]
    public Text[] flightPriceTexts;
    public Text[] trainPriceTexts;
    public Text[] basPriceTexts;

    [Header("SEAT TEXT OBJECTS")]
    public Text[] flightSeatTexts;
    public Text[] trainSeatTexts;
    public Text[] basSeatTexts;

    [Header("TIME TEXT OBJECTS")]
    public Text[] flightStartTimeTexts;
    public Text[] flightEndTimeTexts;
    public Text[] trainStartTimeTexts;
    public Text[] trainEndTimeTexts;
    public Text[] basStartTimeTexts;
    public Text[] basEndTimeTexts;

    private Coroutine setupCoroutine;


    public GameObject profileScreenPopup;
    public GameObject oTPScreenScreenPopup;
    public GameObject TravelScreenPopup;

    private void Start()
    {
        // initial setup
        RefreshCityData();
    }

    /// <summary>
    /// Public method — call this after PlayerPrefs.City is updated (eg. after signup/verify or Next button)
    /// </summary>
    public void RefreshCityData()
    {
        // ensure PlayerPrefs flushed
        PlayerPrefs.Save();

        // restart coroutine to apply latest data
        if (setupCoroutine != null)
            StopCoroutine(setupCoroutine);

        setupCoroutine = StartCoroutine(SetupAfterDelay());
        Debug.Log("CityNameSetup: RefreshCityData() called");
    }

    private IEnumerator SetupAfterDelay()
    {
        // small delay to ensure any UI changes / PlayerPrefs write completed
        yield return new WaitForEndOfFrame();

        // load latest city (fallback to Ahmedabad if empty)
        string cityName = PlayerPrefs.GetString("City", "Ahmedabad");
        if (string.IsNullOrEmpty(cityName))
        {
            cityName = "Ahmedabad";
            PlayerPrefs.SetString("City", cityName);
            PlayerPrefs.Save();
        }

        // Apply city names safely
        ApplyCityNames(flightCityName, cityName);
        ApplyCityNames(trainCityName, cityName);
        ApplyCityNames(basCityName, cityName);

        // Assign data
        AssignFlightData();
        AssignTrainData();
        AssignBusData();

        Debug.Log("CityNameSetup: SetupAfterDelay completed for city: " + cityName);
    }

    private void ApplyCityNames(Text[] textArray, string city)
    {
        if (textArray == null) return;
        for (int i = 0; i < textArray.Length; i++)
            if (textArray[i] != null) textArray[i].text = city;
    }

    private void AssignFlightData()
    {
        if (flightPriceTexts == null || flightSeatTexts == null) return;

        int count = Mathf.Min(Mathf.Min(flightPriceTexts.Length, flightSeatTexts.Length),
                              Mathf.Min(flightStartTimeTexts != null ? flightStartTimeTexts.Length : int.MaxValue,
                                        flightEndTimeTexts != null ? flightEndTimeTexts.Length : int.MaxValue));

        // If there is no start/end arrays or they are shorter, we'll still set price/seat for all present
        count = Mathf.Max(count, 0);

        // set prices & seats for whatever arrays exist (independent)
        for (int i = 0; i < flightPriceTexts.Length; i++)
            if (flightPriceTexts[i] != null)
                flightPriceTexts[i].text = "₹" + UnityEngine.Random.Range(4999, 9999).ToString("N0");

        for (int i = 0; i < flightSeatTexts.Length; i++)
            if (flightSeatTexts[i] != null)
            {
                flightSeatTexts[i].text = UnityEngine.Random.Range(20, 80) + " Seats";
                flightSeatTexts[i].color = Color.green;
            }

        // set times up to min(start,end) length
        if (flightStartTimeTexts != null && flightEndTimeTexts != null)
        {
            int timeCount = Mathf.Min(flightStartTimeTexts.Length, flightEndTimeTexts.Length);
            for (int i = 0; i < timeCount; i++)
                AssignRandomTimeAtIndex(flightStartTimeTexts, flightEndTimeTexts, i, 1);
        }
    }

    private void AssignTrainData()
    {
        if (trainPriceTexts == null || trainSeatTexts == null) return;

        for (int i = 0; i < trainPriceTexts.Length; i++)
            if (trainPriceTexts[i] != null)
                trainPriceTexts[i].text = "₹" + UnityEngine.Random.Range(999, 4999).ToString("N0");

        for (int i = 0; i < trainSeatTexts.Length; i++)
            if (trainSeatTexts[i] != null)
            {
                trainSeatTexts[i].text = UnityEngine.Random.Range(15, 60) + " Seats";
                trainSeatTexts[i].color = Color.green;
            }

        if (trainStartTimeTexts != null && trainEndTimeTexts != null)
        {
            int timeCount = Mathf.Min(trainStartTimeTexts.Length, trainEndTimeTexts.Length);
            for (int i = 0; i < timeCount; i++)
            {
                int duration = UnityEngine.Random.Range(6, 9); // 6-8 hours
                AssignRandomTimeAtIndex(trainStartTimeTexts, trainEndTimeTexts, i, duration);
            }
        }
    }

    private void AssignBusData()
    {
        if (basPriceTexts == null || basSeatTexts == null) return;

        for (int i = 0; i < basPriceTexts.Length; i++)
            if (basPriceTexts[i] != null)
                basPriceTexts[i].text = "₹" + UnityEngine.Random.Range(499, 1000).ToString("N0");

        for (int i = 0; i < basSeatTexts.Length; i++)
            if (basSeatTexts[i] != null)
            {
                basSeatTexts[i].text = UnityEngine.Random.Range(8, 30) + " Seats";
                basSeatTexts[i].color = Color.green;
            }

        if (basStartTimeTexts != null && basEndTimeTexts != null)
        {
            int timeCount = Mathf.Min(basStartTimeTexts.Length, basEndTimeTexts.Length);
            for (int i = 0; i < timeCount; i++)
            {
                int duration = UnityEngine.Random.Range(10, 13); // 10-12 hours
                AssignRandomTimeAtIndex(basStartTimeTexts, basEndTimeTexts, i, duration);
            }
        }
    }

    private void AssignRandomTimeAtIndex(Text[] startTexts, Text[] endTexts, int index, int durationHours)
    {
        if (startTexts == null || endTexts == null) return;
        if (index < 0 || index >= startTexts.Length || index >= endTexts.Length) return;

        DateTime start = DateTime.Today.AddHours(UnityEngine.Random.Range(0, 24)).AddMinutes(UnityEngine.Random.Range(0, 60));
        DateTime end = start.AddHours(durationHours);

        startTexts[index].text = start.ToString("HH:mm");
        endTexts[index].text = end.ToString("HH:mm");
    }


    public void AllPopUpClose()
    {
        profileScreenPopup.SetActive(false);
        oTPScreenScreenPopup.SetActive(false);
        TravelScreenPopup.SetActive(false);
    }


}
