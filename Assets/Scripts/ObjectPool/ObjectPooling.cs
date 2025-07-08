using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;
    //[SerializeField] private GameObject bulletPrefab; // Prefab cua dan
    [SerializeField] private int poolSize = 10; // So luong dan toi da trong pool
    //private Queue<GameObject> bulletPool; // Hang doi cho dan
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>(); // Tu dien de quan ly nhieu loai doi tuong pool
    [Header("Initialization")]
    [SerializeField] private GameObject weaponPickup; // Danh sach cac prefab can pool
    [SerializeField] private GameObject ammoPickup; // Danh sach cac prefab can pool
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // giu doi tuong khi chuyen scene
        }
        else
        {
            Destroy(gameObject); // neu da co Instance thi huy doi tuong nay
        }
    }
    private void Start()
    {
        
        CreateInitialPool(weaponPickup); // Tao hang doi ban dau cho vu khi pickup
        CreateInitialPool(ammoPickup); // Tao hang doi ban dau cho hop dan pickup

    }

    public GameObject GetObject(GameObject prefabs,Transform target) 
    {
        if(!poolDictionary.ContainsKey(prefabs)) // Neu chua co hang doi cho prefab nay
        {
            CreateInitialPool(prefabs); // Tao hang doi ban dau
        }
        if (poolDictionary[prefabs].Count == 0) // Neu hang doi da het doi tuong
        {
            CreateNewObject(prefabs); // Tao mot doi tuong moi
        }
        GameObject objectToGet = poolDictionary[prefabs].Dequeue(); // Lay dan tu hang doi
                                                                   // bulletPool.Dequeue(); // Lay dan tu hang doi
        objectToGet.transform.position = target.position; // Dat vi tri cua dan theo vi tri cua doi tuong
        objectToGet.transform.parent = null; // Bo quan he cha con de dan co the di chuyen tu do
        objectToGet.SetActive(true); // Bat dan
        return objectToGet; // Tra ve dan
    }
    private void CreateInitialPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>(); // Khoi tao hang doi cho prefab
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newobject = Instantiate(prefab, transform); // Tao dan moi tu prefab
        //newobject.AddComponent<PoolObject>().originalObject = prefab; // Them component PoolObject de quan ly doi tuong                                                           
        PoolObject poolObj = newobject.GetComponent<PoolObject>();
        if (poolObj == null)
        {
            poolObj = newobject.AddComponent<PoolObject>();
        }
        poolObj.originalObject = prefab; 
        newobject.SetActive(false); // Tat dan moi
        //bulletPool.Enqueue(newobject); // Dua dan moi vao hang doi
        poolDictionary[prefab].Enqueue(newobject); // Dua dan moi vao tu dien

    }

    public void ReturnToPool(GameObject objectReturn)
    {

        //bulletPool.Enqueue(bullet); // Dua dan vao hang doi
        GameObject originalPrefab = objectReturn.GetComponent<PoolObject>().originalObject; // Lay prefab goc cua dan
        objectReturn.SetActive(false); // Tat dan
        objectReturn.transform.parent = transform; // Gan dan ve lai cha de quan ly
        poolDictionary[originalPrefab].Enqueue(objectReturn); // Dua dan vao tu dien
        //poolDictionary[bulletPrefab].Enqueue(bullet); // Dua dan vao tu dien
      
    }
    public void ReturnObject(GameObject objectToReturn, float delay = 0.001f)
    {
        StartCoroutine(DelayReturn(delay, objectToReturn)); // Bat dau ham tra ve doi tuong sau mot khoang thoi gian
    }
    private IEnumerator DelayReturn(float delay,GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay); // Cho mot khoang thoi gian truoc khi tra ve hang doi
        ReturnToPool(objectToReturn); // Tra ve doi tuong vao hang doi
    }
}
