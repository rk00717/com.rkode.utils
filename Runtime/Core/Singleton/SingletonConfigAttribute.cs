using System;

namespace RKode.Utils {
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SingletonConfigAttribute : Attribute {
    public bool AutoCreate { get; set; } = false;
}
}