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
                if (mValue == null || !mValue.Equals(value) || !value.GetType().IsAssignableFrom(typeof(ValueType)))
                {
                    mValue = value;
                    mOnValueChanged?.Invoke(value);
                    mOnValueChangedNoParam?.Invoke();
                }
            }
        }

        [JsonIgnore]
        public Action<T> mOnValueChanged = (v) => { };

        [JsonIgnore]
        public Action mOnValueChangedNoParam = () => { };

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

        public IUnRegister RegisterOnValueChangedNoParam(Action onValueChanged, bool fireAction = false)
        {
            mOnValueChangedNoParam += onValueChanged;
            if (fireAction) mOnValueChangedNoParam?.Invoke();
            return new BindPropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChangedNoParam = onValueChanged
            };
        }

        public void UnRegisterOnValueChanged(Action<T> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }

        public void UnRegisterOnValueChangedNoParam(Action onValueChanged)
        {
            mOnValueChangedNoParam -= onValueChanged;
        }
    }

    public class BindPropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }
        public Action OnValueChangedNoParam { get; set; }

        public void UnRegister()
        {
            BindableProperty.UnRegisterOnValueChanged(OnValueChanged);
            BindableProperty.UnRegisterOnValueChangedNoParam(OnValueChangedNoParam);
        }
    }
}