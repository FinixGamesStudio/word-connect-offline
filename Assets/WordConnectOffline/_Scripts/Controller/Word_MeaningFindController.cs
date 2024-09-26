using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WordConnectByFinix
{
    public class Word_MeaningFindController : MonoBehaviour
    {
        public string wordForSearch;
        public List<string> levelWords;
        public List<WordMeaningClass> meaningClass;
        public void GetMeaning(string word)
        {
            wordForSearch = word;
            levelWords.Add(word);
            StartCoroutine(GetRequest($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}"));
        }

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                    Debug.LogError("Error: " + webRequest.error);
                else
                {
                    Debug.Log("Response: " + webRequest.downloadHandler.text);
                    meaningClass = JsonConvert.DeserializeObject<List<WordMeaningClass>>(webRequest.downloadHandler.text);
                    Word_MainController.instance.dictionaryScreenController.SetDictionaryPrefab(meaningClass[0].word, meaningClass[0].meanings[0].definitions);
                }
            }
        }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    [System.Serializable]
    public class Definition
    {
        public string definition;
        public List<string> synonyms;
        public List<string> antonyms;
        public string example;
    }
    [System.Serializable]
    public class License
    {
        public string name;
        public string url;
    }
    [System.Serializable]
    public class Meaning
    {
        public string partOfSpeech;
        public List<Definition> definitions;
        public List<string> synonyms;
        public List<string> antonyms;
    }
    [System.Serializable]
    public class Phonetic
    {
        public string audio;
        public string sourceUrl;
        public License license;
        public string text;
    }
    [System.Serializable]
    public class WordMeaningClass
    {
        public string word;
        public List<Phonetic> phonetics;
        public List<Meaning> meanings;
        public License license;
        public List<string> sourceUrls;
        public string phonetic;
    }

}
