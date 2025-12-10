using TMPro;
using UnityEngine;

public class TmpVirtualKeyboardInputSource : MonoBehaviour
{
    public OVRVirtualKeyboard VirtualKeyboard;
    public TMP_InputField[] InputFields;
    private TMP_InputField _currentInputField;

    private void Start()
    {
        foreach (var field in InputFields)
        {
            if (field == null) continue;

            var f = field;
            f.onSelect.AddListener(_ => OnInputFieldSelect(f));
            f.onValueChanged.AddListener(newText => OnInputFieldValueChange(f, newText));
        }

        if (VirtualKeyboard != null)
        {
            VirtualKeyboard.CommitText += OnCommitText;
            VirtualKeyboard.Backspace += OnBackspace;
            VirtualKeyboard.Enter += OnEnter;
            VirtualKeyboard.KeyboardHidden += OnKeyboardHidden;
        }
    }

    private void OnInputFieldSelect(TMP_InputField field)
    {
        _currentInputField = field;

        if (VirtualKeyboard == null)
            return;

        VirtualKeyboard.ChangeTextContext(field.text);
        VirtualKeyboard.gameObject.SetActive(true);
    }

    private void OnInputFieldValueChange(TMP_InputField field, string newText)
    {
        if (VirtualKeyboard == null)
            return;

        if (_currentInputField == field)
        {
            VirtualKeyboard.ChangeTextContext(newText);
        }
    }

    private void OnCommitText(string committedText)
    {
        if (_currentInputField == null)
            return;

        var f = _currentInputField;

        f.text += committedText;
        f.caretPosition = f.text.Length;
        f.selectionAnchorPosition = f.caretPosition;
        f.selectionStringAnchorPosition = f.caretPosition;
        f.selectionFocusPosition = f.caretPosition;
        f.selectionStringFocusPosition = f.caretPosition;
    }

    private void OnBackspace()
    {
        if (_currentInputField == null)
            return;

        var f = _currentInputField;

        if (string.IsNullOrEmpty(f.text))
            return;

        f.text = f.text.Substring(0, f.text.Length - 1);
        f.caretPosition = f.text.Length;
    }

    private void OnEnter()
    {
        if (_currentInputField == null)
            return;

        var f = _currentInputField;

        f.onEndEdit?.Invoke(f.text);
        f.DeactivateInputField();

        if (VirtualKeyboard != null)
            VirtualKeyboard.gameObject.SetActive(false);

        _currentInputField = null;
    }

    private void OnKeyboardHidden()
    {
        if (_currentInputField == null)
            return;

        if (_currentInputField.isFocused)
        {
            _currentInputField.DeactivateInputField();
        }

        _currentInputField = null;
    }
}