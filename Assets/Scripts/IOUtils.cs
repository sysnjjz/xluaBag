using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using XLua;

[LuaCallCSharp]
public class IOUtils
{
    private static List<string> GetFile(string path, List<string> FileList)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] fil = dir.GetFiles();
        DirectoryInfo[] dii = dir.GetDirectories();
        foreach (FileInfo f in fil)
        {
            //int size = Convert.ToInt32(f.Length);
            long size = f.Length;
            //过滤关联文件
            if(f.Name.EndsWith(".meta"))
            {
                continue;
            }
            //删除后缀名
            string sPath = f.FullName.Substring(f.FullName.IndexOf("LuaScripts") + 11).Replace(".lua", "").Replace(@"\", ".");
            if(sPath=="GlobalFunc"||sPath=="Main")
            {
                continue;
            }
            FileList.Add(sPath);//添加文件路径到列表中
        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo d in dii)
        {
            GetFile(d.FullName, FileList);
        }
        return FileList;
    }

    public static List<string> luaCallRequire()
    {
        List<string> fileList = new List<string>();
        GetFile(Application.dataPath + "/LuaScripts", fileList);
        return fileList;
    }
}
