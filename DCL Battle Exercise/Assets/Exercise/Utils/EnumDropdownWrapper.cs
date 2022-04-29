using System;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;

public class EnumDropdownWrapper<T> : IDisposable where T : Enum  
{
    public Action<T> OnValueChanged;

    private readonly TMP_Dropdown dropdown;

    private readonly ReadOnlyDictionary<string, int> nameToIndex;

    public EnumDropdownWrapper(TMP_Dropdown dropdown)
    {
        this.dropdown = dropdown;
        nameToIndex = new ReadOnlyDictionary<string, int>(Enum.GetNames(typeof(T))
            .Select((name,index) => (name,index))
            .ToDictionary(x => x.name, x => x.index));

        dropdown.ClearOptions();
        dropdown.AddOptions(nameToIndex.Keys.ToList());
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public T Value()
    {
        return (T)Enum.GetValues(typeof(T)).GetValue(dropdown.value);
    }

    public void SetValueWithoutNotify(T value)
    {
        dropdown.SetValueWithoutNotify(EnumToIndex(value));
    }

    private void OnDropdownValueChanged(int index)
    {
        OnValueChanged?.Invoke((T)Enum.GetValues(typeof(T)).GetValue(index));
    }

    private int EnumToIndex(T value)
    {
        return nameToIndex[value.ToString()];
    }

    public void Dispose()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}
