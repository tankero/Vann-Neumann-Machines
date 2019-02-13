using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Vector2 pos = new Vector2(20,40);
    private Vector2 size = new Vector2(100,60);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;
    public float barDisplay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Lerp(0f, 1f, barDisplay);
        barDisplay += 0.1f * Time.deltaTime;
    }
    private void OnGUI()
    {
        
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
            GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarEmpty);
            // draw the filled-in part:
              GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
                  GUI.Box(new Rect(0, 0, size.x, size.y), progressBarFull);
              GUI.EndGroup();
        GUI.EndGroup();
    }
    


}
