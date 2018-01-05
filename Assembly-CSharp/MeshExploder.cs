// Decompiled with JetBrains decompiler
// Type: MeshExploder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mesh/Mesh Exploder")]
public class MeshExploder : MonoBehaviour
{
    private static Dictionary<Mesh, MeshExploder.MeshExplosionPreparation> cache = new Dictionary<Mesh, MeshExploder.MeshExplosionPreparation>();
    public float minSpeed = 1f;
    public float maxSpeed = 5f;
    public float minRotationSpeed = 90f;
    public float maxRotationSpeed = 360f;
    public float fadeWaitTime = 0.5f;
    public float fadeTime = 2f;
    public float colliderThickness = 0.125f;
    public MeshExploder.ExplosionType type;
    public bool useGravity;
    public bool useNormals;
    public bool useMeshBoundsCenter;
    public bool allowShadows;
    public bool shadersAlreadyHandleTransparency;
    private MeshExploder.MeshExplosionPreparation preparation;

    private string ComponentName
    {
        get
        {
            return this.GetType().Name;
        }
    }

    private void Start()
    {
        MeshFilter component = this.GetComponent<MeshFilter>();
        if ((Object)component == (Object)null)
        {
            if ((Object)this.GetComponent<SkinnedMeshRenderer>() != (Object)null)
                return;
            Debug.LogError((object)(this.ComponentName + " must be on a GameObject with a MeshFilter or SkinnedMeshRenderer component."));
        }
        else
        {
            Mesh sharedMesh = component.sharedMesh;
            if ((Object)sharedMesh == (Object)null)
                Debug.LogError((object)"The MeshFilter does not reference a mesh.");
            else
                this.Prepare(sharedMesh, true);
        }
    }

    private void PrepareWithoutCaching(Mesh oldMesh)
    {
        this.Prepare(oldMesh, false);
    }

