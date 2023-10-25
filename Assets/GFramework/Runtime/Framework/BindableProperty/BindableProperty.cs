using Newtonsoft.Json;
using System;

namespace Framework
{
    public class BindableProperty<T>
    {
        private T mValue;

        public T Value
        {
            get => mValue;
            set
            {
                if (mValue == null || !mValue.Equals(value))
                {
                    mValue = value;
                    mOnValueChanged?.Invoke(value);
                }
            }
        }

        [JsonIgnore]
        public Action<T> mOnValueChanged = (v) => { };

        public IUnRegister RegisterOnValueChanged(Action<T> onValueChanged, bool fireAction = false)
        {
            mOnValueChanged += onValueChanged;
            if(fireAction) mOnValueChanged?.Invoke(mValue);
            return new BindPropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public void UnRegisterOnValueChanged(Action<T> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }
    }

    public class BindPropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        public void UnRegister()
        {
            BindableProperty.UnRegisterOnValueChanged(OnValueChanged);
        }
    }
}