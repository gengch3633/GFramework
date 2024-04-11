using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    public class BindableTypeReflection<T>
    {
        public T model;
        public List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
        public List<FieldInfo> fieldInfos = new List<FieldInfo>();

        public BindableTypeReflection(T model)
        {
            this.model = model;
            propertyInfos.AddRange(model.GetType().GetProperties());
            fieldInfos.AddRange(model.GetType().GetFields());
        }

        public object GetPropertyValue(string propertyName)
        {
            var propertyInfo = propertyInfos.Find(item => item.Name == propertyName);
            return propertyInfo.GetValue(model);
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            var propertyInfo = propertyInfos.Find(item => item.Name == propertyName);
            propertyInfo.SetValue(model, value);
        }

        public object GetFieldValue(string memberName)
        {
            var propertyInfo = fieldInfos.Find(item => item.Name == memberName);
            return propertyInfo.GetValue(model);
        }

        public void SetFieldValue(string memberName, object value)
        {
            var propertyInfo = fieldInfos.Find(item => item.Name == memberName);
            propertyInfo.SetValue(model, value);
        }

        public void CopyBindableProperties(BindableTypeReflection<T> otherReflection, Action callback = null)
        {
            propertyInfos.ForEach(item =>
            {
                var typeName = item.PropertyType.Name;

                if (typeName.Contains("BindableProperty"))
                {
                    var selfPropertyValue = GetPropertyValue(item.Name);
                    var otherPropertyValue = otherReflection.GetPropertyValue(item.Name);
                    var selfBindableReflection = new BindableTypeReflection<object>(selfPropertyValue);
                    var otherBindableReflection = new BindableTypeReflection<object>(otherPropertyValue);
                    var otherValue = otherBindableReflection.GetPropertyValue("Value");
                    selfBindableReflection.SetPropertyValue("Value", otherValue);


                    if(callback != null)
                    {
                        var itemValue = item.GetValue(model);
                        MethodInfo onValueChanged = item.PropertyType.GetMethod("RegisterOnValueChangedNoParam", new Type[] { typeof(Action), typeof(bool) });
                        onValueChanged.Invoke(itemValue, new object[] { callback, false });
                    }
                }
            });
        }

        public void CopyFields(BindableTypeReflection<T> otherReflection)
        {
            fieldInfos.ForEach(item =>
            {
                var otherFieldValue = otherReflection.GetFieldValue(item.Name);
                SetFieldValue(item.Name, otherFieldValue);
            });
        }
    }

}