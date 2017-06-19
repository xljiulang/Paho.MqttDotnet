using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Paho.MqttDotnet
{
    /// <summary>
    /// 表示动态库加载器
    /// </summary>
    static class LibraryLoader
    {
        /// <summary>
        /// 加载dll
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string fileName);

        /// <summary>
        /// 获取dll的完整路径
        /// </summary>
        /// <param name="dllName">dll名称</param>
        /// <returns></returns>
        private static string GetDllFullPath(string dllName)
        {
            var dllFile = Path.Combine(Environment.Is64BitProcess ? @"libs\x64" : @"libs\x86", dllName);
            if (HttpContext.Current != null)
            {
                dllFile = Path.Combine("~\\bin", dllFile);
                dllFile = HttpContext.Current.Server.MapPath(dllFile);
            }
            return dllFile;
        }


        /// <summary>
        /// 加载dll
        /// </summary>
        /// <param name="dllName">dll名</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public static IntPtr[] Load(params string[] dllName)
        {
            return dllName.Select(dll =>
            {
                var dllPath = GetDllFullPath(dll);
                if (File.Exists(dllPath) == false)
                {
                    throw new FileNotFoundException(dllPath);
                }
                return LoadLibraryA(dllPath);
            }).ToArray();
        }
    }
}
