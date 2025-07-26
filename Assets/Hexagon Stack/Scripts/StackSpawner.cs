using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Transform stackPositionsParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexStack hexagonStackPrefab;

    [Header("Settings")]
    [NaughtyAttributes.MinMaxSlider(2,8)]
    [SerializeField] private Vector2Int minMaxHexCount;
    [SerializeField] private Color[] colors;
    private int stackCounter;

    public static Action onHexaAppear;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        StackController.onStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridCell gridCell)
    {
        stackCounter++;
        if(stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateStacks();
    }

    private void GenerateStacks()
    {
        onHexaAppear?.Invoke();

        for (int i = 0; i < stackPositionsParent.childCount; i++)
        {
            GenerateStacks(stackPositionsParent.GetChild(i));
        }
    }

    private void GenerateStacks(Transform parent)
    {
        HexStack hexStack = Instantiate(hexagonStackPrefab, parent.position, Quaternion.identity, parent);
        hexStack.name = $"Stack{parent.GetSiblingIndex()}";

        int amount = UnityEngine.Random.Range(minMaxHexCount.x, minMaxHexCount.y);

        int firstColorHexagonCount = UnityEngine.Random.Range(0, amount);

        Color[] colorArray = GetRandomColors();

        for (int i = 0; i < amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * .2f;
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity, hexStack.transform);

            hexagonInstance.Color = i < firstColorHexagonCount ? colorArray[0] : colorArray[1];

            hexagonInstance.Configure(hexStack);

            hexStack.Add(hexagonInstance);
        }

    }

    private Color[] GetRandomColors()
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);

        if (colorList.Count <= 0)
        {
            return null;
        }
        Color firstColor = colorList.OrderBy(x => UnityEngine.Random.value).First();
        colorList.Remove(firstColor);

        if (colorList.Count <= 0)
        {
            return null;
        }

        Color secondColor = colorList.OrderBy(x => UnityEngine.Random.value).First();

        return new Color[] { firstColor, secondColor };
    }
}
