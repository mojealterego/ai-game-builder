using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System.Collections.Generic;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_StoreController;
    public OracleBrain oracle;

    // ID produktów zgodne z Google Play Console (będziesz musiał je tam wpisać)
    public const string COIN_TIER_1 = "coin_pack_micro"; 
    public const string COIN_TIER_6 = "coin_pack_god";    
    public const string PRODUCT_SUB = "oracle_covenant"; 

    void Start()
    {
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // Definiujemy produkty dla sklepu Google
        builder.AddProduct(COIN_TIER_1, ProductType.Consumable);
        builder.AddProduct(COIN_TIER_6, ProductType.Consumable);
        builder.AddProduct(PRODUCT_SUB, ProductType.Subscription);
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
    }

    // Wywołanie zakupu (podpinasz to pod przyciski)
    public void BuyCheapest() => m_StoreController.InitiatePurchase(COIN_TIER_1);
    public void BuyGodPack() => m_StoreController.InitiatePurchase(COIN_TIER_6);

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // Jak Google potwierdzi przelew, dajemy sygnał do Mózgu
        if (oracle != null) oracle.ReceiveOffering(args.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OfferSmallPack()
    {
        // Agresywny popup (tylko logika, GUI robi SimpleVisuals)
        Debug.Log("POPUP: KUPUJ ALBO PRZEGRYWASZ");
    }

    public void OnInitializeFailed(InitializationFailureReason error) {}
    public void OnInitializeFailed(InitializationFailureReason error, string message) {}
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {}
}