    private void Prepare(Mesh oldMesh, bool cachePreparation = true)
    {
        if (!oldMesh.isReadable)
        {
            Debug.LogError((object)"The mesh is not readable. Switch on the \"Read/Write Enabled\" option on the mesh's import settings.");
        }
        else
        {
            bool flag1 = this.type == MeshExploder.ExplosionType.Physics;
            MeshExploder.MeshExplosionPreparation explosionPreparation;
            if (MeshExploder.cache.TryGetValue(oldMesh, out explosionPreparation) && (flag1 && explosionPreparation.physicsMeshes != null || !flag1 && (Object)explosionPreparation.startMesh != (Object)null))
            {
                this.preparation = explosionPreparation;
            }
            else
            {
                Vector3[] vertices1 = oldMesh.vertices;
                Vector3[] normals1 = oldMesh.normals;
                Vector4[] tangents1 = oldMesh.tangents;
                Vector2[] uv = oldMesh.uv;
                Vector2[] uv2 = oldMesh.uv2;
                Color[] colors = oldMesh.colors;
                int subMeshCount = oldMesh.subMeshCount;
                int[][] numArray1 = new int[subMeshCount][];
                int[] numArray2 = !flag1 ? (int[])null : (explosionPreparation.frontMeshTrianglesPerSubMesh = new int[subMeshCount]);
                int length1 = 0;
                for (int submesh = 0; submesh < subMeshCount; ++submesh)
                {
                    int num = (numArray1[submesh] = oldMesh.GetTriangles(submesh)).Length / 3;
                    if (flag1)
                        numArray2[submesh] = num;
                    length1 += num;
                }
                explosionPreparation.totalFrontTriangles = length1;
                int num1 = length1 * 2;
                int length2 = !flag1 ? num1 * 3 : 6;
                if (length2 > 65534)
                {
                    Debug.LogError((object)("The mesh has too many triangles to explode. It must have " + (object)10922 + " or fewer triangles."));
                }
                else
                {
                    Vector3[] vertices2 = new Vector3[length2];
                    Vector3[] normals2 = normals1 == null || normals1.Length == 0 ? (Vector3[])null : new Vector3[length2];
                    Vector4[] tangents2 = tangents1 == null || tangents1.Length == 0 ? (Vector4[])null : new Vector4[length2];
                    Vector2[] vector2Array1 = uv == null || uv.Length == 0 ? (Vector2[])null : new Vector2[length2];
                    Vector2[] vector2Array2 = uv2 == null || uv2.Length == 0 ? (Vector2[])null : new Vector2[length2];
                    Color[] colorArray = colors == null || colors.Length == 0 ? (Color[])null : new Color[length2];
                    Vector3[] vector3Array1 = explosionPreparation.triangleCentroids = new Vector3[length1];
                    Mesh[] meshArray = explosionPreparation.physicsMeshes = !flag1 ? (Mesh[])null : new Mesh[length1];
                    Quaternion[] quaternionArray = explosionPreparation.rotations = !flag1 ? (Quaternion[])null : new Quaternion[length1];
                    int[] numArray3;
                    if (flag1)
                        numArray3 = new int[6] { 0, 1, 2, 3, 4, 5 };
                    else
                        numArray3 = (int[])null;
                    int[] numArray4 = numArray3;
                    int index1 = 0;
                    int index2 = 0;
                    Quaternion quaternion = Quaternion.identity;
                    for (int index3 = 0; index3 < subMeshCount; ++index3)
                    {
                        int[] numArray5 = numArray1[index3];
                        int length3 = numArray5.Length;
                        int num2 = 0;
                        while (num2 < length3)
                        {
                            int num3 = num2;
                            Vector3 vector3_1 = Vector3.zero;
                            for (int index4 = 0; index4 < 2; ++index4)
                            {
                                num2 = num3;
                                bool flag2 = index4 == 1;
                                while (num2 < length3)
                                {
                                    int index5 = numArray5[!flag2 ? num2 : num3 + (2 - (num2 - num3))];
                                    if (flag1 && index1 % 6 == 0)
                                    {
                                        Vector3 vector3_2 = vertices1[index5];
                                        Vector3 vector3_3 = vertices1[numArray5[num2 + 1]];
                                        Vector3 vector3_4 = vertices1[numArray5[num2 + 2]];
                                        Vector3 vector3_5 = Vector3.Cross(vector3_3 - vector3_2, vector3_4 - vector3_2);
                                        quaternionArray[index2] = Quaternion.FromToRotation(Vector3.up, vector3_5);
                                        quaternion = Quaternion.FromToRotation(vector3_5, Vector3.up);
                                        vector3Array1[index2] = vector3_1 = (vector3_2 + vector3_3 + vector3_4) / 3f;
                                    }
                                    if (!flag2)
                                    {
                                        vertices2[index1] = quaternion * (vertices1[index5] - vector3_1);
                                        if (normals2 != null)
                                            normals2[index1] = quaternion * normals1[index5];
                                        if (tangents2 != null)
                                            tangents2[index1] = (Vector4)(quaternion * (Vector3)tangents1[index5]);
                                    }
                                    if (vector2Array1 != null)
                                        vector2Array1[index1] = uv[index5];
                                    if (vector2Array2 != null)
                                        vector2Array2[index1] = uv2[index5];
                                    if (colorArray != null)
                                        colorArray[index1] = colors[index5];
                                    ++num2;
                                    ++index1;
                                    if (index1 % 6 == 0)
                                    {
                                        if (flag1)
                                        {
                                            MeshExplosion.SetBackTriangleVertices(vertices2, normals2, tangents2, 1);
                                            Mesh mesh = new Mesh();
                                            mesh.vertices = vertices2;
                                            if (normals2 != null)
                                                mesh.normals = normals2;
                                            if (tangents2 != null)
                                                mesh.tangents = tangents2;
                                            if (vector2Array1 != null)
                                                mesh.uv = vector2Array1;
                                            if (vector2Array2 != null)
                                                mesh.uv2 = vector2Array2;
                                            if (colorArray != null)
                                                mesh.colors = colorArray;
                                            mesh.triangles = numArray4;
                                            meshArray[index2] = mesh;
                                            index1 = 0;
                                            break;
                                        }
                                        break;
                                    }
                                    if (index1 % 3 == 0 && !flag2)
                                        break;
                                }
                            }
                            ++index2;
                        }
                    }
                    Vector3 vector3_6 = Vector3.zero;
                    if (!flag1)
                    {
                        Mesh mesh = explosionPreparation.startMesh = new Mesh();
                        mesh.MarkDynamic();
                        mesh.vertices = vertices2;
                        if (normals2 != null)
                            mesh.normals = normals2;
                        if (tangents2 != null)
                            mesh.tangents = tangents2;
                        if (vector2Array1 != null)
                            mesh.uv = vector2Array1;
                        if (vector2Array2 != null)
                            mesh.uv2 = vector2Array2;
                        if (colorArray != null)
                            mesh.colors = colorArray;
                        mesh.subMeshCount = subMeshCount;
                        int num2 = 0;
                        for (int submesh = 0; submesh < subMeshCount; ++submesh)
                        {
                            int length3 = numArray1[submesh].Length * 2;
                            int[] triangles = new int[length3];
                            int index3 = 0;
                            while (index3 < length3)
                            {
                                triangles[index3] = num2;
                                ++index3;
                                ++num2;
                            }
                            mesh.SetTriangles(triangles, submesh);
                        }
                        if (this.useMeshBoundsCenter)
                            vector3_6 = mesh.bounds.center;
                    }
                    Vector3[] vector3Array2 = explosionPreparation.triangleNormals = new Vector3[length1];
                    int index6 = 0;
                    int index7 = 0;
                    while (index7 < length1)
                    {
                        Vector3 vector3_1;
                        if (flag1)
                        {
                            vector3_1 = vector3Array1[index7];
                        }
                        else
                        {
                            vector3_1 = (vertices2[index6] + vertices2[index6 + 1] + vertices2[index6 + 2]) / 3f;
                            vector3Array1[index7] = vector3_1;
                        }
                        Vector3 vector3_2;
                        if (this.useNormals && normals2 != null)
                        {
                            if (flag1)
                            {
                                normals2 = meshArray[index7].normals;
                                index6 = 0;
                            }
                            vector3_2 = ((normals2[index6] + normals2[index6 + 1] + normals2[index6 + 2]) / 3f).normalized;
                        }
                        else
                        {
                            vector3_2 = vector3_1;
                            if (this.useMeshBoundsCenter)
                                vector3_2 -= vector3_6;
                            vector3_2.Normalize();
                        }
                        vector3Array2[index7] = vector3_2;
                        ++index7;
                        index6 += 6;
                    }
                    this.preparation = explosionPreparation;
                    if (cachePreparation)
                        MeshExploder.cache[oldMesh] = explosionPreparation;
                    if ((double)this.fadeTime == 0.0 || this.shadersAlreadyHandleTransparency)
                        return;
                    foreach (Material sharedMaterial in this.GetComponent<Renderer>().sharedMaterials)
                    {
                        Shader shader = sharedMaterial.shader;
                        Shader replacementFor = Fade.GetReplacementFor(shader);
                        if ((Object)replacementFor == (Object)null || !replacementFor.name.StartsWith("Transparent/"))
                            Debug.LogWarning((object)("Couldn't find an explicitly transparent version of shader '" + shader.name + "' so fading may not work. If the shader does support transparency then this warning can be avoided by enabling the 'Shaders Already Handle Transparency' option."));
                    }
                }
            }
        }
    }

