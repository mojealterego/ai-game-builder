using UnityEngine;

public class GameManager : MonoBehaviour
{
    public OracleBrain oracle;

    void Update()
    {
        // Symulacja dla testów (bez mikrofonu)
        if (Input.GetKeyDown(KeyCode.Space))
            oracle.ProcessVoiceRequest("Daj mi złoto!"); 
        
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log("Symulacja zakupu..."); 
    }
}
