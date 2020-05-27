using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Logger = IGG.Logging.Logger;

namespace IGG.Core.Helper
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.01.28
    /// Desc    文件操作相关工具
    /// </summary>
    public static class FileHelper
    {
        // 拷贝文件夹
        public delegate bool CopyFilter(string file);
        
        public static Hash128 DefaultHash = new Hash128(0, 0, 0, 0);
        public static string DefaultHashString = DefaultHash.ToString();
        
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }
        
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        public static void CopyFile(string pathFrom, string pathTo)
        {
            if (!File.Exists(pathFrom)) {
                return;
            }
            
            DeleteFile(pathTo);
            
            CreateDirectoryFromFile(pathTo);
            File.Copy(pathFrom, pathTo);
        }
        /// <summary>
        /// 确认文件是否存在
        /// </summary>
        public static bool CheckFileExist(string fileName)
        {
            return File.Exists(fileName);
        }
        
        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void CreateFileDirectory(string FilePath)
        {
            if (Directory.Exists(FilePath) == false) {
                Directory.CreateDirectory(FilePath);
            }
        }
        
        /// <summary>
        /// 清空文件夹
        /// </summary>
        public static void ClearFileDirectory(string FileDirectory)
        {
        
            CreateFileDirectory(FileDirectory);
            List<string> lfile = new List<string>();
            
            DirectoryInfo rootDirInfo = new DirectoryInfo(FileDirectory);
            foreach (FileInfo file in rootDirInfo.GetFiles()) {
                lfile.Add(file.FullName);
            }
            
            foreach (string fileName in lfile) {
                File.Delete(fileName);
            }
            
            
            DirectoryInfo rootDic = new DirectoryInfo(FileDirectory);
            foreach (DirectoryInfo fileDic in rootDic.GetDirectories()) {
                DeleteFileDirectory(fileDic.FullName);
            }
            
        }
        
        // <summary>
        // 删除文件夹
        // </summary>
        public static void DeleteFileDirectory(string path)
        {
            if (Directory.Exists(path)) {
                DirectoryInfo rootDirInfo = new DirectoryInfo(path);
                rootDirInfo.Delete(true);
            }
        }
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="suffix"></param>
        /// <param name="onFilter"></param>
        public static void CopyDirectory(string sourcePath, string destinationPath, string suffix = "", CopyFilter onFilter = null)
        {
            if (onFilter != null && onFilter(sourcePath)) {
                return;
            }
            
            if (!Directory.Exists(destinationPath)) {
                Directory.CreateDirectory(destinationPath);
            }
            
            foreach (string file in Directory.GetFileSystemEntries(sourcePath)) {
                if (File.Exists(file)) {
                    FileInfo info = new FileInfo(file);
                    if (string.IsNullOrEmpty(suffix) || file.ToLower().EndsWith(suffix.ToLower())) {
                        string destName = Path.Combine(destinationPath, info.Name);
                        if (!(onFilter != null && onFilter(file))) {
                            File.Copy(file, destName);
                        }
                    }
                }
                
                if (Directory.Exists(file)) {
                    DirectoryInfo info = new DirectoryInfo(file);
                    string destName = Path.Combine(destinationPath, info.Name);
                    CopyDirectory(file, destName, suffix, onFilter);
                }
            }
        }
        
        /// <summary>
        /// 根据文件名，创建文件所在的目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectoryFromFile(string path)
        {
            path = path.Replace("\\", "/");
            int index = path.LastIndexOf("/");
            
            string dir = path.Substring(0, index);
            
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
        }
        
        /// <summary>
        /// 获取文件夹下所有的文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suffix"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> GetAllChildFiles(string path, string suffix = null, List<string> files = null)
        {
            if (files == null) {
                files = new List<string>();
            }
            
            if (!Directory.Exists(path)) {
                return files;
            }
            
            AddFiles(path, suffix, files);
            
            string[] temps = Directory.GetDirectories(path);
            for (int i = 0; i < temps.Length; ++i) {
                string dir = temps[i];
                GetAllChildFiles(dir, suffix, files);
            }
            
            return files;
        }
        
        private static void AddFiles(string path, string suffix, List<string> files)
        {
            string[] temps = Directory.GetFiles(path);
            for (int i = 0; i < temps.Length; ++i) {
                string file = temps[i];
                if (string.IsNullOrEmpty(suffix) || file.ToLower().EndsWith(suffix.ToLower())) {
                    files.Add(file);
                }
            }
        }
        
        public static bool IsFileInPackage(string fullpath)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (fullpath.Contains(Application.streamingAssetsPath)) {
                fullpath = fullpath.Replace(Application.streamingAssetsPath + "/", "");
                return AndroidHelper.FileHelper.CallStatic<bool> ("IsAssetExist", fullpath);;
            }
