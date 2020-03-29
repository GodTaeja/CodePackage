using System;
using UnityEngine;
using UnityEngine.EventSystems;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class ILRAdaptor
{
    private void ForLink()
    {
        UnityEngine.Debug.Log(typeof(CursorMode));
        UnityEngine.Debug.Log(typeof(BoxCollider2D));
    }

    private static Action<AppDomain> _callback;

    public static void SetCallback(Action<AppDomain> callback)
    {
        _callback = callback;
    }

    public static void Init(AppDomain appDomain)
    {
        InitDelegate(appDomain);

        InitValueTypesBiding(appDomain);

        InitMethodRedirection(appDomain);

        InitAdaptor(appDomain);
    }

    static void InitValueTypesBiding(AppDomain appDomain)
    {
        //appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
        //appDomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
        //appDomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
    }


    public static void InitAdaptor(AppDomain appDomain)
    {
        //游戏用
        appDomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        appDomain.RegisterCrossBindingAdaptor(new Adapt_IComparable());
        appDomain.RegisterCrossBindingAdaptor(new Adapt_MComponent());
        appDomain.RegisterCrossBindingAdaptor(new Adapt_MSystem());
    }

    private static void InitMethodRedirection(AppDomain appDomain)
    {
        //R_UnityEngine_Debug.Register(appDomain);
        //R_JediEntity.Register(appDomain);

        //litjson
        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);

        //CLRBinding, 务必放在其它方法重定向注册的后面，因为CLRBinding本身也是方法重定向，谁先注册谁有效，
        // 所以优先保证自己手写的重定向代码有效。
        if (Application.isPlaying)
        {
            ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);
        }
    }

    static void InitDelegate(AppDomain appdomain)
    {

        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String, UnityEngine.LogType>();

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Application.LogCallback>((act) =>
        {
            return new UnityEngine.Application.LogCallback((condition, stackTrace, type) =>
            {
                ((Action<System.String, System.String, UnityEngine.LogType>)act)(condition, stackTrace, type);
            });
        });
        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.GameObject>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();



        appdomain.DelegateManager.RegisterFunctionDelegate<String>();


        appdomain.DelegateManager.RegisterMethodDelegate<string>();
        appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();

        appdomain.DelegateManager.RegisterMethodDelegate<int>();

        appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();


        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.UInt32>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.Action>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, System.Action>();
        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.RectTransform, System.Boolean>();

        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<UnityEngine.RectTransform>>((act) =>
        {
            return new System.Predicate<UnityEngine.RectTransform>((obj) =>
            {
                return ((Func<UnityEngine.RectTransform, System.Boolean>)act)(obj);
            });
        });



        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, UnityEngine.Vector2>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.Int32>();
        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2>();

        appdomain.DelegateManager.RegisterFunctionDelegate<System.Single>();

        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector3>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector3>();

        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Color>();
        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Color>();


        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
        {
            return new UnityEngine.Events.UnityAction(() =>
            {
                ((Action)act)();
            });
        });


        appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.ThreadStart>((act) =>
        {
            return new System.Threading.ThreadStart(() =>
            {
                ((Action)act)();
            });
        });

        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String>();

        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>();

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>((arg0, arg1) =>
            {
                ((Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>)act)(arg0, arg1);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<UnityEngine.Transform>>((act) =>
        {
            return new System.Predicate<UnityEngine.Transform>((obj) =>
            {
                return ((Func<UnityEngine.Transform, System.Boolean>)act)(obj);
            });
        });


        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
            {
                ((Action<System.Boolean>)act)(arg0);
            });
        });


        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>)act)(obj);
            });
        });

        appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32>();

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
            {
                ((Action<UnityEngine.Vector2>)act)(arg0);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
            {
                ((Action<System.String>)act)(arg0);
            });
        });




        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Transform>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Transform>();

        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.UInt32, System.Boolean>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Transform, System.Boolean>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.String>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.UInt64>();

        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2, ILRuntime.Runtime.Intepreter.ILTypeInstance>();

        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2, UnityEngine.Vector2>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
       
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Boolean>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int32>>((act) =>
        {
            return new System.Predicate<System.Int32>((obj) =>
            {
                return ((Func<System.Int32, System.Boolean>)act)(obj);
            });
        });

        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.SkinnedMeshRenderer, System.Boolean>();
        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.SkinnedMeshRenderer, UnityEngine.GameObject>();

        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2, System.Int32>();

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Single>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<System.Single>((arg0) =>
            {
                ((Action<System.Single>)act)(arg0);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.UInt32>>((act) =>
        {
            return new System.Predicate<System.UInt32>((obj) =>
            {
                return ((Func<System.UInt32, System.Boolean>)act)(obj);
            });
        });

        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.UI.Image>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>((arg0) =>
            {
                ((Action<UnityEngine.EventSystems.BaseEventData>)act)(arg0);
            });
        });
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.UI.Image>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.Single>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.UInt32, System.Single>, System.Single>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Func<System.Collections.Generic.KeyValuePair<System.UInt32, System.Single>, System.Int32>>((act) =>
        {
            return new System.Func<System.Collections.Generic.KeyValuePair<System.UInt32, System.Single>, System.Int32>((arg1) =>
            {
                return ((Func<System.Collections.Generic.KeyValuePair<System.UInt32, System.Single>, System.Int32>)act)(arg1);
            });
        });


        appdomain.DelegateManager.RegisterFunctionDelegate<System.Reflection.Assembly, System.Collections.Generic.IEnumerable<System.Type>>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Type, System.Boolean>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Sprite, LitJson.JsonWriter>();

        appdomain.DelegateManager.RegisterDelegateConvertor<LitJson.ExporterFunc<UnityEngine.Sprite>>((act) =>
        {
            return new LitJson.ExporterFunc<UnityEngine.Sprite>((obj, writer) =>
            {
                ((Action<UnityEngine.Sprite, LitJson.JsonWriter>)act)(obj, writer);
            });
        });

        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.UI.ILayoutElement, System.Single>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>>((act) =>
        {
            return new System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>((input) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(input);
            });
        });

        appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Transform, System.Single>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
        {
            return new System.Comparison<System.Int32>((x, y) =>
            {
                return ((Func<System.Int32, System.Int32, System.Int32>)act)(x, y);
            });
        });
        appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>, System.Boolean>();
        appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Boolean>();
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.String>>((act) =>
        {
            return new System.Predicate<System.String>((obj) =>
            {
                return ((Func<System.String, System.Boolean>)act)(obj);
            });
        });


        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>();


    }
}






