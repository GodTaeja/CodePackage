using System;
using UnityEngine;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class Adapt_IComparable : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(IComparable); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : IComparable, CrossBindingAdaptorType
    {
        AppDomain AppDomain { get; set; }
        private ILTypeInstance _instance;
        private bool _compareToGot;
        private IMethod _compareTo;

        public ILTypeInstance ILInstance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            this.AppDomain = appdomain;
            this._instance = instance;
            Debug.Log("adapt ctor: " + instance.Type.TypeForCLR.FullName);
        }

        public Adaptor()
        {

        }

        public int CompareTo(object obj)
        {
            if (!_compareToGot)
            {
                _compareTo = _instance.Type.GetMethod("CompareTo", 1);
                _compareToGot = true;
            }

            if (_compareTo != null)
            {
                //没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                return (int)AppDomain.Invoke(_compareTo, _instance, obj);
            }
            return 0;
        }
    }
}