#endif
            
            return File.Exists(fullpath);
        }
        
        // 拷贝文件(Android)
        public static bool CopyAndroidAssetFile(string pathSrc, string pathDst)
        {
            bool ret = true;
#if UNITY_ANDROID && !UNITY_EDITOR
            pathSrc = pathSrc.Replace(Application.streamingAssetsPath + "/", "");
            ret = AndroidHelper.FileHelper.CallStatic<bool>("CopyFileTo", pathSrc, pathDst);
#endif
            
            return ret;
        }
        
        /// <summary>
        /// 保存直接流到文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveBytesToFile(byte[] bytes, string path)
        {
            CreateDirectoryFromFile(path);
            
            try {
                Stream stream = File.Open(path, FileMode.Create);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                return true;
            } catch (Exception e) {
                Logger.LogError(e.Message);
                return false;
            }
        }
        
        /// <summary>
        /// 保存文本到文件
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveTextToFile(string text, string path)
        {
            CreateDirectoryFromFile(path);
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return SaveBytesToFile(bytes, path);
        }
        
        /// <summary>
        /// 以字节流读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadByteFromFile(string path)
        {
            byte[] bytes = null;
            
            bool useFileReader = true;
#if UNITY_ANDROID && !UNITY_EDITOR
            // 如果是读apk包里的资源,使用Android帮助库加载(目前还没有用到,如果需要,要实现一下java代码,暂时保留)
            if (path.Contains(Application.streamingAssetsPath)) {
                useFileReader = false;
                
                path = path.Replace(Application.streamingAssetsPath + "/", "");
                bytes = AndroidHelper.FileHelper.CallStatic<byte[]> ("LoadFile", path);
            }
#endif
            if (useFileReader && File.Exists(path)) {
                bytes = File.ReadAllBytes(path);
            }
            
            return bytes;
        }
        
        /// <summary>
        /// 以文本方式读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadTextFromFile(string path)
        {
            string text = "";
            
            byte[] bytes = ReadByteFromFile(path);
            if (bytes != null) {
                text = Encoding.UTF8.GetString(bytes);
            }
            
            return text;
        }
        /// <summary>
        /// 获取md5
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string CalcMd5StringFromHash(byte[] bytes)
        {
            string ret = "";
            foreach (byte b in bytes) {
                ret += Convert.ToString(b, 16);
            }
            
            return ret;
        }
        /// <summary>
        /// 计算文件的md5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CalcFileMd5(string path)
        {
            if (!File.Exists(path)) {
                return "";
            }
            
            FileStream stream = File.OpenRead(path);
            
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(stream);
            stream.Close();
            
            return CalcMd5StringFromHash(result);
        }
        
        public static string GetCacheAssetBundlePath(string filename)
        {
            return string.Format("{0}/{1}/{2}/__data", ConstantData.UnpackPath, filename, DefaultHashString);
        }
        
        /// <summary>
        /// 获取ab的全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAssetBundleFullPath(string path)
        {
#if UNITY_IOS
            return string.Format("file:///{0}", path);
#else
            return path;
#endif
        }
    }
}