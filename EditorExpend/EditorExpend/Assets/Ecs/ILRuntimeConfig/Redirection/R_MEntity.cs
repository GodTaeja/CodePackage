using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections.Generic;
using System.Reflection;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class R_MEntity
{
    public static unsafe void Register(AppDomain appDomain)
    {
        foreach (var i in typeof(MEntity).GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            if (i.Name == "Add" && i.IsGenericMethodDefinition)
            {
                appDomain.RegisterCLRMethodRedirection(i, Add);
                break;
            }
        }
    }

    //编写重定向方法对于刚接触ILRuntime的朋友可能比较困难，比较简单的方式是通过CLR绑定生成绑定代码，然后在这个基础上改
    //如何使用CLR绑定请看相关教程和文档
    static unsafe StackObject* Add(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //ILRuntime的调用约定为被调用者清理堆栈，因此执行这个函数后需要将参数从堆栈清理干净，并把返回值放在栈顶，具体请看ILRuntime实现原理文档
        AppDomain __domain = __intp.AppDomain;

        //这个是最后方法返回后esp栈指针的值，应该返回清理完参数并指向返回值，这里是只需要返回清理完参数的值即可
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);

        //取方法的参数，如果有两个参数的话，第一个参数是esp - 2,第二个参数是esp -1, 因为Mono的bug，直接-2值会错误，所以要调用ILIntepreter.Minus
        var ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        //这里是将栈指针上的值转换成object，如果是基础类型可直接通过ptr->Value和ptr->ValueLow访问到值，具体请看ILRuntime实现原理文档
        int componentType = ptr_of_this_method->Value;

        //this
        ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
        global::MEntity instance_of_this_method = (global::MEntity)typeof(global::MEntity).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        __intp.Free(ptr_of_this_method);

        //这里的type = <T>里的T
        var type = __method.GenericArguments[0].ReflectionType;
        object component;
        MComponent c3;
        if (type is ILRuntime.Reflection.ILRuntimeType)
        {
            component = ((ILRuntime.Reflection.ILRuntimeType)type).ILType.Instantiate();
            var c = (ILTypeInstance)component;
            c3 = c.CLRInstance as MComponent;
        }
        else
        {
            component = Activator.CreateInstance(type);
            c3 = component as MComponent;
        }

        instance_of_this_method.Add(componentType, c3);

        return ILIntepreter.PushObject(__ret, __mStack, component);
    }


}
