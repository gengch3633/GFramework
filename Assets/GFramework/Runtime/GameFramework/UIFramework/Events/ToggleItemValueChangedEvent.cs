
namespace GameFramework
{
    public class ToggleItemValueChangedEvent
    {
        public string itemName;
        public bool value;
        public ToggleItemValueChangedEvent(string typeFullName, bool value)
        {
            this.itemName = typeFullName;
            this.value = value;
        }
    }
}

