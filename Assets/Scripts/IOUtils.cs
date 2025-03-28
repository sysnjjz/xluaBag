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
            //���˹����ļ�
            if(f.Name.EndsWith(".meta"))
            {
                continue;
            }
            //ɾ����׺��
            string sPath = f.FullName.Substring(f.FullName.IndexOf("LuaScripts") + 11).Replace(".lua", "").Replace(@"\", ".");
            if(sPath=="GlobalFunc"||sPath=="Main")
            {
                continue;
            }
            FileList.Add(sPath);//����ļ�·�����б���
        }
        //��ȡ���ļ����ڵ��ļ��б��ݹ����
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
