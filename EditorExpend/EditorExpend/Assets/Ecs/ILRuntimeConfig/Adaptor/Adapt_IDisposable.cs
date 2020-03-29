using System;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class Adapt_IDisposable : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(IDisposable); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MyAdaptor, IDisposable
    {
        protected override AdaptHelper.AdaptMethod[] GetAdaptMethods()
        {
            return new AdaptHelper.AdaptMethod[]
            {
                new AdaptHelper.AdaptMethod{Name = "Dispose", ParamCount = 0}
            };
        }

        public Adaptor(AppDomain appdomain, ILTypeInstance instance):base(appdomain,instance)
        {
        }

        public void Dispose()
        {
            Invoke(0, null);
        }
    }
}
