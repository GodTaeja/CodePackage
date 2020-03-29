using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class Adapt_MSystem : CrossBindingAdaptor
{
    public override Type BaseCLRType => typeof(MSystem);

    public override Type AdaptorType => typeof(Adaptor);

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MSystem, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        //缓存这个数组来避免调用时的GC Alloc
        object[] param1 = new object[1];

        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            this.instance = instance;
            this.appdomain = appdomain;
        }

        public ILTypeInstance ILInstance => instance;

        IMethod mGetJob;
        bool mGetJobGot;
        bool isGetJobInvoking = false;
        public override MSystemJob GetJob(MEntity arg0)
        {
            if (!mGetJobGot)
            {
                mGetJob = instance.Type.GetMethod("GetJob", 1);
                mGetJobGot = true;
            }
            //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
            if (mGetJob != null && !isGetJobInvoking)
            {
                isGetJobInvoking = true;
                param1[0] = arg0;
                object ret=appdomain.Invoke(mGetJob, instance, this.param1);
                isGetJobInvoking = false;
                return (MSystemJob)ret;
            }
            else
                return base.GetJob(arg0);
        }


        IMethod mUpdate;
        bool mUpdateGot;
        bool isUpdateInvoking = false;
        public override void Update(float arg0)
        {
            if (!mUpdateGot)
            {
                mUpdate = instance.Type.GetMethod("Update", 1);
                mUpdateGot = true;
            }
            //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
            if (mUpdate != null && !isUpdateInvoking)
            {
                isUpdateInvoking = true;
                param1[0] = arg0;
                appdomain.Invoke(mUpdate, instance, this.param1);
                isUpdateInvoking = false;
            }
            else
                base.Update(arg0);
        }


        IMethod mUpdateJobs;
        bool mUpdateJobsGot;
        bool isUpdateJobsInvoking = false;
        protected override void UpdateJobs(float arg0)
        {
            if (!mUpdateJobsGot)
            {
                mUpdateJobs = instance.Type.GetMethod("UpdateJobs", 1);
                mUpdateJobsGot = true;
            }
            //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
            if (mUpdateJobs != null && !isUpdateJobsInvoking)
            {
                isUpdateJobsInvoking = true;
                param1[0] = arg0;
                appdomain.Invoke(mUpdateJobs, instance, this.param1);
                isUpdateJobsInvoking = false;
            }
            else
                base.UpdateJobs(arg0);
        }


        IMethod mWaitForJobDone;
        bool mWaitForJobDoneGot;
        bool isWaitForJobDoneInvoking = false;
        protected override void WaitForJobDone()
        {
            if (!mWaitForJobDoneGot)
            {
                mWaitForJobDone = instance.Type.GetMethod("WaitForJobDone", 1);
                mWaitForJobDoneGot = true;
            }
            //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
            if (mWaitForJobDone != null && !isWaitForJobDoneInvoking)
            {
                isWaitForJobDoneInvoking = true;
                appdomain.Invoke(mWaitForJobDone, instance, null);
                isWaitForJobDoneInvoking = false;
            }
            else
                base.WaitForJobDone();
        }
    }

}
