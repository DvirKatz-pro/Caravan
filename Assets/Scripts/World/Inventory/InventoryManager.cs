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

    private GameObject currentMouseOverSlot = null;

    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[i].slots.Count; j++)
            {
                slots[i].slots[j].GetComponent<SlotTriggerCollider>().setInventoryManager(this);
               
            }
        }
        addToInventory("apple");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentMouseOverSlot != null && Input.GetMouseButton(0))
        {
            mouseDragInventory();
        }
    }
    private void addToInventory(string tradeableItem)
    {
        string fullPath = spritePath + tradeableItem;
        Texture2D texture = Resources.Load(fullPath) as Texture2D;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                Sprite currentSprite = slots[i].slots[j].GetComponent<Image>().sprite;
                if (currentSprite.name.Equals("f"))
                {
                   slots[i].slots[j].GetComponent<Image>().sprite = sprite;
                    j = slots[i].slots.Count;
                    i = slots.Count;
                    
                }
            }
        }
    }

    public void setCurrentMouseOverSlot(GameObject currentMouseOverSlot)
    {
        this.currentMouseOverSlot = currentMouseOverSlot;
    }

    public void mouseDragInventory()
    {
       
        string currentFullPath = spritePath + this.currentMouseOverSlot.GetComponent<Image>().sprite.name;
        Texture2D currentTexture = Resources.Load(currentFullPath) as Texture2D;
        Sprite currentSprite = Sprite.Create(currentTexture, new Rect(0, 0, currentTexture.width, currentTexture.height), Vector2.zero);
        GameObject spriteObejct = null;
        if (currentSprite.name != "f")
        {
            string emptyFullPath = spritePath + "f";
            Texture2D emptyTexture = Resources.Load(emptyFullPath) as Texture2D;
            Sprite sprite = Sprite.Create(emptyTexture, new Rect(0, 0, emptyTexture.width, emptyTexture.height), Vector2.zero);
            this.currentMouseOverSlot.GetComponent<Image>().sprite = sprite;

            spriteObejct = new GameObject("spriteMouseDrag");
            spriteObejct.AddComponent<SpriteRenderer>();
            spriteObejct.GetComponent<SpriteRenderer>().sprite = currentSprite;
            spriteObejct = Instantiate(spriteObejct, Input.mousePosition,Quaternion.identity);


        }
        while (Input.GetMouseButton(0))
        {
            spriteObejct.transform.position = Input.mousePosition;
        }
        
    }
   
}
