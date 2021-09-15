﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController  : MonoBehaviour
{
    [Header("Template")]
    public List<TerrainTemplateController> terrainTemplates ;
    public float terrainTemplateWidht;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;

    [Header("Force Early Template")]
    public List<TerrainTemplateController> earlyTerrainTemplate;

    private List<GameObject> spawnedTerrain;
    private float lastGeneratedPositionX ;
    private float lastRemovePositionX;

    private const float debugLineHeight = 10.0f;

    private Dictionary<string ,List<GameObject>>pool;

    // Start is called before the first frame update
    private void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();
        spawnedTerrain = new List<GameObject>();
        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovePositionX = lastGeneratedPositionX - terrainTemplateWidht;

        foreach (TerrainTemplateController terrain in earlyTerrainTemplate) {
            GenerateTerrain(lastGeneratedPositionX, terrain);
            lastGeneratedPositionX += terrainTemplateWidht;
        }
        while(lastGeneratedPositionX < GetHorizontalPositionEnd()){
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidht;
        }
    }



    // Update is called once per frame
    void Update()
    {
        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidht;
        }

        while (lastRemovePositionX + terrainTemplateWidht < GetHorizontalPositionStart())
        {
            lastRemovePositionX += terrainTemplateWidht;
            RemoveTerrain(lastRemovePositionX);
        }
    }

    

    private void GenerateTerrain(float posX, TerrainTemplateController forceterrain = null){

        GameObject newTerrain = Instantiate(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);
        newTerrain.transform.position = new Vector2(posX, 0f);
        spawnedTerrain.Add(newTerrain);


    }

    private float GetHorizontalPositionStart(){
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;

    }

    private float GetHorizontalPositionEnd ()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;

    }

    private void RemoveTerrain(float posX){

        GameObject terrainToRemove = null;
        foreach(GameObject item in spawnedTerrain)
        {
        if(item.transform.position.x == posX)
        {
                terrainToRemove = item;
                break;
        }
        }

        if(terrainToRemove != null){
            spawnedTerrain.Remove(terrainToRemove);
            Destroy(terrainToRemove);
        }
    }

    // pool function
private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        if (pool.ContainsKey(item.name))
        {
            // if item available in pool
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
        {
            // if item list not defined, create new one
            pool.Add(item.name, new List<GameObject>());
        }


        // create new one if no item available in pool
        GameObject newItem = Instantiate(item, parent);
        newItem.name = item.name;
        return newItem;

        pool[item.name].Add(item);
        item.SetActive(false);
    }

    private void ReturnToPool(GameObject item)
    {
        if (!pool.ContainsKey(item.name))
        {
            Debug.LogError("INVALID POOL ITEM!!");
        }
    }
        private void OnDrawGizmoz()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, areaStartPosition + Vector3.down * debugLineHeight / 2, Color.red);
        Debug.DrawLine(areaEndPosition + Vector3.up * debugLineHeight / 2, areaEndPosition + Vector3.down * debugLineHeight / 2, Color.red);
    }
}
