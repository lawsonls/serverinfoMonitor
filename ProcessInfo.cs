using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor
{
    /// <summary>
    /// 进程信息实体
    /// </summary>
    public class ProcessShowInfo
    {
        private string name = "";
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }
        private string pID = "";
        /// <summary>
        /// 进程PID
        /// </summary>
        public string PID
        {
            get
            {
                return pID;
            }
            set
            {
                this.pID = value;
            }
        }
        private string location = "";
        /// <summary>
        /// 进程地址
        /// </summary>
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                this.location = value;
            }
        }
        private string threadNum = "";
        /// <summary>
        /// 线程数量
        /// </summary>
        public string ThreadNum
        {
            get
            {
                return threadNum;
            }
            set
            {
                this.threadNum = value;
            }
        }
    }

    /// <summary>
    /// 线程线程比较器
    /// </summary>
    public class ProcessShowCompare : IComparer<ProcessShowInfo>
    {
        //当前排序列
        public int m_Column = 0;
        //0字符排序，1数字排序
        public int m_SortType = 0;
        //正序排，倒序排
        public bool m_asc = true;
        /// <summary>
        /// 设置排序方式
        /// </summary>
        /// <param name="column">当前排序列</param>
        /// <param name="sortType">0字符排序，1数字排序</param>
        /// <param name="asc">正序排，倒序排</param>
        public ProcessShowCompare(int column, int sortType, bool asc)
        {
            this.m_Column = column;
            this.m_SortType = sortType;
            this.m_asc = asc;
        }

        #region 排序

        public int Compare(ProcessShowInfo x, ProcessShowInfo y)
        {
            int intSort = 0;
            if (!m_asc)//反序
            {
                //互换数据
                ProcessShowInfo temp = x;
                x = y;
                y = temp;
            }
            if (m_SortType == 0)   //字符排序
            {
                intSort = String.Compare(GetProcessShowInfoValue(x,m_Column), GetProcessShowInfoValue(y,m_Column));
                return intSort;
            }
            else      //数值排序
            {
                int str1 = 0;
                int str2 = 0;
                if (string.IsNullOrEmpty(x.PID) || int.Parse(x.PID) < 0)//为空设置为最小
                    return 1;
                else if (string.IsNullOrEmpty(y.PID) || int.Parse(y.PID) < 0)
                    return 0;
                try
                {
                    str1 = int.Parse(GetProcessShowInfoValue(x, m_Column));
                    str2 = int.Parse(GetProcessShowInfoValue(y, m_Column));
                }
                catch
                {
                    //转换出错
                    return 0;
                }
                if (str1 > str2)
                {
                    return 1;
                }
                else if (str1 == str2)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }
        
        #endregion
        
        private string GetProcessShowInfoValue(ProcessShowInfo info, int i)
        {
            if (i == 1)
            {
                return info.PID;
            }
            else if (i == 2)
            {
                return info.Name;
            }
            else if (i == 3)
            {
                return info.ThreadNum;
            }
            else if (i == 4)
            {
                return info.Location;
            }
            return info.PID;
        }
    }


}
