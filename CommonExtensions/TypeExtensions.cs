using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonExtensions.TypeExtensions
{
    public static partial class TypeExtensions
    {
        public static bool CanCastTo(this Type from, Type to)
        {
            return to.IsAssignableFrom(from)
                || HasPrimitiveImplicitCast(from, to)
                || HasPrimitiveExplicitCast(from, to)
                || IsCastDefined(to, m => m.GetParameters()[0].ParameterType, _ => from, false)
                || IsCastDefined(from, _ => to, m => m.ReturnType, true);
        }

        public static bool CanCastFrom(this Type to, Type from)
        {
            return CanCastTo(from, to);
        }

        private static bool IsCastDefined(Type type, Func<MethodInfo, Type> baseType, Func<MethodInfo, Type> derivedType, bool lookInBase)
        {
            var flags = BindingFlags.Public | BindingFlags.Static
                            | (lookInBase ? BindingFlags.FlattenHierarchy : BindingFlags.DeclaredOnly);
            return type.GetMethods(flags).Any(m => (m.Name == "op_Implicit" || m.Name == "op_Explicit")
                                                && baseType(m).IsAssignableFrom(derivedType(m)));
        }

        private static bool HasPrimitiveImplicitCast(Type from, Type to)
        {
            switch (from.Name)
            {
                case "SByte":
                    switch (to.Name)
                    {
                        case "Int16":
                        case "Int32":
                        case "Int64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Byte":
                    switch (to.Name)
                    {
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Int16":
                    switch (to.Name)
                    {
                        case "Int32":
                        case "Int64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "UInt16":
                    switch (to.Name)
                    {
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Int32":
                    switch (to.Name)
                    {
                        case "Int64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "UInt32":
                    switch (to.Name)
                    {
                        case "Int64":
                        case "UInt64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Int64":
                    switch (to.Name)
                    {
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "UInt64":
                    switch (to.Name)
                    {
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Char":
                    switch (to.Name)
                    {
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Single":
                    switch (to.Name)
                    {
                        case "Double":
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        private static bool HasPrimitiveExplicitCast(Type from, Type to)
        {
            switch (from.Name)
            {
                case "SByte":
                    switch (to.Name)
                    {
                        case "Byte":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "Byte":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "Int16":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "UInt16":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "Int32":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "UInt32":
                        case "UInt64":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "UInt32":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "Int64":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "UInt64":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "UInt64":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                case "Char":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                            return true;
                        default:
                            return false;
                    }
                case "Single":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Char":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Double":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Single":
                        case "Char":
                        case "Decimal":
                            return true;
                        default:
                            return false;
                    }
                case "Decimal":
                    switch (to.Name)
                    {
                        case "SByte":
                        case "Byte":
                        case "Int16":
                        case "UInt16":
                        case "Int32":
                        case "UInt32":
                        case "Int64":
                        case "UInt64":
                        case "Single":
                        case "Double":
                        case "Char":
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }
    }
}
