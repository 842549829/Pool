using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.ReleaseNote.Repository;
using ChinaPay.Core;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ReleaseNote
{
    /// <summary>
    /// 平台更新日志服务类
    /// </summary>
    public static class ReleaseNoteService
    {
        /// <summary>
        /// 查询带分页
        /// </summary>
        /// <param name="paination"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Domain.ReleaseNote> Query(Pagination paination, DateTime? startTime, DateTime? endTime, ChinaPay.B3B.Common.Enums.CompanyType? type, ReleaseNoteType? releaseType)
        {
            var repository = Factory.CreateReportRepository();
            return repository.Query(paination, startTime, endTime, type, releaseType);
        }
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Domain.ReleaseNote Query(Guid id)
        {
            var repository = Factory.CreateReportRepository();
            return repository.Query(id);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="note"></param>
        public static void Update(Domain.ReleaseNote note, string operatorAccount)
        {
            var repository = Factory.CreateReportRepository();
            var n = repository.Query(note.Id);
            note.CreateTime = n.CreateTime;
            repository.Update(note);
            string str = "";
            if ((CompanyType.Provider & note.Type.Value) > 0)
            {
                str += "前台可见";
            }
            if ((CompanyType.Purchaser & note.Type.Value) > 0)
            {
                str += "后台可见";
            }
            saveUpdateLog(string.Format("原内容  编号：{0},创建时间：{1},网站更新日志操作者：{2},网站日志更新时间：{3},标题：{4},内容：{5},显示类型：{6}", note.Id, note.CreateTime, note.Creator, note.UpdateTime, note.Title, note.Context,
                note.Type.HasValue ? note.ReleaseType == ReleaseNoteType.B3BVisible ? string.Join("/",
                Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Where(e => (e & note.Type.Value) > 0).Select(e => e.GetDescription()))
                : str : string.Empty)
                + string.Format("新内容  编号：{0},创建时间：{1},网站更新日志操作者：{2},网站日志更新时间：{3},标题：{4},内容：{5},显示类型：{6}", n.Id, n.CreateTime, n.Creator, n.UpdateTime, n.Title, n.Context,
                note.Type.HasValue ? note.ReleaseType == ReleaseNoteType.B3BVisible ? string.Join("/",
                 Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Where(e => (e & note.Type.Value) > 0).Select(e => e.GetDescription()))
                 : str : string.Empty
                ), OperatorRole.Platform, note.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="note"></param>
        public static void Add(Domain.ReleaseNote note, string operatorAccount)
        {
            note.Id = Guid.NewGuid();
            note.CreateTime = DateTime.Now;
            var repository = Factory.CreateReportRepository();
            repository.Add(note);
            string str = "";
            if ((CompanyType.Provider & note.Type.Value) > 0)
            {
                str += "前台可见";
            }
            if ((CompanyType.Purchaser & note.Type.Value) > 0)
            {
                str += "后台可见";
            }
            saveAddLog(string.Format("编号：{0},创建时间：{1},网站更新日志操作者：{2},网站日志更新时间：{3},标题：{4},内容：{5},显示类型：{6}", note.Id, note.CreateTime, note.Creator, note.UpdateTime, note.Title, note.Context,
                note.Type.HasValue ? note.ReleaseType == ReleaseNoteType.B3BVisible ?  string.Join("/",
                Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Where(e => (e & note.Type.Value) > 0).Select(e => e.GetDescription())) : str : string.Empty), OperatorRole.Platform, note.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(Guid id, string operatorAccount)
        {
            var repository = Factory.CreateReportRepository();
            var n = repository.Query(id);
            repository.Delete(id);
            string str = "";
            if ((CompanyType.Provider & n.Type.Value) > 0)
            {
                str += "前台可见";
            }
            if ((CompanyType.Purchaser & n.Type.Value) > 0)
            {
                str += "后台可见";
            }
            saveDeleteLog(string.Format("编号：{0},创建时间：{1},网站更新日志操作者：{2},网站日志更新时间：{3},标题：{4},内容：{5},显示类型：{6}", n.Id, n.CreateTime, n.Creator, n.UpdateTime, n.Title, n.Context,
                n.Type.HasValue ? n.ReleaseType == ReleaseNoteType.B3BVisible ? string.Join("/",
                Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Where(e => (e & n.Type.Value) > 0).Select(e => e.GetDescription())) : str : string.Empty
                ), OperatorRole.Platform, n.Id.ToString(), operatorAccount);
        }

        #region "日志"
        static void saveAddLog(string itemName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + "。", role, key, account);
        }
        static void saveUpdateLog(string itemName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。", itemName), role, key, account);
        }
        static void saveDeleteLog(string itemName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。", role, key, account);
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.网站更新日志, operationType, account, role, key, content);
            LogService.SaveOperationLog(log);
        }
        #endregion


    }
}
