using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class PersonnageSetup
{
    [MenuItem("Tools/Personnage/Dupliquer et détacher")]
    public static void DupliquerEtDetacher()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/personnage.prefab");
        if (prefab == null)
        {
            Debug.LogError("Prefab Assets/personnage.prefab introuvable.");
            return;
        }

        // Instance A : reste liée au prefab (toute modif du prefab s'y répercute)
        var instanceA = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instanceA.name = "personnage";
        instanceA.transform.position = Vector3.zero;

        // Instance B : copie du prefab, qu'on va détacher
        var instanceB = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instanceB.name = "personnage (détaché)";
        instanceB.transform.position = new Vector3(3f, 0f, 0f);

        // Casse complètement le lien avec le prefab : instanceB devient un GameObject indépendant
        PrefabUtility.UnpackPrefabInstance(instanceB, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        // Donne à l'instance détachée sa propre couleur, sans toucher au prefab ni à l'instance A
        var newMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        newMat.color = Color.red;
        AssetDatabase.CreateAsset(newMat, AssetDatabase.GenerateUniqueAssetPath("Assets/personnage_detache_material.mat"));

        foreach (var renderer in instanceB.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.sharedMaterial = newMat;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Selection.activeGameObject = instanceB;

        Debug.Log("Terminé : 'personnage (détaché)' n'est plus lié au prefab et a sa propre couleur (rouge). Pense à sauvegarder la scène (Cmd+S).");
    }
}
