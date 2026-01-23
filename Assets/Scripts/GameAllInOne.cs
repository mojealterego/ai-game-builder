using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing.Extension;
using System.Collections;

public class GameAllInOne : MonoBehaviour, IDetailedStoreListener, IUnityAdsInitializationListener
{
    private Texture2D titleTexture, oracleTexture;
    private bool gameStarted = false;
    private IStoreController m_StoreController;
    
    // --- PEŁNA LISTA PAKIETÓW (CENNIK) ---
    public const string ID_TIER1 = "coin_pack_micro";  // 4.99 PLN
    public const string ID_TIER2 = "coin_pack_small";  // 24.99 PLN
    public const string ID_TIER3 = "coin_pack_medium"; // 49.99 PLN
    public const string ID_TIER4 = "coin_pack_large";  // 99.99 PLN
    public const string ID_TIER5 = "coin_pack_huge";   // 249.99 PLN
    public const string ID_TIER6 = "coin_pack_god";    // 499.99 PLN
    public const string ID_SUB   = "oracle_covenant";  // Subskrypcja
    
    // REKLAMY (Logika dla Android i iOS)
    string androidGameId = "1234567";
    string iosGameId = "7654321"; 
    string adUnitIdAndroid = "Interstitial_Android";
    string adUnitIdIOS = "Interstitial_iOS";
    string adUnitId; 
    
    private bool isCovenantActive = false;

    void Start()
    {
        // 1. Ładowanie Grafik
        titleTexture = Resources.Load<Texture2D>("TitleScreen");
        oracleTexture = Resources.Load<Texture2D>("Oracle");

        // 2. Inicjalizacja Reklam (Wybór systemu)
        string gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? adUnitIdIOS : adUnitIdAndroid;
        Advertisement.Initialize(gameId, false, this);

        // 3. Konfiguracja Sklepu (Wszystkie pakiety)
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(ID_TIER1, ProductType.Consumable);
        builder.AddProduct(ID_TIER2, ProductType.Consumable);
        builder.AddProduct(ID_TIER3, ProductType.Consumable);
        builder.AddProduct(ID_TIER4, ProductType.Consumable);
        builder.AddProduct(ID_TIER5, ProductType.Consumable);
        builder.AddProduct(ID_TIER6, ProductType.Consumable);
        builder.AddProduct(ID_SUB, ProductType.Subscription);
        UnityPurchasing.Initialize(this, builder);
    }

    void OnGUI()
    {
        Rect full = new Rect(0, 0, Screen.width, Screen.height);
        
        if (!gameStarted)
        {
            // Ekran Startowy
            if(titleTexture) GUI.DrawTexture(full, titleTexture, ScaleMode.ScaleAndCrop);
            if (GUI.Button(full, "", GUIStyle.none)) gameStarted = true;
        }
        else
        {
            // Ekran Gry
            if(oracleTexture) GUI.DrawTexture(full, oracleTexture, ScaleMode.ScaleAndCrop);
            
            // Styl Przycisków
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button); 
            btnStyle.fontSize = 35; 
            btnStyle.normal.textColor = Color.yellow;
            
            float w = Screen.width - 100;
            float h = 80;
            float startY = 50;

            // --- MENU ZAKUPÓW (Wszystkie opcje) ---
            if (GUI.Button(new Rect(50, startY, w, h), "OFIARA (4.99 zł)", btnStyle)) BuyProduct(ID_TIER1);
            if (GUI.Button(new Rect(50, startY + 90, w, h), "WOREK ZŁOTA (24.99 zł)", btnStyle)) BuyProduct(ID_TIER2);
            if (GUI.Button(new Rect(50, startY + 180, w, h), "SKRZYNIA (49.99 zł)", btnStyle)) BuyProduct(ID_TIER3);
            if (GUI.Button(new Rect(50, startY + 270, w, h), "SKARBIEC (99.99 zł)", btnStyle)) BuyProduct(ID_TIER4);
            if (GUI.Button(new Rect(50, startY + 360, w, h), "FORTUNA (249.99 zł)", btnStyle)) BuyProduct(ID_TIER5);
            if (GUI.Button(new Rect(50, startY + 450, w, h), "PAKIET BOGA (499.99 zł)", btnStyle)) BuyProduct(ID_TIER6);

            // Przycisk Rytuału (Gra)
            if (GUI.Button(new Rect(50, Screen.height - 150, w, 120), "SZEPNIJ ŻYCZENIE", btnStyle)) 
                StartCoroutine(ProcessRitual());
        }
    }

    IEnumerator ProcessRitual()
    {
        yield return new WaitForSeconds(1.0f);
        // Szansa: 90% dla płacących, 10% dla darmowych
        float chance = isCovenantActive ? 0.90f : 0.10f; 
        
        if (Random.value > chance)
        {
            // Przegrana -> Reklama
            if (Advertisement.IsReady(adUnitId)) Advertisement.Show(adUnitId);
        }
    }

    void BuyProduct(string id) => m_StoreController?.InitiatePurchase(id);
    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        // Każdy zakup aktywuje "Przymierze" (lepsze szanse)
        isCovenantActive = true;
        return PurchaseProcessingResult.Complete;
    }
    
    public void OnInitialized(IStoreController c, IExtensionProvider e) { m_StoreController = c; }
    public void OnInitializeFailed(InitializationFailureReason e) {}
    public void OnInitializeFailed(InitializationFailureReason e, string m) {}
    public void OnPurchaseFailed(Product p, PurchaseFailureReason r) {}
    public void OnInitializationComplete() {}
    public void OnInitializationFailed(UnityAdsInitializationError e, string m) {}
}
