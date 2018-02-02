<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnnounceList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.AnnounceList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="query" runat="server">
        <h3 class="titleBg">
            公告管理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w35" />
                    <col class="w35" />
                    <col class="w30" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">发布时间：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({skin:'default',isShowClear:false,readOnly:true, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}'})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({skin:'default',isShowClear:false,readOnly:true,minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate: '2020-10-01'})">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">标题：</span><asp:TextBox runat="server" ID="txtTitle" class="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">发布人：</span><asp:TextBox runat="server" ID="txtPublishPerson" class="text"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                     <td>
                        <div class="input">
                            <span class="name">审核状态：</span><asp:DropDownList runat="server" ID="dropAuditStatus">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <input type="button" id="btnAdd" value="添&nbsp;&nbsp;&nbsp;加" class="btn class1"
                            onclick="javascript:window.location.href='AnnounceAddOrUpdate.aspx'" />
                        <asp:Button runat="server" ID="btnQuery" class="btn class1 submit" Text="查&nbsp;&nbsp;&nbsp;询"
                            OnClick="btnQuery_Click" />
                        <asp:Button runat="server" ID="btnAduit" CssClass="btn class1" Text="审&nbsp;&nbsp;&nbsp;核"
                            OnClientClick="return showCheckbox('审核');" OnClick="btnAduit_Click" />
                        <asp:Button runat="server" ID="btnCancelAduit" CssClass="btn class1" Text="取消审核"
                            OnClientClick="return showCheckbox('取消审核');" OnClick="btnCancelAduit_Click" />
                        <asp:Button runat="server" ID="btnDelete" CssClass="btn class1" Text="删除" OnClientClick="return showCheckbox('删除');"
                            OnClick="btnDelete_Click" />
                        <input type="button" onclick="ResetSearchOption()" class="btn class2" id="btnReset"
                            value="清空条件" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id='data-list'>
        <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" OnRowCommand="dataSource_RowCommand">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="chk_all" type="checkbox" onclick="checkAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkBox" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="标题" DataField="Title" />
                <asp:BoundField HeaderText="发布时间" DataField="PublishTime" />
                <asp:BoundField HeaderText="发布人" DataField="PublishAccount" />
                <asp:BoundField HeaderText="审核状态" DataField="AduiteStatus" />
                <asp:BoundField HeaderText="公告类型" DataField="AnnounceType" />
                <asp:TemplateField HeaderText="公告范围">
                  <ItemTemplate>
                      <asp:Label ID="lblAnnounceScope" runat="server" Text='<%#Eval("AnnouceScope") %>'  Visible='<%#(bool)Eval("IsPlatform") %>'></asp:Label>
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='AnnounceAddOrUpdate.aspx?announceId=<%#Eval("Id") %>'>修改</a>
                        <asp:LinkButton ID="lnkDel" CommandName="del" CommandArgument='<%#Eval("Id") %>'
                            runat="server" OnClientClick='return confirm("您确定要删除吗？")'>删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <asp:HiddenField ID="hfdDate" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script src="../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    function checkAll(all) {
        var checks = document.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox") {
                checks[i].checked = all.checked;
            }
        }
    }
    function showCheckbox(content) {
        var a = $(":checkbox:checked");
        if (a.length < 1) { alert("请先选择需要被" + content + "的数据行！"); return false; }
        var flag = confirm('确认要' + content + '吗？');
        if (flag == false) {
            return false;
        }
    }
</script>
<script type="text/javascript">
    $(function () {
        $("#btnQuery").click(function () {
            if ($.trim($("#txtTitle").val()).length > 100) {
                alert("标题格式错误！");
                $("#txtTitle").select();
                return false;
            }
            if ($.trim($("#txtPublishPerson").val()).length > 20) {
                alert("发布人格式错误！");
                $("#txtPublishPerson").select();
                return false;
            }
        });
        SaveDefaultData();
        //        $("#btnReset").click(function () {
        //            var dateTimeNow = $("#hfdDate").val();
        //            $("#txtStartDate").val(dateTimeNow);
        //            $("#txtEndDate").val(dateTimeNow);
        //            $("select option").eq(0).attr("selected", "selected");
        //        });
    })
</script>