    public GameObject Explode()
    {
        if ((Object)this.preparation.startMesh == (Object)null && this.preparation.physicsMeshes == null)
        {
            SkinnedMeshRenderer component = this.GetComponent<SkinnedMeshRenderer>();
            if ((Object)component == (Object)null)
                return (GameObject)null;
            if ((Object)component.sharedMesh == (Object)null)
            {
                Debug.LogError((object)"The SkinnedMeshRenderer does not reference a mesh.");
                return (GameObject)null;
            }
            Mesh mesh = new Mesh();
            component.BakeMesh(mesh);
            this.PrepareWithoutCaching(mesh);
        }
        string name = this.gameObject.name + " (Mesh Explosion)";
        if (this.type == MeshExploder.ExplosionType.Physics)
        {
            Mesh[] physicsMeshes = this.preparation.physicsMeshes;
            Quaternion[] rotations = this.preparation.rotations;
            float num1 = this.maxSpeed - this.minSpeed;
            float num2 = this.maxRotationSpeed - this.minRotationSpeed;
            bool flag1 = (double)this.minSpeed == (double)this.maxSpeed;
            bool flag2 = (double)this.minRotationSpeed == (double)this.maxRotationSpeed;
            Vector3[] triangleCentroids = this.preparation.triangleCentroids;
            Vector3[] triangleNormals = this.preparation.triangleNormals;
            Transform transform1 = this.transform;
            Quaternion rotation1 = transform1.rotation;
            Vector3 position = transform1.position;
            Vector3 lossyScale = transform1.lossyScale;
            bool flag3 = lossyScale != Vector3.one;
            int length1 = physicsMeshes.Length;
            int index1 = 0;
            Material[] materials = this.GetComponent<Renderer>().materials;
            int[] trianglesPerSubMesh = this.preparation.frontMeshTrianglesPerSubMesh;
            int num3 = trianglesPerSubMesh[0];
            Material material = (Material)null;
            float num4 = this.fadeWaitTime + this.fadeTime;
            int index2 = 0;
            int num5 = 0;
            while (index2 < length1)
            {
                if (num5 == num3)
                {
                    num5 = 0;
                    ++index1;
                    num3 = trianglesPerSubMesh[index1];
                    material = (Material)null;
                }
                GameObject gameObject = this.SetUpExplosionPiece(name, (Object)material == (Object)null);
                if ((Object)material == (Object)null)
                {
                    material = materials[index1];
                    if ((double)this.fadeTime != 0.0)
                        gameObject.GetComponent<Fade>().materials = new Material[1]
                        {
              material
                        };
                }
                else if ((double)this.fadeTime != 0.0)
                    gameObject.AddComponent<DestroyAfterTime>().waitTime = num4;
                gameObject.GetComponent<MeshRenderer>().sharedMaterials = new Material[1]
                {
          material
                };
                Vector3 vector3_1 = Vector3.Scale(triangleCentroids[index2], lossyScale);
                Quaternion quaternion1 = rotations[index2];
                Vector3 vector3_2 = rotation1 * vector3_1 + position;
                Quaternion quaternion2 = rotation1 * quaternion1;
                Transform transform2 = gameObject.transform;
                transform2.localPosition = vector3_2;
                transform2.localRotation = quaternion2;
                Mesh original = this.preparation.physicsMeshes[index2];
                MeshFilter component = gameObject.GetComponent<MeshFilter>();
                if (flag3)
                {
                    component.sharedMesh = original = Object.Instantiate<Mesh>(original);
                    Vector3[] vertices = original.vertices;
                    Vector3[] normals = original.normals;
                    int length2 = vertices.Length;
                    Quaternion rotation2 = rotations[index2];
                    Quaternion quaternion3 = Quaternion.Inverse(rotation2);
                    for (int index3 = 0; index3 < length2; ++index3)
                        vertices[index3] = quaternion3 * Vector3.Scale(rotation2 * vertices[index3], lossyScale);
                    original.vertices = vertices;
                    original.normals = normals;
                }
                else
                    component.sharedMesh = original;
                Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.angularVelocity = Quaternion.AngleAxis(!flag2 ? this.minRotationSpeed + Random.value * num2 : this.minRotationSpeed, Random.onUnitSphere).eulerAngles;
                rigidbody.velocity = (!flag1 ? this.minSpeed + Random.value * num1 : this.minSpeed) * triangleNormals[index2];
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                Vector3 size = original.bounds.size;
                size.y = this.colliderThickness;
                boxCollider.size = size;
                rigidbody.SetDensity(1f);
                ++index2;
                ++num5;
            }
            return (GameObject)null;
        }
        GameObject gameObject1 = this.SetUpExplosionPiece(name, true);
        gameObject1.AddComponent<MeshExplosion>().Go(this.preparation, this.minSpeed, this.maxSpeed, this.minRotationSpeed, this.maxRotationSpeed, this.useGravity, this.transform.lossyScale);
        return gameObject1;
    }

