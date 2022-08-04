using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrapBookDrawing : MonoBehaviour
{
    public Camera camera;
    public GameObject brush;
    [SerializeField] GameObject canvasObj;

    LineRenderer currentLineRenderer;
    Vector2 lastPos;

    private PlayerInput playerInput;
    private InputAction click;

    private bool firstMousePress = true;

    private void Awake()
    {
        playerInput = canvasObj.GetComponent<PlayerInput>();
        click = playerInput.actions["Click"];
        click.started += MouseDown;
        click.performed += LeftMousePressed;
        click.canceled += CancelMouse;
    }

    private void MouseDown(InputAction.CallbackContext context)
    {
        //CreateBrush();
    }

    private void LeftMousePressed(InputAction.CallbackContext context)
    {
        if (firstMousePress)
        {
            CreateBrush();
            firstMousePress = false;
        }
        
        Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if(mousePos != lastPos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    private void CancelMouse(InputAction.CallbackContext context)
    {
        currentLineRenderer = null;
        firstMousePress = true;
    }

    private void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    private void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }
}
