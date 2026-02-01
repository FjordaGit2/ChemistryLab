using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PeriodTable : MonoBehaviour
{
    [SerializeField] PTEUI PTElementUIPrefab = null;
    [SerializeField] Transform tableParent = null;
    [SerializeField] Transform modelParent = null;
    [SerializeField] Material orbitMaterial = null;
    [SerializeField] Text detailText = null;
    public static bool isInteractable = true;

    private int[] orbitCapacity = new[] { 2, 2, 6, 2, 6, 2, 10, 6, 2, 10, 6, 2, 14, 10, 6, 2 };
    private int[] orbitOrder = new[] { 1, 2, 2, 3, 3, 4, 3, 4, 5, 4, 5, 6, 4, 5, 6, 7 };
    private float nuclearDisplaySize = 0.1f;

    public PTData data;
    public static PeriodTable Instance;
    List<Transform> types = new List<Transform>();

    private int selectedType = -1;
    private int selectedNumber = -1;
    public int SelectedType
    {
        get { return selectedType; }
        set
        {
            if(selectedType != value)
            {
                if(value >= types.Count || value < 0)
                {
                    selectedType = -1;
                    for(int i = 0; i < types.Count; i++)
                    {
                        types[i].GetComponent<CanvasGroup>().alpha = 1f;
                    }
                }
                else
                {
                    selectedType = value;
                    for (int i = 0; i < types.Count; i++)
                    {
                        types[i].GetComponent<CanvasGroup>().alpha = (i == selectedType) ? 1f : 0.5f;
                    }
                }
            }
        }
    }
    private void Awake()
    {
        Instance = this;
        foreach (Transform t in tableParent)
        {
            types.Add(t);
        }
    }
    void Start()
    {
        if (data == null) Destroy(this.gameObject);
        tableParent.GetComponent<Button>().onClick.AddListener(() => { SelectedType = -1; });
        PTEUI.originPt = new Vector2(0, -PTElementUIPrefab.GetComponent<RectTransform>().sizeDelta.x);
        foreach(var d in data.ElementData)
        {
            var eui = Instantiate(PTElementUIPrefab, types[(int)d.chemicalProperty]);
            eui.Init(d);
        }
    }
    public void ElementClick(ChemicalElement e) // this is press event listener function. you can put condition he
    {
        if (selectedNumber == e.Number || !isInteractable) return;
        SelectedType = (int)e.chemicalProperty;
        detailText.text = e.Detail;
        selectedNumber = e.Number;
        if (modelParent.childCount > 0)
        {
            for(int i = modelParent.childCount - 1; i >= 0; i --)
                Destroy(modelParent.GetChild(i).gameObject);
        }

        // Nuclear Generate
        float nuclearSize = 2;
        float overlayRate = 1.5f;
        Transform nuclear = new GameObject("Nuclear").transform;
        if (e.Mass >= 1.5f)
        {
            Vector3[] pos = ArrangeOnSpheer(Mathf.RoundToInt(e.Mass));
            float protonRate = e.Number / (float)pos.Length;
            float temp = 0;
            float distanceBetweenQuantum = (pos[0] - pos[1]).magnitude * overlayRate;
            for(int i = 0; i < pos.Length; i++)
            {
                temp += protonRate;
                var s = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                s.gameObject.layer = 5;
                s.SetParent(nuclear);
                s.localPosition = pos[i] / distanceBetweenQuantum;
                if(temp >= 1)
                {
                    s.GetComponent<MeshRenderer>().material.color = Color.red;
                    temp -= 1;
                }
                else
                {
                    s.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
            nuclearSize = 2 / distanceBetweenQuantum;
        }
        else
        {
            var s = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            s.gameObject.layer = 5;
            s.SetParent(nuclear);
            s.localPosition = Vector3.zero;
            s.GetComponent<MeshRenderer>().material.color = Color.red;
            nuclearSize = 1;
        }
        nuclear.SetParent(modelParent);
        nuclear.localPosition = Vector3.zero;
        nuclear.localScale = Vector3.one * nuclearDisplaySize / nuclearSize;

        // Electron arrange
        int[] electrons = new int[e.Row];
        int remainElectron = e.Number;
        for(int i = 0; i < orbitCapacity.Length; i++)
        {
            if(remainElectron <= orbitCapacity[i])
            {
                electrons[orbitOrder[i] - 1] += remainElectron;
                break;
            }
            electrons[orbitOrder[i] - 1] += orbitCapacity[i];
            remainElectron -= orbitCapacity[i];
        }

        // Electron Orbit Generate
        float orbitAngleDelta = Mathf.PI / (float)e.Row;
        for(int i = 0; i < e.Row; i++)
        {
            var orbit = GenerateOrbit(1.5f + i * 0.5f, 0.02f, electrons[i], 32, orbitAngleDelta * i).transform;
            orbit.name = "Orbit" + (i + 1);
            orbit.SetParent(modelParent);
            orbit.localPosition = Vector3.zero;
            orbit.localScale = Vector3.one * nuclearDisplaySize;
        }
    }
    private Vector3[] ArrangeOnSpheer(int number)
    {
        Vector3[] result = new Vector3[number];
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2f / number;
        for(int k = 0; k < number; k++)
        {
            float y = k * off - 1 + (off / 2f);
            float r = Mathf.Sqrt(1 - y * y);
            float phi = k * inc;
            result[k] = new Vector3(Mathf.Cos(phi) * r, y, Mathf.Sin(phi) * r);
        }
        return result;
    }
    private GameObject GenerateOrbit(float radius, float thickness, int electrons, int resolution, float originAngle)
    {
        GameObject result = new GameObject();
        result.layer = 5;
        // Generate Orbit Path
        var mf = result.AddComponent<MeshFilter>();
        var mr = result.AddComponent<MeshRenderer>();
        mr.material = orbitMaterial;
        var mesh = mf.mesh;

        Vector3[] verts = new Vector3[(resolution + 1) * 4];
        int[] trigs = new int[resolution * 6 * 4];
        Vector2[] uv = new Vector2[(resolution + 1) * 4];

        for(int i = 0; i <= resolution; i++)
        {
            float progress = i / (float)resolution;
            float angle = Mathf.PI * 2 * progress;
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);

            verts[i * 4] = new Vector3(x, 0f, z) * (radius + thickness);
            verts[i * 4 + 1] = new Vector3(x, 0f, z) * radius + new Vector3(0, thickness, 0);
            verts[i * 4 + 2] = new Vector3(x, 0f, z) * (radius - thickness);
            verts[i * 4 + 3] = new Vector3(x, 0f, z) * radius - new Vector3(0, thickness, 0);

            uv[i * 4] = new Vector2(progress, 0f);
            uv[i * 4 + 1] = new Vector2(progress, 0.25f);
            uv[i * 4 + 2] = new Vector2(progress, 0.5f);
            uv[i * 4 + 3] = new Vector2(progress, 0.75f);

            if(i != resolution)
            {
                trigs[i * 24 + 0] = trigs[i * 24 + 20] = trigs[i * 24 + 21] = i * 4 + 0;
                trigs[i * 24 + 2] = trigs[i * 24 + 3] = trigs[i * 24 + 6] = i * 4 + 1;
                trigs[i * 24 + 8] = trigs[i * 24 + 9] = trigs[i * 24 + 12] = i * 4 + 2;
                trigs[i * 24 + 14] = trigs[i * 24 + 15] = trigs[i * 24 + 18] = i * 4 + 3;
                trigs[i * 24 + 1] = trigs[i * 24 + 4] = trigs[i * 24 + 23] = i * 4 + 4;
                trigs[i * 24 + 5] = trigs[i * 24 + 7] = trigs[i * 24 + 10] = i * 4 + 5;
                trigs[i * 24 + 11] = trigs[i * 24 + 13] = trigs[i * 24 + 16] = i * 4 + 6;
                trigs[i * 24 + 17] = trigs[i * 24 + 19] = trigs[i * 24 + 22] = i * 4 + 7;
            }
        }
        mesh.vertices = verts;
        mesh.triangles = trigs;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        // Generate Electrons
        Transform electronParent = new GameObject().transform;
        electronParent.SetParent(result.transform);
        electronParent.position = Vector3.zero;
        float angleDelta = Mathf.PI * 2 / (float)electrons;
        for (int i = 0; i < electrons; i++)
        {
            var e = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            e.gameObject.layer = 5;
            e.GetComponent<MeshRenderer>().material.color = Color.green;
            e.SetParent(electronParent);
            e.localPosition = new Vector3(Mathf.Cos(angleDelta * i + originAngle), 0f, Mathf.Sin(angleDelta * i + originAngle)) * radius;
            e.localScale = Vector3.one * thickness * 6;
        }
        electronParent.DOLocalRotate(new Vector3(0, 180, 0), 1).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        float rotateAxis = originAngle * Mathf.Rad2Deg;
        result.transform.localEulerAngles = new Vector3(0, rotateAxis, 0);
        Sequence s = DOTween.Sequence();
        s.Append(
            result.transform.DORotate(new Vector3(180, rotateAxis, 0), 10).SetDelay(2f).SetLoops(2, LoopType.Incremental).SetEase(Ease.Linear)
            ).AppendInterval(2).SetLoops(-1);
        return result;
    }
}