    private GameObject SetUpExplosionPiece(string name, bool addFade = true)
    {
        GameObject gameObject = new GameObject(name);
        Transform transform1 = this.transform;
        Transform transform2 = gameObject.transform;
        transform2.localPosition = transform1.position;
        transform2.localRotation = transform1.rotation;
        gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Renderer component = this.GetComponent<Renderer>();
        meshRenderer.castShadows = component.castShadows;
        meshRenderer.receiveShadows = component.receiveShadows;
        meshRenderer.sharedMaterials = component.sharedMaterials;
        meshRenderer.lightProbeUsage = component.lightProbeUsage;
        if ((double)this.fadeTime != 0.0)
        {
            if (addFade)
            {
                Fade fade = gameObject.AddComponent<Fade>();
                fade.waitTime = this.fadeWaitTime;
                fade.fadeTime = this.fadeTime;
                fade.replaceShaders = !this.shadersAlreadyHandleTransparency;
                gameObject.AddComponent<DestroyOnFadeCompletion>();
            }
            if (!this.allowShadows)
            {
                meshRenderer.castShadows = false;
                meshRenderer.receiveShadows = false;
            }
        }
        return gameObject;
    }

    public enum ExplosionType
    {
        Visual,
        Physics,
    }

    public struct MeshExplosionPreparation
    {
        public Mesh startMesh;
        public Vector3[] triangleNormals;
        public Vector3[] triangleCentroids;
        public int totalFrontTriangles;
        public Mesh[] physicsMeshes;
        public Quaternion[] rotations;
        public int[] frontMeshTrianglesPerSubMesh;
    }
}
