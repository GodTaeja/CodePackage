using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;


public class Adapt_MComponent: CrossBindingAdaptor
{
    public override Type BaseCLRType => typeof(MComponent);

    public override Type AdaptorType => typeof(Adaptor);

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MComponent, CrossBindingAdaptorType
    {
        private AppDomain _appdomain;
        private bool _gotMethod;
        private IMethod _methodOnDestroy;
        private ILTypeInstance _instance;
        private bool _isOnDestroyInvoking;

        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            _instance = instance;
            _appdomain = appdomain;
        }

        public Adaptor()
        {

        }

        public ILTypeInstance ILInstance => _instance;

        public override void OnDestroy()
        {
            if (!_gotMethod)
            {
                _methodOnDestroy = _instance.Type.GetMethod("OnDestroy", 1);
                _gotMethod = true;
            }

            //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
            if (_methodOnDestroy != null && !_isOnDestroyInvoking)
            {
                _isOnDestroyInvoking = true;
                _appdomain.Invoke(_methodOnDestroy, _instance, null);
                _isOnDestroyInvoking = false;
            }
            else
                base.OnDestroy();
        }
    }
}

