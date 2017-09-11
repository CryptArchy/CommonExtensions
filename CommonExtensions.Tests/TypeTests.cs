using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonExtensions.TypeExtensions;

namespace CommonExtensions.Tests
{
    [TestClass]
    public class TypeTests
    {
        [TestMethod]
        public void Primitive_Types_Correctly_Queried_For_Casts()
        {
            Type[] fromTypes = {typeof(bool), typeof(byte), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(sbyte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(Single), typeof(double), typeof(char), typeof(IntPtr), typeof(UIntPtr)};
            Type[] toTypes = new Type[fromTypes.Length];
            fromTypes.CopyTo(toTypes, 0);
            //                  bool , byte , ui16 , ui32 , ui64 , sbyte, int16, int32, int64, sngle, dble , char , intpt, uinpt
            bool[] expecteds = { 
                    /*bool */   true , false, false, false, false, false, false, false, false, false, false, false, false, false,
                    /*byte */   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*ui16 */   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*ui32 */   false, true , true , true , true , true , true , true , true , true , true , true , false, true ,
                    /*ui64 */   false, true , true , true , true , true , true , true , true , true , true , true , false, true ,
                    /*sbyte*/   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*int16*/   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*int32*/   false, true , true , true , true , true , true , true , true , true , true , true , true , false,
                    /*int64*/   false, true , true , true , true , true , true , true , true , true , true , true , true , false,
                    /*sngle*/   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*dble */   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*char */   false, true , true , true , true , true , true , true , true , true , true , true , false, false,
                    /*intpt*/   false, false, false, false, false, false, false, true , true , false, false, false, true , true ,
                    /*uinpt*/   false, false, false, true , true , false, false, false, false, false, false, false, false, true 
                              };

            var index = 0;
            foreach (var from in fromTypes)
            {
                foreach (var to in toTypes)
                {
                    var actual = from.CanCastTo(to);
                    Assert.AreEqual(expecteds[index++], actual, 
                        "Failure on {0} -> {1}.", from.Name, to.Name);
                }
            }
        }

        private class SuperClass { }
        private class ChildClass : SuperClass { }
        private class ExplicitClass { public static explicit operator ChildClass(ExplicitClass me) { return new ChildClass(); } }
        private class ImplicitClass { public static implicit operator ChildClass(ImplicitClass me) { return new ChildClass(); } }
        private class ReverseClass {
            public static implicit operator ChildClass(ReverseClass me) { return new ChildClass(); }
            public static implicit operator ReverseClass(ChildClass me) { return new ReverseClass(); }
        }
        private class StrangerClass { }

        [TestMethod]
        public void User_Types_Correctly_Queried_For_Casts()
        {
            Type[] fromTypes = { typeof(SuperClass), typeof(ChildClass), typeof(ExplicitClass), typeof(ImplicitClass), typeof(ReverseClass), typeof(StrangerClass)};
            Type[] toTypes = new Type[fromTypes.Length];
            fromTypes.CopyTo(toTypes, 0);
            //                  super, child, expli, impli, revrs, strng
            bool[] expecteds = { 
                    /*super*/   true , false, false, false, false, false,
                    /*child*/   true , true , false, false, true , false,  
                    /*expli*/   true , true , true , false, false, false,  
                    /*impli*/   true , true , false, true , false, false,  
                    /*revrs*/   true , true , false, false, true , false, 
                    /*strng*/   false, false, false, false, false, true  
                              };

            var index = 0;
            foreach (var from in fromTypes)
            {
                foreach (var to in toTypes)
                {
                    var actual = from.CanCastTo(to);
                    Assert.AreEqual(expecteds[index++], actual,
                        "Failure on {0} -> {1}.", from.Name, to.Name);
                }
            }
        }
    }
}
