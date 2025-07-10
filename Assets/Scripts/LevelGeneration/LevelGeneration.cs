using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private List<Transform> levelParts; //Danh sach cac prefab phan cua level
    private List<Transform> currentLevelParts; // Danh sach ban sao cua levelParts de khong anh huong toi levelParts goc
    [SerializeField] private Transform lastLevel; // Phan cuoi cung cua level, phan nay se duoc sinh ra khi khong con phan nao trong currentLevelParts
    [SerializeField] private SnapPoint nextSnapPoint;
    [SerializeField] private List<Transform> generatedParts = new List<Transform>();
    private SnapPoint defaultSnapPoint;
    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;
    private void Start()
    {
      
        defaultSnapPoint = nextSnapPoint; 
        InitalizeLevelParts();
    }
    private void Update()
    {
        if (generationOver)
            return;
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            if (currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevel();
            }
            else if (!generationOver)
            {
                FinishGenerate();
            }
        }
    }

    private void FinishGenerate()
    {
        generationOver = true;
        GenerateNextLevel();
    }

    [ContextMenu("Generate Level")]
    private void GenerateNextLevel()
    {
        Transform newPart = null; 
        if (generationOver) //Neu da sinh xong thi dung manh cuoi con neu chua thi khoi tao cac manh ngau nhien khac nhau
        {
            newPart = Instantiate(lastLevel);
        }
        else
        {
            newPart = Instantiate(ChooseRandomPart());
        }
        generatedParts.Add(newPart); //Ghi nho cac manh dc sinh ra
        LevelParts levelPart = newPart.GetComponent<LevelParts>();  //Gan script LevelParts cho manh moi sinh ra
        levelPart.SnapAndAlignPart(nextSnapPoint); //Xoay va dinh vi manh moi sinh ra vao snap point tiep theo
        if (levelPart.IntersectionDetect()) //Neu phat hien giao nhau voi cac manh da sinh ra
        {
            Debug.LogWarning("Intersection detected! Regenerating the part.");
            InitalizeLevelParts(); //Khoi tao lai cac manh de sinh ra
            return;

        }
        nextSnapPoint = levelPart.GetExitSnapPoint(); //Cap nhat snap point tiep theo de sinh ra manh tiep theo
    }
    [ContextMenu("Reset Level Generation")]
    private void InitalizeLevelParts()
    {
        nextSnapPoint = defaultSnapPoint; //dat lai snap point ve mac dinh
        generationOver = false; //Dat lai trang thai sinh xong
        currentLevelParts = new List<Transform>(levelParts); //cop danh sach cac phan cua level de sinh ra, tranh viec xoa phan goc
        DestroyOldLevelPart(); //Xoa cac phan cu da sinh ra  truoc do
    }

    private void DestroyOldLevelPart()
    {
        foreach (Transform part in generatedParts) // Huy toan bo cac level part cu 
        {
            Destroy(part.gameObject);
        }
        generatedParts = new List<Transform>(); //khoi tao lai danh sach
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);
        Transform choosenPart = currentLevelParts[randomIndex];
        currentLevelParts.RemoveAt(randomIndex);
        return choosenPart;
    }
    //Tong ket: bat dau goi ham initializeLevelParts() de khoi tao snap point mac dinh -> moi vai giay goi generation -> snap + xoay align lai cac manh ->neu het thi goi toi part last level -> neu cac part va cham vao nhau trong qua trinh init thi reset lai
}
