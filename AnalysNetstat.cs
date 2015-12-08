using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor
{
    /// <summary>
    /// netstat的分析器
    /// </summary>
    public class AnalysNetstat
    {
        /// <summary>
        /// 分析netstat的内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<NetConnData> Analysis(string content)
        {
            List<NetConnData> result = new List<NetConnData>();
            string[] datas = content.Split(new string[] { "\r\n"},  StringSplitOptions.RemoveEmptyEntries);
            string keyWord = "协议";
            string[] realData= new string[2];
            for(int i =0;i< datas.Length;i++)
            {
                if (datas[i].IndexOf(keyWord) > 0)
                {
                    realData = datas.Skip(i+1).ToArray();
                    break;
                }
            }
            for (int i = 0; i < realData.Length; i++)
            {
                NetConnData data = new NetConnData();
                if (realData.Length > i+1 && !string.IsNullOrEmpty(realData[i]))
                {
                    data = GetData(realData[i]);
                    data.ProcessName = GetProcessName(realData[i + 1]);
                    i = i + 1;
                    result.Add(data);
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// 从一行数据里获取连接信息
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static NetConnData GetData(string line)
        {
            NetConnData result = new NetConnData();
            string[] data = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length > 4)
            {
                result.Protocal = data[0];
                string[] localInfo = data[1].Split(':');
                result.LocalIP = localInfo[0];
                result.LocalPort = localInfo[1];
                string[] foreignInfo = data[2].Split(':');
                result.ForeignIP = foreignInfo[0];
                result.ForeignPort = foreignInfo[1];
                result.Status = data[3];
                result.PID = data[4];
            }


            return result;
        }
        /// <summary>
        /// 根据字符串获取进程名称
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string GetProcessName(string line)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(line))
            {
                result = line.Trim().TrimEnd(']').TrimStart('[');
            }

            return result;
        }
    }
}
