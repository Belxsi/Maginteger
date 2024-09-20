using System;
using System.Runtime.Serialization;
using UnityEngine;

public class UnEqualTypesException : Exception
{
    public Type type;
    public object value;
    public UnEqualTypesException(Type type,object value) : base() {
        this.value = value;
        this.type = type;
            }

    public UnEqualTypesException(string message, Exception innerException,object value,Type type) : base(message, innerException)
    {
        this.value = value;
        this.type = type;
    }
   
    public override string ToString()
    {
        
        return "��� ���������� " + value + " �� �������� � ��������� " + type;
    }
    protected UnEqualTypesException(SerializationInfo info, StreamingContext context, object value, Type type) : base(info, context)
    {
        this.value = value;
        this.type = type;
    }
}
