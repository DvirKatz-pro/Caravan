using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InvnetorySlots
    {
        public List<GameObject> slots;
    }
    [SerializeField] private List<InvnetorySlots> slots;

    [SerializeField] private string spritePath;

    
    // Start is called before the first frame update
    void Start()
    {
        addToInventory("apple");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void addToInventory(string tradeableItem)
    {
        string fullPath = spritePath + tradeableItem;
        Texture2D texture = Resources.Load(fullPath) as Texture2D;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[i].slots.Count; j++)
            {
                Sprite currentSprite = slots[i].slots[j].GetComponent<Image>().sprite;
                if (currentSprite.name.Equals("f"))
                {
                   slots[i].slots[j].GetComponent<Image>().sprite = sprite;
                }
            }
        }
    }
   
}
