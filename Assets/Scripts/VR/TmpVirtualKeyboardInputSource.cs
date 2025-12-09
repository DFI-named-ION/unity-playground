using TMPro;
using UnityEngine;

public class TmpVirtualKeyboardInputSource : MonoBehaviour
{
    private OVRVirtualKeyboard _virtualKeyboard;
    private TMP_InputField _inputField;

    private void Start()
    {
        _inputField.onSelect.AddListener(OnInputFieldSelect);
        _inputField.onValueChanged.AddListener(OnInputFieldValueChange);

        _virtualKeyboard.CommitText += OnCommitText;
        _virtualKeyboard.Backspace += OnBackspace;
        _virtualKeyboard.Enter += OnEnter;
        _virtualKeyboard.KeyboardHidden += OnKeyboardHidden;
    }

    private void OnDestroy()
    {
        if (_virtualKeyboard != null)
        {
            _virtualKeyboard.CommitText -= OnCommitText;
            _virtualKeyboard.Backspace -= OnBackspace;
            _virtualKeyboard.Enter -= OnEnter;
            _virtualKeyboard.KeyboardHidden -= OnKeyboardHidden;
        }
    }

    private void OnInputFieldSelect(string currentText)
    {
        _virtualKeyboard.ChangeTextContext(currentText);
        _virtualKeyboard.gameObject.SetActive(true);
    }

    private void OnInputFieldValueChange(string newText)
    {
        _virtualKeyboard.ChangeTextContext(newText);
    }

    private void OnCommitText(string committedText)
    {
        _inputField.text += committedText;
        _inputField.caretPosition = _inputField.text.Length;
        _inputField.selectionAnchorPosition = _inputField.caretPosition;
        _inputField.selectionStringAnchorPosition = _inputField.caretPosition;
        _inputField.selectionFocusPosition = _inputField.caretPosition;
        _inputField.selectionStringFocusPosition = _inputField.caretPosition;
    }

    private void OnBackspace()
    {
        if (string.IsNullOrEmpty(_inputField.text))
            return;

        _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
        _inputField.caretPosition = _inputField.text.Length;
    }

    private void OnEnter()
    {
        _inputField.onEndEdit?.Invoke(_inputField.text);
        _inputField.DeactivateInputField();

        _virtualKeyboard.gameObject.SetActive(false);
    }

    private void OnKeyboardHidden()
    {
        if (_inputField.isFocused)
        {
            _inputField.DeactivateInputField();
        }
    }
}
