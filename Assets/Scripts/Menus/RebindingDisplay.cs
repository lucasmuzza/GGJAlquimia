using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[Serializable]
public class InputKeyBind
{
    public InputActionReference inputAction;
    public string bindingDisplayName;
}

public class RebindingDisplay : MonoBehaviour
{
    public PlayerInput playerInput;
    public OptionsMenu optionsMenu;
    public List<InputActionReference> defaultKeyBinds;
    public List<InputKeyBind> inputKeyBinds = new List<InputKeyBind>();

    public InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    private string rebindedText;
    private List<InputActionReference> tempDefaultBinds;
    private const string RebindsKey = "rebinds";


    public UnityEvent<string, string> OnBindUpdate = new UnityEvent<string, string>();

    private void Start()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        optionsMenu = GetComponent<OptionsMenu>();

        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
            InitializeRebindsFromCurrentActions();
        }
        else
        {
            InitializeDefaultBinds(inputKeyBinds);
            Save();
        }
    }
    private void InitializeDefaultBinds(List<InputKeyBind> inputKeyBinds)
    {
        inputKeyBinds.Clear();

        for (int i = 0; i < defaultKeyBinds.Count; i++)
        {
            InputKeyBind newKeyBind = new InputKeyBind
            {
                inputAction = defaultKeyBinds[i],
                bindingDisplayName = InputControlPath.ToHumanReadableString(
                    defaultKeyBinds[i].action.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                )
            };

            inputKeyBinds.Add(newKeyBind);
        }
    }

    private void InitializeRebindsFromCurrentActions()
    {
        inputKeyBinds.Clear();

        foreach (var defaultBind in defaultKeyBinds)
        {
            InputKeyBind newKeyBind = new InputKeyBind
            {
                inputAction = defaultBind,
                bindingDisplayName = InputControlPath.ToHumanReadableString(
                    defaultBind.action.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                )
            };

            inputKeyBinds.Add(newKeyBind);
        }
    }

    public List<InputActionReference> GetDefaultBinds()
    {
        return defaultKeyBinds;
    }

    public void Save()
    {
        // Save the current bindings to PlayerPrefs
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsKey, rebinds);

        Debug.Log("Bindings saved.");
    }

    public string GetRebindedText()
    {
        return rebindedText;
    }

    public void StartRebinding(InputActionReference bindToRebind, Label currentBind)
    {
        string oldBindText = InputControlPath.ToHumanReadableString(
            bindToRebind.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        bindToRebind.action.Disable();

        _rebindingOperation = bindToRebind.action.PerformInteractiveRebinding()
            
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted(bindToRebind, oldBindText, currentBind)) // Pass the label
            .Start();
    }

    private void RebindCompleted(InputActionReference bindToRebind, string oldBindText, Label currentBind)
    {
        string controlPath = _rebindingOperation.selectedControl.path;
        rebindedText = InputControlPath.ToHumanReadableString(controlPath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        foreach (var keyBind in inputKeyBinds)
        {
            if (keyBind.inputAction == bindToRebind)
            {
                keyBind.bindingDisplayName = rebindedText;
                break;
            }
        }

        // Update the Label text directly
        currentBind.text = rebindedText.ToLower();

        OnBindUpdate?.Invoke(oldBindText, rebindedText);

        bindToRebind.action.Enable();
        _rebindingOperation.Dispose();
    }

    private void RebindingCancelled(InputActionReference bindToRebind)
    {
        // Re-enable the action if rebinding is canceled
        _rebindingOperation.Dispose();
    }

    public void ResetBinds()
    {
        tempDefaultBinds = new List<InputActionReference>(defaultKeyBinds);

        // Clear all current binding overrides from player input
        playerInput.actions.RemoveAllBindingOverrides();

        // Loop through all the current key binds and reset them to their defaults
        for (int i = 0; i < inputKeyBinds.Count; i++)
        {
            // Get the old binding text (currently assigned key)
            string oldBindText = InputControlPath.ToHumanReadableString(
                inputKeyBinds[i].inputAction.action.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            // Get the new binding text from the default bindings (this will be the reset value)
            string newBindText = InputControlPath.ToHumanReadableString(
                tempDefaultBinds[i].action.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            // Update the binding display name with the new (default) binding text
            inputKeyBinds[i].bindingDisplayName = newBindText;

            if (optionsMenu._bindTextDic.ContainsKey(i))
            {
                optionsMenu._bindTextDic[i].text = newBindText.ToUpper();
            }

            // Invoke the event to update UI and pass the old/new bind texts
            OnBindUpdate?.Invoke(oldBindText, newBindText);
        }

        // Save the updated bindings to PlayerPrefs
        Save();

        // Log the reset action
        Debug.Log("Bindings have been reset to default.");
    }
}
