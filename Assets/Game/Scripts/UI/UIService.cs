using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UIService : MonoBehaviour {
    private readonly Dictionary<string, UIElement> _elements = new Dictionary<string, UIElement>();
    private readonly Dictionary<string, UIPanel> _panels = new Dictionary<string, UIPanel>();
    private readonly List<UIPanel> _stack = new List<UIPanel>();
    public delegate void OnPanelPopDelegate(UIPanel panel);
    public event OnPanelPopDelegate OnPanelPop;

    [Inject]
    public void Construct() {
        var panels = FindObjectsOfType<UIPanel>(true);
        var elements = FindObjectsOfType<UIElement>(true);
        foreach (var panel in panels) {
            _panels.Add(panel.GetType().Name, panel);
        }
        foreach (var element in elements) {
            _elements.Add(element.GetType().Name, element);
        }
    }

    public void Init() {
        foreach (var element in _elements) {
            element.Value.Init();
        }
    }

    public T GetPanel<T>() where T : UIPanel {
        return (T)_panels[typeof(T).Name];
    }

    public T GetUIElement<T>() where T : UIElement {
        return (T)_elements[typeof(T).Name];
    }

    public void OpenPanelBypassStack<T>() where T : UIPanel {
        var panel = _panels[typeof(T).Name];
        panel.transform.SetAsLastSibling();
        panel.Show();
        _stack.RemoveAll(x => x == panel);
    }

    public void HidePanelBypassStack<T>() where T : UIPanel {
        var panel = _panels[typeof(T).Name];
        panel.Hide();
        _stack.RemoveAll(x => x == panel);
    }

    public void OpenPanel<T>() where T : UIPanel {
        ClearStack();
        PushPanel<T>();
    }

    public void PushPanel<T>() where T : UIPanel {
        if (_stack.Count > 0) {
            if (_stack[_stack.Count - 1].GetType() == typeof(T)) {
                return;
            }
        }

        foreach (var panelItem in _stack) {
            panelItem.Hide();
        }

        var panel = _panels[typeof(T).Name];
        _stack.Add(panel);
        panel.transform.SetAsLastSibling();
        panel.Show();
    }

    public void PopPanel() {
        if (_stack.Count > 0) {
            var panel = _stack[^1];
            _stack.Remove(panel);
            panel.Hide();

            OnPanelPop?.Invoke(panel);
        }
    }

    public bool IsPanelOpen<T>() where T : UIPanel {
        return _stack.Count(x => x.GetType().Name == typeof(T).Name) > 0;
    }

    public bool IsAnyPanelOpened() {
        return _stack.Count > 0;
    }

    public void PopAllPanels() {
        ClearStack();
    }

    public void ShowUIElements() {
        foreach (var element in _elements) {
            if (element.Value.IHidenElement) {
                element.Value.Show();
            }
        }
    }

    public void HideUIElements() {
        foreach (var element in _elements) {
            if (element.Value.IHidenElement) {
                element.Value.Hide();
            }
        }
    }

    private void ClearStack() {
        while (_stack.Count > 0) {
            var panel = _stack[^1];
            _stack.Remove(panel);
            panel.Hide();
        }
    }
}
