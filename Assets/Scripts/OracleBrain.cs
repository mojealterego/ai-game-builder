using UnityEngine;
using System.Collections;

public class OracleBrain : MonoBehaviour
{
    public enum OracleMood { Neutral, Benevolent, Angry, Obsessive }
    public OracleMood currentMood = OracleMood.Neutral;
    public float baseWinChance = 0.30f;
    public bool isCovenantActive = false;

    public void ProcessVoiceRequest(string text)
    {
        Debug.Log($"Gracz szepcze: {text}");
        
        // Ukryte komendy (Wirusowość)
        if (text.ToLower().Contains("aperio fatum"))
        {
            GiveJackpot("Sekretne słowa otworzyły wrota!");
            return;
        }

        // Zmiana nastroju
        if (text.ToLower().Contains("błagam")) 
        {
            currentMood = OracleMood.Benevolent;
            StartCoroutine(PerformRitual());
        }
    }

    public void ReceiveOffering(string productId)
    {
        // Reakcja na zakupy (IAP)
        if (productId == "oracle_covenant")
        {
            isCovenantActive = true;
            currentMood = OracleMood.Obsessive;
            Debug.Log("Oracle: Przymierze zawarte.");
        }
        else
        {
            currentMood = OracleMood.Benevolent;
            Debug.Log("Oracle: Ofiara przyjęta.");
        }
    }

    private IEnumerator PerformRitual()
    {
        Debug.Log("Oracle: *Dźwięk kości*...");
        yield return new WaitForSeconds(2.0f);
        
        float roll = Random.value;
        float finalChance = baseWinChance;

        // Algorytm "Omamienia"
        if (isCovenantActive) finalChance = 0.85f;
        else if (currentMood == OracleMood.Benevolent) finalChance = 0.60f;

        if (roll <= finalChance)
        {
            Debug.Log("<color=gold>WYGRANA!</color>");
        }
        else
        {
            Debug.Log("<color=red>PRZEGRANA.</color>");
            if (!isCovenantActive) Debug.Log("Sugestia: Złóż ofiarę...");
        }
    }

    private void GiveJackpot(string reason)
    {
        Debug.Log($"$$$ JACKPOT $$$: {reason}");
    }
}
