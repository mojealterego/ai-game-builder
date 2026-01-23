using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing.Extension;
using System.Collections;

public class GameAllInOne : MonoBehaviour, IDetailedStoreListener, IUnityAdsInitializationListener
{
    // --- KONFIGURACJA ---
    private Texture2D titleTexture, oracleTexture;
    private bool gameStarted = false;
    private IStoreController m_StoreController;
    
    // PRODUKTY
    public const string ID_MICRO = "coin_pack_micro"; // 4.99 PLN
    public const string ID_GOD = "coin_pack_god";     // 499.99 PLN
    public const string ID_SUB = "oracle_covenant";   // Subskrypcja
    
    // REKLAMY
    string adGameId = "1234567"; 
    string adUnitId = "Interstitial_Android";
    
    private bool isCovenantActive = false;

    void Start()
    {
        // 1. Grafika
        titleTexture = Resources.Load<Texture2D>("TitleScreen");
        oracleTexture = Resources.Load<Texture2D>("Oracle");

        // 2. Sklep
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(ID_MICRO, ProductType.Consumable);
        builder.AddProduct(ID_GOD, ProductType.Consumable);
        builder.AddProduct(ID_SUB, ProductType.Subscription);
        UnityPurchasing.Initialize(this, builder);

        // 3. Reklamy
        Advertisement.Initialize(adGameId, false, this);
    }

    void OnGUI()
    {
        Rect full = new Rect(0, 0, Screen.width, Screen.height);
        if (!gameStarted)
        {
            if(titleTexture) GUI.DrawTexture(full, titleTexture, ScaleMode.ScaleAndCrop);
            if (GUI.Button(full, "", GUIStyle.none)) gameStarted = true;
        }
        else
        {
            if(oracleTexture) GUI.DrawTexture(full, oracleTexture, ScaleMode.ScaleAndCrop);
            
            // PRZYCISKI
            GUIStyle bStyle = new GUIStyle(GUI.skin.button); bStyle.fontSize = 40;
            if (GUI.Button(new Rect(50, Screen.height - 300, Screen.width - 100, 150), "SZEPNIJ ŻYCZENIE", bStyle)) 
                StartCoroutine(ProcessRitual());

            if (GUI.Button(new Rect(50, 50, 500, 100), "KUP OFIARĘ (4.99 zł)", bStyle)) BuyProduct(ID_MICRO);
            if (GUI.Button(new Rect(50, 170, 500, 100), "PAKIET BOGA (499.99 zł)", bStyle)) BuyProduct(ID_GOD);
        }
    }

    IEnumerator ProcessRitual()
    {
        yield return new WaitForSeconds(1.0f);
        // 10% szansy dla darmowych graczy
        float chance = isCovenantActive ? 0.90f : 0.10f; 
        
        if (Random.value > chance)
        {
            Debug.Log("PRZEGRANA. REKLAMA!");
            if (Advertisement.IsReady(adUnitId)) Advertisement.Show(adUnitId);
        }
    }

    void BuyProduct(string id) => m_StoreController?.InitiatePurchase(id);
    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        if(args.purchasedProduct.definition.id != ID_MICRO) isCovenantActive = true;
        return PurchaseProcessingResult.Complete;
    }
    
    public void OnInitialized(IStoreController c, IExtensionProvider e) { m_StoreController = c; }
    public void OnInitializeFailed(InitializationFailureReason e) {}
    public void OnInitializeFailed(InitializationFailureReason e, string m) {}
    public void OnPurchaseFailed(Product p, PurchaseFailureReason r) {}
    public void OnInitializationComplete() {}
    public void OnInitializationFailed(UnityAdsInitializationError e, string m) {}
}
