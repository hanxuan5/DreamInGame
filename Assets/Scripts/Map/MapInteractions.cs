using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/**
 * ��ͼ������
 * ������Ʒ���϶���
 */

public class MapInteractions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject Background;
    public GameObject CollideMap;
    public GameObject TempImage;
    public GameObject ObjectPrefab;
    public GameObject ColliderImage;
    /**
     * 0 represent object
     * 1 represent collider
     */
    public int ObjectType = -1;


    
    private bool Dragging = false;
    private Vector3 MouseIniPos;

    private int ColliderSize = 20;
    private bool[,] collideMap;
    private List<GameObject> colliders = new List<GameObject>();

    private List<GameObject> objects = new List<GameObject>();

    private void Start()
    {
        SetMap();
    }

    void Update()
    {
        if (TempImage.GetComponent<Image>().sprite != null)
        {
            TempImage.transform.position = Input.mousePosition;
        }
        if (Dragging)
        {
            Vector3 diff = Input.mousePosition - MouseIniPos;
            Background.transform.position += diff;
            CollideMap.transform.position += diff;
            MouseIniPos = Input.mousePosition;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(1))
        {
            Dragging = false;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            MouseIniPos = Input.mousePosition;
            Dragging = true;
        } else if (ObjectType == 0 && Input.GetMouseButtonDown(0) && TempImage.GetComponent<Image>().sprite != null)
        {
            AddObject();
        } else if (ObjectType == 1 && Input.GetMouseButtonDown(0) && TempImage.GetComponent<Image>().sprite != null)
        {
             AddCollider();
        }
    }

    public void ColliderButton()
    {
        TempImage.GetComponent<Image>().sprite = ColliderImage.GetComponent<Image>().sprite;
        TempImage.GetComponent<Image>().color = ColliderImage.GetComponent<Image>().color;
        TempImage.GetComponent<Image>().rectTransform.sizeDelta = ColliderImage.GetComponent<Image>().rectTransform.sizeDelta;
    }

    public void ClearTempImage()
    {
        TempImage.GetComponent<Image>().sprite = null;
        TempImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    /**
     * Set MyMap after changing background
     */
    public void SetMap()
    {
        //Set the Collide Map
        Vector2 map_size = Background.GetComponent<Image>().rectTransform.sizeDelta;
        collideMap = new bool[Mathf.CeilToInt(map_size.y / ColliderSize), Mathf.CeilToInt(map_size.x / ColliderSize)];
        for (int i = 0; i < colliders.Count; i++)
        {
            Destroy(colliders[i]);
        }
        colliders.Clear();
    }

    public void AddCollider()
    {
        Vector3 mousePos = TempImage.transform.localPosition;
        Vector3 mapPos = GetMapPosition();
        float mapWidth = GetMapWidth(), mapHeight = GetMapHeight();
        int indexOfWidth = Mathf.FloorToInt(Math.Abs(mousePos.x - (mapPos.x - mapWidth / 2)) / ColliderSize);
        int indexOfHeight = Mathf.FloorToInt(Math.Abs(mousePos.y - (mapPos.y + mapHeight / 2)) / ColliderSize);

        float posOfWidth = -mapWidth / 2 + indexOfWidth * ColliderSize + 10;
        float posOfHeight = mapHeight / 2 - indexOfHeight * ColliderSize - 10;

        GameObject AddedObject = Instantiate(ColliderImage, CollideMap.transform);
        AddedObject.transform.localPosition = new Vector2(posOfWidth, posOfHeight);
        colliders.Add(AddedObject);
        collideMap[indexOfHeight, indexOfWidth] = true;
    }

    public void RemoveCollider(GameObject collider)
    {
        Vector3 mousePos = TempImage.transform.localPosition;
        Vector3 mapPos = GetMapPosition();
        float mapWidth = GetMapWidth(), mapHeight = GetMapHeight();
        int indexOfWidth = Mathf.FloorToInt(Math.Abs(mousePos.x - (mapPos.x - mapWidth / 2)) / ColliderSize);
        int indexOfHeight = Mathf.FloorToInt(Math.Abs(mousePos.y - (mapPos.y + mapHeight / 2)) / ColliderSize);

        colliders.Remove(collider);
        collideMap[indexOfHeight, indexOfWidth] = false;
    }

    public void AddObject()
    {
        Image curImage = TempImage.GetComponent<Image>();
        GameObject AddedObject = Instantiate(ObjectPrefab, Background.transform);
        AddedObject.transform.position = Input.mousePosition;
        AddedObject.GetComponent<Image>().sprite = curImage.sprite;
        AddedObject.GetComponent<Image>().rectTransform.sizeDelta = curImage.sprite.textureRect.size;
        objects.Add(AddedObject);
    }

    public void RemoveObject(GameObject obj)
    {
        objects.Remove(obj);
    }

    private Vector3 GetMapPosition()
    {
        return Background.transform.localPosition;
    }

    private float GetMapWidth()
    {
        return Background.GetComponent<RectTransform>().rect.width;
    }
    
    private float GetMapHeight()
    {
        return Background.GetComponent<RectTransform>().rect.height;
    }
}