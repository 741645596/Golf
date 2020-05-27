using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IGG.Core.Module
{
    public class ModuleMgr
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        public static void InitAllModule()
        {
            Type[] moduleTypes = GetAllModuleTypes();
            foreach (Type moduleType in moduleTypes)
            {
                IModule decoder = Activator.CreateInstance(moduleType) as IModule;
                if (decoder == null)
                {
                    continue;
                }
                decoder.OnInit();
            }
        }

        private static Type[] GetAllModuleTypes()
        {
            Type cfgInterfaceType = typeof(IModule);
            List<Type> decoderTypes = new List<Type>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblys)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.GetInterfaces().Contains(cfgInterfaceType))
                    {
                        if (!type.IsAbstract)
                        {
                            decoderTypes.Add(type);
                        }
                    }
                }
            }
            return decoderTypes.ToArray();
        }
        /// <summary>
        /// 清理模块DC的数据
        /// </summary>
        public static void ClearAllModuleDC()
        {
            Type[] moduleTypes = GetAllModuleTypes();
            foreach (Type moduleType in moduleTypes)
            {
                IModule decoder = Activator.CreateInstance(moduleType) as IModule;
                if (decoder == null)
                {
                    continue;
                }
                decoder.ClearDC();
            }
        }
    }
}
