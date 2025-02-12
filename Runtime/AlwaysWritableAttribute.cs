using System;

namespace SuperSync
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlwaysWritableAttribute : Attribute
    {
    }
}