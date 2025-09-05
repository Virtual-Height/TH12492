using UnityEngine;

public class MaterialVariationManager : MonoBehaviour
{
    public Renderer mesh;
    public Material[] materials;

    private void Awake()
    {
        mesh.material = materials[Random.Range(0, materials.Length)];
    }
}
