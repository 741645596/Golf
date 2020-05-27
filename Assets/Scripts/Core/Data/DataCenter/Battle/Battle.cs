using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author  zhulin
/// Date    2019.6.5
/// Desc    战斗管理器
/// </summary>

namespace IGG.Core.Data.DataCenter.Battle
{
    public class Battle
    {
        /// <summary>
        /// 自己的操作数据
        /// </summary>
        protected List<BattleOperateInfo> m_SelfOpertioan = new List<BattleOperateInfo>();
        /// <summary>
        /// 己方操作次数
        /// </summary>
        protected int m_SelfOperateTimes;
        /// <summary>
        /// 对方的操作数据
        /// </summary>
        protected List<BattleOperateInfo> m_PlayerOpertioan = new List<BattleOperateInfo>();
        /// <summary>
        /// 对方操作次数
        /// </summary>
        protected int m_PlayerOperateTimes;
        /// <summary>
        /// 是否为先手
        /// </summary>
        protected bool m_IsFirsthand = true;
        /// <summary>
        /// 设置先手
        /// </summary>
        /// <param name="IsSelfFirstHand"></param>
        public void SetFirstHand(bool IsSelfFirstHand)
        {
            m_IsFirsthand = IsSelfFirstHand;
        }
        /// <summary>
        /// 添加战斗操作数据
        /// </summary>
        /// <param name="IsSelf"></param>
        /// <param name="OperateTimes"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool AddBattleOperate(bool IsSelf,int OperateTimes, BattleOperateInfo v)
        {
            List<BattleOperateInfo> l = GetOperateList(IsSelf);
            if (l == null || v == null)
                return false;
            if (l.Count != OperateTimes - 1)
                return false;
            else
            {
                l.Add(new BattleOperateInfo(v));
                if (IsSelf == true)
                {
                    m_SelfOperateTimes = OperateTimes;
                }
                else
                {
                    m_PlayerOperateTimes = OperateTimes;
                }
                return true;
            }
        }

        /// <summary>
        /// 获取操作列表
        /// </summary>
        /// <param name="IsSelf"></param>
        /// <returns></returns>
        protected List<BattleOperateInfo> GetOperateList(bool IsSelf)
        {
            if (IsSelf == true)
            {
                return m_SelfOpertioan;
            }
            else
            {
                return m_PlayerOpertioan;
            }
        }

        /// <summary>
        /// 判断是否赢了
        /// </summary>
        public virtual bool CheckWin()
        {
            return true;
        }

        public virtual void Clear()
        {
            m_SelfOperateTimes = 0;
            m_PlayerOperateTimes = 0;
            if (m_SelfOpertioan != null)
            {
                m_SelfOpertioan.Clear();
            }
            if (m_PlayerOpertioan != null)
            {
                m_PlayerOpertioan.Clear();
            }
        }

        /// <summary>
        /// 战斗帧运行
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Update(float deltaTime)
        {

        }

        /// <summary>
        /// 接管场景中关注对象的FixedUpdate
        /// </summary>
        public virtual void FixedUpdate(float deltaTime)
        {

        }

    }
}

