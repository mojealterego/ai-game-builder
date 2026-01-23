using UnityEngine;

public class SimpleVisuals : MonoBehaviour
{
    private Texture2D titleTexture;
    private Texture2D oracleTexture;
    private bool gameStarted = false;
    public OracleBrain brain; 

    void Start()
    {
        // Ładuje pliki, które zaraz wgrasz do folderu Resources
        titleTexture = Resources.Load<Texture2D>("TitleScreen");
        oracleTexture = Resources.Load<Texture2D>("Oracle");
    }

    void OnGUI()
    {
        Rect fullScreen = new Rect(0, 0, Screen.width, Screen.height);

        if (!gameStarted)
        {
            // EKRAN STARTOWY
            if (titleTexture != null) 
                GUI.DrawTexture(fullScreen, titleTexture, ScaleMode.ScaleAndCrop);
            
            // Kliknięcie gdziekolwiek startuje grę
            if (GUI.Button(fullScreen, "", GUIStyle.none)) 
            {
                gameStarted = true;
            }
        }
        else
        {
            // EKRAN GRY (WYROCZNIA)
            if (oracleTexture != null) 
                GUI.DrawTexture(fullScreen, oracleTexture, ScaleMode.ScaleAndCrop);
        }
    }
}
