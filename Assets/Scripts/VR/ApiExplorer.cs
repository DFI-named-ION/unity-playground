using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ApiExplorer : MonoBehaviour
{
    public TMP_InputField InputCity;
    public TextMeshProUGUI OutputResponseStatusLabel;
    public TextMeshProUGUI OutputResponseDataLabel;

    public void OnButtonGetForecastClick()
    {
        StartCoroutine(GetForecast());
    }

    private IEnumerator GetForecast()
    {
        using (var request = UnityWebRequest.Get(
            $"https://api.weatherapi.com/v1/current.json?key=046d0bb3145b43789fb210348230102&q={InputCity.text}&aqi=no"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OutputResponseStatusLabel.text = $"Error: {request.error}";
                yield break;
            }
            
            OutputResponseStatusLabel.text = "Success";

            var json = request.downloadHandler.text;
            OutputResponseDataLabel.text = $"Country: {GetStringValue(json, "country")}\n" +
                $"City: {GetStringValue(json, "name")}\n" +
                $"Temperature in c: {GetNumberValue(json, "temp_c")}\n" +
                $"Condition: {GetStringValue(json, "text")}\n" +
                $"Wind direction: {GetStringValue(json, "wind_dir")}\n" +
                $"Wind speed in km: {GetNumberValue(json, "wind_kph")}\n" +
                $"Humidity: {GetNumberValue(json, "humidity")}";
        }
    }

    private string GetStringValue(string json, string key)
    {
        var k = $"\"{key}\"";
        int i = json.IndexOf(k);
        if (i < 0) return "N/A";

        i = json.IndexOf('"', json.IndexOf(':', i) + 1) + 1;
        int j = json.IndexOf('"', i);
        return json.Substring(i, j - i);
    }

    private string GetNumberValue(string json, string key)
    {
        var k = $"\"{key}\"";
        int i = json.IndexOf(k);
        if (i < 0) return "N/A";

        i = json.IndexOf(':', i) + 1;
        int j = json.IndexOfAny(new[] { ',', '}' }, i);
        if (j < 0) j = json.Length;
        return json.Substring(i, j - i).Trim();
    }
}