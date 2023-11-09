using System;
using UnityEngine;
using UnityEngine.purchasing;

public class IAPManager : MonoBehaviour, IStoreListener //для получения сообщений из Unity Purchasing
{
    [SerializeField]
    private PlayerDataManager _playerDataManager;

    [SerializeField]
    private GameObject _messagePanel;

    [SerializeField]
    private GameObject _messageSuccessText;

    [SerializeField]
    private GameObject _messageFailText;

    private static IStoreController m_StoreController;          //доступ к системе Unity Purchasing
    private static IExtensionProvider m_StoreExtensionProvider; // подсистемы закупок для конкретных магазинов

    public static string CRYSTAL_PACK = "CrystalsPack"; //многоразовые - consumable
    public static string MEGA_CRYSTAL_PACK = "MegaCrystalsPack"; //многоразовые - consumable
    public static string GOLDEN_CARD = "GoldenCard"; //многоразовые - consumable

    public int _crystalPack;
    public int _megaCrystalPack;

    



    void Start()
    {
        if (m_StoreController == null) //если еще не инициализаровали систему Unity Purchasing, тогда инициализируем
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) //если уже подключены к системе - выходим из функции
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Прописываем свои товары для добавления в билдер
        builder.AddProduct(CRYSTAL_PACK, ProductType.Consumable);
        builder.AddProduct(MEGA_CRYSTAL_PACK, ProductType.Consumable); //или ProductType.Subscription
        builder.AddProduct(GOLDEN_CARD, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyCrystalPack()
    {
        BuyProductID(CRYSTAL_PACK);
    }

    public void BuyMegaCrystalPack()
    {
        BuyProductID(MEGA_CRYSTAL_PACK);
    }

    public void BuyGoldenCard()
    {
        BuyProductID(GOLDEN_CARD);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized()) //если покупка инициализирована 
        {
            Product product = m_StoreController.products.WithID(productId); //находим продукт покупки 

            if (product != null && product.availableToPurchase) //если продукт найдет и готов для продажи
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product); //покупаем
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) //контроль покупок
    {
        if (String.Equals(args.purchasedProduct.definition.id, CRYSTAL_PACK, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            _playerDataManager.TryChangeValue(ValueType.gem, _crystalPack);

            IsSuccessMessage(true);



        }
        else if (String.Equals(args.purchasedProduct.definition.id, MEGA_CRYSTAL_PACK, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            _playerDataManager.TryChangeValue(ValueType.gem, _megaCrystalPack);
            IsSuccessMessage(true);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, GOLDEN_CARD, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            ArtifactPurchaseController.Instance.ActivateArtifact();

            Debug.Log("TRY BUY GOLDEN CARD");




        }

        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void RestorePurchases() //Восстановление покупок (только для Apple). У гугл это автоматический процесс.
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) //если запущенно на эпл устройстве
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        IsSuccessMessage(false);

        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void IsSuccessMessage(bool isSucces)
    {
        _messagePanel.SetActive(true);
        _messageSuccessText.SetActive(isSucces);
        _messageFailText.SetActive(!isSucces);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
