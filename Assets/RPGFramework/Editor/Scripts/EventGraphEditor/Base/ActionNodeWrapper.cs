using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ActionNodeWrapper<T> : ActionNode 
    where T : GraphActionBase
{
    public T Action => action as T;

    private List<VisualElement> elements;

    public ActionNodeWrapper(T action) : base(action)
    {
        elements = new List<VisualElement>();
    }

    protected void AddToExtensionContainer(VisualElement element)
    {
        extensionContainer.Add(element);
    }

    protected TextField BuildTextField(string defaultValue, Action<string> onChangedCallback, string label = "",
        bool multiline = false, string tooltip = "", bool updateUI = false, bool makeDirty = true)
    {
        TextField textField = new TextField(label)
        {
            multiline = multiline,
            tooltip = tooltip
        };

        textField.SetValueWithoutNotify(defaultValue);
        textField.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke(val.newValue);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return textField;
    }

    protected IntegerField BuildIntegerField(int defaultValue, Action<int> onChangedCallback, string label = "", 
        string tooltip = "", bool updateUI = false, bool makeDirty = true)
    {
        IntegerField intField = new IntegerField(label)
        {
            tooltip = tooltip
        };

        intField.SetValueWithoutNotify(defaultValue);
        intField.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke(val.newValue);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return intField;
    }

    protected FloatField BuildFloatField(float defaultValue, Action<float> onChangedCallback, string label = "",
        string tooltip = "", bool updateUI = false, bool makeDirty = true)
    {
        FloatField field = new FloatField(label)
        {
            tooltip = tooltip
        };

        field.SetValueWithoutNotify(defaultValue);
        field.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke(val.newValue);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return field;
    }

    protected Toggle BuildToggle(bool defaultValue, Action<bool> onChangedCallback, string label = "",
        string tooltip = "", bool updateUI = false, bool makeDirty = true)
    {
        Toggle field = new Toggle(label)
        {
            tooltip = tooltip
        };

        field.SetValueWithoutNotify(defaultValue);
        field.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke(val.newValue);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return field;
    }

    protected ObjectField BuildObjectField<K>(K defaultValue, Action<K> onChangedCallback, string label = "",
        string tooltip = "", bool allowSceneObjects = true, bool updateUI = false, bool makeDirty = true)
        where K : UnityEngine.Object
    {
        ObjectField field = new ObjectField(label)
        {
            objectType = typeof(K),
            tooltip = tooltip,
            allowSceneObjects = allowSceneObjects,
        };

        field.SetValueWithoutNotify(defaultValue);
        field.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke(val.newValue as K);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return field;
    }

    protected EnumField BuildEnumField<K>(K defaultValue, Action<K> onChangedCallback, string label = "",
        string tooltip = "", bool updateUI = false, bool makeDirty = true)
        where K : Enum
    {
        EnumField field = new EnumField(label, defaultValue)
        {
            tooltip = tooltip
        };

        field.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke((K)val.newValue);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return field;
    }

    protected PopupField<K> BuildPopupField<K>(int defaultIndex, List<K> values, Action<K> onChangedCallback, Func<K, string> formatValueCallback,
        string label = "", string tooltip = "", bool updateUI = false, bool makeDirty = true)
    {
        PopupField<K> field = new PopupField<K>(values, defaultIndex, formatValueCallback, formatValueCallback)
        {
            tooltip = tooltip,
            label = label
        };

        field.RegisterValueChangedCallback(val =>
        {
            onChangedCallback.Invoke(val.newValue);

            if (updateUI)
                UpdateUI();

            if (makeDirty)
                MakeDirty();
        });

        return field;
    }

    public override void UIContructor() { }
}
