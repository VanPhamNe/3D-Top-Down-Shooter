using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public enum Enemy_MeleeWeaponType { OneHand, Unarm };
public enum Enemy_RangeWeaponType { Pistol,Revolver, Rifle,Shotgun,Sniper }
public class EnemyVisual : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

  
    public GameObject currentWeaponModel { get; private set; }

    [Header("Corruption visuals")]
    [SerializeField] private GameObject[] corruptionCyrstals;
    [SerializeField] private int corruptionAmount = 0;

    [Header("Rig")]
    [SerializeField] private Transform lefthandIK;
    [SerializeField] private Transform leftElbowIK;
    [SerializeField] private Rig rig;
    private void Awake()
    {
       
        CollectCrystal();
    }
    private void Start()
    {
        
       /* InvokeRepeating("SetUpLook", 0f, 1.5f);*/ // Set up the look every 5 seconds
    }

 

    public void SetUpLook()
    {
        SetUpRandomColor();
        SetUpRandomWeapon();
        SetupRandomCyrstal();
    }
    private void SetUpRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);
        Material newMat = new Material(skinnedMeshRenderer.material);
        newMat.mainTexture = colorTextures[randomIndex];
        skinnedMeshRenderer.material = newMat;
    }
    private void SetUpRandomWeapon()
    {
        bool thisEnemyMelee = GetComponent<Enemy_Melee>() != null; // Check if the enemy has a melee component
        bool thisEnemyRanged = GetComponent<EnemyRange>() != null; // Check if the enemy has a ranged component
        if (thisEnemyRanged)
        {
            currentWeaponModel = FindRangeWeaponModel();
        }
        else if (thisEnemyMelee)
        {
            currentWeaponModel = FindMeleeWeaponModel();
        }
      
        
        currentWeaponModel.SetActive(true);
        OverrideAnimatorController();
    }

    private GameObject FindRangeWeaponModel()
    {
        EnemyRange_WeaponModel[] weaponModels = GetComponentsInChildren<EnemyRange_WeaponModel>(true); // Get all weapon models in children
        Enemy_RangeWeaponType weaponType = GetComponent<EnemyRange>().weaponType; // Get the weapon type from the EnemyRange component
        foreach(var weaponModel in weaponModels)
        {
            if(weaponModel.weaponType == weaponType)
            {
                SwitchAnimationLayer((int)weaponModel.holdType);
                SetUpLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget); // Set up the left hand IK targets
                return weaponModel.gameObject; // Return the first matching weapon model
            }
        }
        Debug.LogWarning("No matching weapon model found for type: " + weaponType);
        return null; // Return null if no matching weapon model is found
    }

    private GameObject FindMeleeWeaponModel()
    {
        Enemy_WeaponModel[] weaponModel = GetComponentsInChildren<Enemy_WeaponModel>(true); // Get all weapon models in children
        Enemy_MeleeWeaponType weaponType = GetComponent<Enemy_Melee>().weaponType; // Get the weapon type from the Enemy_Melee component
        List<Enemy_WeaponModel> filtersWeapons = new List<Enemy_WeaponModel>();
        foreach (var weapon in weaponModel)
        {
            if (weapon.weaponType == weaponType)
            {
                filtersWeapons.Add(weapon);
            }
        }

        int randomIndex = Random.Range(0, filtersWeapons.Count);
        return filtersWeapons[randomIndex].gameObject;
    }

    private void OverrideAnimatorController()
    {
        AnimatorOverrideController animatorOverrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>()?.animOverrideController;
        if (animatorOverrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = animatorOverrideController;
        }
    }

    private void SetupRandomCyrstal()
    {
        List<int> avalibleIndex = new List<int>();
        corruptionCyrstals = CollectCrystal(); // Collect all corruption crystals from the children
        for (int i = 0; i < corruptionCyrstals.Length; i++)
        {
            avalibleIndex.Add(i);
            corruptionCyrstals[i].SetActive(false);
        }
        for (int i = 0; i < corruptionAmount; i++)
        {
            if (avalibleIndex.Count == 0) break; // If no more crystals are available, exit the loop
            int randomIndex = Random.Range(0, avalibleIndex.Count);
            int objectIndex = avalibleIndex[randomIndex];
            corruptionCyrstals[objectIndex].SetActive(true);
            avalibleIndex.RemoveAt(randomIndex);
        }
    }
    private GameObject[] CollectCrystal()
    {
        Enemy_Cyrstal[] crystals = GetComponentsInChildren<Enemy_Cyrstal>(true);
        GameObject[] corruptionCyrstals = new GameObject[crystals.Length];
        for (int i = 0; i < crystals.Length; i++)
        {
            corruptionCyrstals[i] = crystals[i].gameObject;
        }
        return corruptionCyrstals;
    }

    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable); // Enable or disable the trail effect for the current weapon model
    }
    private void SwitchAnimationLayer(int layerIndex)
    {
        Animator animator = GetComponentInChildren<Animator>();
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0f); //tat tat ca cac layer
        }
        animator.SetLayerWeight(layerIndex, 1f); //bat layer hien tai
    }
    public void EnableIK(bool enable)
    {
       rig.weight = enable ? 1f : 0f; // Enable or disable the rig weight
    }
    
    private void SetUpLeftHandIK(Transform leftHandTarget, Transform leftElbowTarget)
    {
        lefthandIK.localPosition = leftHandTarget.localPosition; // Set the position of the left elbow IK target
        lefthandIK.localRotation = leftHandTarget.localRotation; // Set the rotation of the left hand IK target

        leftElbowIK.localPosition = leftElbowTarget.localPosition; // Set the position of the left elbow IK target
        leftElbowIK.localRotation = leftElbowTarget.localRotation; // Set the rotation of the left elbow IK target
        

    }


}
